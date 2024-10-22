using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum ProjectileType
{
    Arrow,
    Bolt,
    Boulder,
    CannonBall,
    Bullet
};

public class ProjectileManager : MonoBehaviour
{
    // Singleton Instance
    private static ProjectileManager instance;
    public static ProjectileManager GetInstance() => instance;

    public ProjectileManagerConfig config;
    public Projectile prefab;
    private ObjectPool projectilePool;
    private List<PoolableObject> projectileList = new();

    void Start()
    {
        projectilePool = ObjectPool.CreateInstance(prefab, 50, transform);
        projectilePool.spawnOption = ObjectPoolSpawnOption.CreateNew;
    }

    public IEnumerator FireProjectile(Transform origin, Transform target, ProjectileType projectile, float speed)
    {
        var proj = projectilePool.GetObject(config.GetConfig(projectile));
        proj.transform.SetPositionAndRotation(origin.position, origin.rotation);
        projectileList.Add(proj);
        while (true)
        {
            if (target == null) break;
            Vector3 dir = target.position - proj.transform.position;
            float distThisFrame = speed * Time.deltaTime;
            if (dir.magnitude <= distThisFrame) break; // Hit

            proj.transform.Translate(dir.normalized * distThisFrame, Space.World);
            proj.transform.LookAt(target);

            yield return null;
        }
        projectileList.Remove(proj);
        proj.gameObject.SetActive(false);
    }

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one Projectile Manager in the scene");
        }
        instance = this;
    }


#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        foreach (PoolableObject p in projectileList)
        {
            DrawCurveTo(p.transform);
        }
    }

    private void DrawCurveTo(Transform t)
    {
        Vector3 managerPos = transform.position;
        Vector3 enemyPos = t.position;
        float halfHeight = (managerPos.y - enemyPos.y) * 0.5f;
        Vector3 offset = Vector3.up * halfHeight;
        Handles.DrawBezier(
            managerPos,
            enemyPos,
            managerPos - offset,
            enemyPos + offset,
            Color.green,
            EditorGUIUtility.whiteTexture,
            1f
        );
    }
#endif
}
