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
    public Transform origin;
    public CombatUnit target;
    public GameObject projectile;
    public float speed;
}


public class ProjectileManager : MonoBehaviour
{
    private static ProjectileManager instance;
    public static ProjectileManager GetInstance()
    {
        return instance;
    }

    private List<Projectile> projectilesInFlight = new();

    // @TODO: Turn this into an IEnumerator
    //
    //  The reasoning is that we might want to fire a projectile as a part of a series of actions.
    //  It could go like so:
    //      1. Fire Projectile, wait for it to hit
    //      2. Do damage
    //      3. Some sort of effect (like an explosion)
    //      4. Start a kill unit effect
    //      5. Destroy the unit
    //
    //  That could look like:
    //
    //      ...
    //      yield return projectileManager.FireProjectile(...);
    //      from.DoDamageTo(to);
    //      yield return from.DieEffect();
    //      from.Die();
    //      ...
    //
    //
    // @TODO: Use the enum above to create the projectile prefab instead of passing it in.
    // @TODO: Create a pool of each projectile and pull from the pool instead of creating/deleting a new projectile each time
    public void FireProjectile(Transform origin, CombatUnit target, GameObject projectile, float speed)
    {
        projectilesInFlight.Add(new Projectile()
        {
            target = target,
            projectile = Instantiate(projectile, origin),
            speed = speed
        });
    }

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one Projectile Manager in the scene");
        }
        instance = this;
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
