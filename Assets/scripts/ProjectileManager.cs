using System.Collections.Generic;
using UnityEngine;

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

struct Projectile
{
    public CombatUnit origin;
    public CombatUnit target;
    public GameObject projectile;
    public float speed;
}

public class ProjectileManager : MonoBehaviour
{
    private static List<Projectile> projectilesInFlight = new();
    public static void FireProjectile(CombatUnit origin, CombatUnit target, GameObject projectile, float speed)
    {
        projectilesInFlight.Add(new Projectile()
        {
            origin = origin,
            target = target,
            projectile = Instantiate(projectile, origin.transform),
            speed = speed
        });
    }

    void Update()
    {
        List<Projectile> toDelete = new();
        foreach (Projectile p in projectilesInFlight)
        {
            if (p.target == null)
            {
                toDelete.Add(p);
                Destroy(p.projectile);
                continue;
            }

            Vector3 dir = p.target.transform.position - p.projectile.transform.position;
            float distThisFrame = p.speed * Time.deltaTime;
            if (dir.magnitude <= distThisFrame)
            {
                // Hit Target
                Destroy(p.projectile);
                toDelete.Add(p);
                continue;
            }
            p.projectile.transform.Translate(dir.normalized * distThisFrame, Space.World);
            p.projectile.transform.LookAt(p.target.transform);
        }
        foreach (Projectile p in toDelete)
        {
            projectilesInFlight.Remove(p);
        }
    }




#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        foreach (Projectile p in projectilesInFlight)
        {
            DrawCurveTo(p.projectile.transform);
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
