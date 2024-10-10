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


    [Serializable]
    public struct ProjectilePrefabMap
    {
        public GameObject arrowPrefab;
        public GameObject boltPrefab;
        public GameObject boulderPrefab;
        public GameObject cannonBallPrefab;
        public GameObject bulletPrefab;
    };

    public ProjectilePrefabMap prefabMap;
    private List<GameObject> projectileList = new();


    // @TODO: Create a pool of each projectile and pull from the pool instead of creating/deleting a new projectile each time
    public IEnumerator FireProjectile(Transform origin, CombatUnit target, ProjectileType projectile, float speed)
    {
        GameObject proj = InstantiateProjectile(projectile, origin);
        projectileList.Add(proj);
        while (true)
        {
            if (target == null) break;

            Vector3 dir = target.transform.position - proj.transform.position;
            float distThisFrame = speed * Time.deltaTime;
            if (dir.magnitude <= distThisFrame) break; // Hit

            proj.transform.Translate(dir.normalized * distThisFrame, Space.World);
            proj.transform.LookAt(target.transform);

            yield return null;
        }
        projectileList.Remove(proj);
        Destroy(proj);
    }

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one Projectile Manager in the scene");
        }
        instance = this;
    }


    private GameObject InstantiateProjectile(ProjectileType type, Transform origin)
    {
        switch (type)
        {
            case ProjectileType.Arrow:
                {
                    return Instantiate(prefabMap.arrowPrefab, origin);
                }
            case ProjectileType.Bolt:
                {
                    return Instantiate(prefabMap.boltPrefab, origin);
                }
            case ProjectileType.Boulder:
                {
                    return Instantiate(prefabMap.boulderPrefab, origin);
                }
            case ProjectileType.Bullet:
                {
                    return Instantiate(prefabMap.bulletPrefab, origin);
                }
            case ProjectileType.CannonBall:
                {
                    return Instantiate(prefabMap.cannonBallPrefab, origin);
                }
        }
        return null;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        foreach (GameObject p in projectileList)
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
