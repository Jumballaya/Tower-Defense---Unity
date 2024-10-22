using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public ObjectPool parent;
    public virtual ScriptableObject Config { get; set; }

    public virtual void OnDisable()
    {
        parent.FreeObject(this);
    }
}
