using UnityEngine;

public class TowerTargetingBasic : Targeting
{
    public override void AcquireTarget()
    {
        if (currentTarget)
        {
            float dist = (currentTarget.transform.position - self.transform.position).magnitude;
            if (dist > range)
            {
                currentTarget = null;
            }
        }
        if (currentTarget != null)
        {
            return;
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);

        GameObject closest = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject enemy in enemies)
        {
            float dist = (enemy.transform.position - self.transform.position).magnitude;
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
            currentTarget = closest.GetComponent<CombatUnit>();
        }
    }
}
