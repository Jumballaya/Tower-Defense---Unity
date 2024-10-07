using UnityEngine;

//
// Eventually build this out to use some sort of 'targetable' and 'damageable' interfaces
//
//
//

public abstract class Targeting : MonoBehaviour
{
    [Header("Attributes")]
    public float range = 10f;

    [Header("Internal")]
    public string enemyTag;
    public Transform tower;
    public Transform currentTarget;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public bool HasTarget()
    {
        return currentTarget != null;
    }

    public Transform GetTarget()
    {
        return currentTarget;
    }

    // Targeting logic
    public abstract void AcquireTarget();
}
