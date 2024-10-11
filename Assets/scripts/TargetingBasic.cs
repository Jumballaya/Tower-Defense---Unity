using UnityEngine;

public class TargetingBasic : Targeting
{
    public override void AcquireTarget()
    {
        // current target is dead, drop target
        if (currentTarget && currentTarget.GetHealth() <= 0f)
        {
            currentTarget = null;
        }

        // current target is out of range, drop target
        if (currentTarget)
        {
            float dist = (currentTarget.transform.position - self.transform.position).magnitude;
            if (dist > range)
            {
                currentTarget = null;
            }
        }

        // if we have still have a target, then we are done
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
