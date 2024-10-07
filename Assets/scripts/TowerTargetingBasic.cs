using UnityEngine;

public class TowerTargetingBasic : Targeting
{
    public override void AcquireTarget()
    {
        if (currentTarget)
        {
            float dist = (currentTarget.position - tower.position).magnitude;
            if (dist > range)
            {
                currentTarget = null;
            }
        }
        if (currentTarget != null)
        {
            return;
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        GameObject closest = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject enemy in enemies)
        {
            float dist = (enemy.transform.position - tower.position).magnitude;
            if (dist > range)
            {
                continue;
            }
            if (closestDistance > dist)
            {
                closestDistance = dist;
                closest = enemy;
            }
        }
        if (closest != null)
        {
            currentTarget = closest.transform;
        }
    }
}
