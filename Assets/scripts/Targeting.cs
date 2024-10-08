using UnityEngine;

public abstract class Targeting : MonoBehaviour
{
    [Header("Attributes")]
    public float range = 10f;

    [Header("Internal")]
    public string targetTag;
    public CombatUnit self;
    public CombatUnit currentTarget;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public bool HasTarget()
    {
        return currentTarget != null;
    }

    public CombatUnit GetTarget()
    {
        return currentTarget;
    }

    // Targeting logic
    public abstract void AcquireTarget();
}
