using System.Collections.Generic;
using UnityEngine;

public enum ObjectPoolSpawnOption
{
    ReturnNull,
    CreateNew,
}

public class ObjectPool
{
    public ObjectPoolSpawnOption spawnOption = ObjectPoolSpawnOption.ReturnNull;
    private PoolableObject prefab;
    private List<PoolableObject> availableObjects;
    private Transform parent;

    private ObjectPool(PoolableObject prefab, Transform parent, int count)
    {
        this.prefab = prefab;
        this.parent = parent;
        availableObjects = new(capacity: count);
    }

    public static ObjectPool CreateInstance(PoolableObject prefab, int count, Transform parent)
    {
        ObjectPool pool = new(prefab, parent, count);
        pool.CreateObjects(count);
        return pool;
    }

    private void CreateObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PoolableObject obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
            obj.parent = this;
            obj.gameObject.SetActive(false);
            availableObjects.Add(obj);
        }
    }

    public void FreeObject(PoolableObject obj)
    {
        availableObjects.Add(obj);
        obj.transform.position = parent.transform.position;
    }

    public PoolableObject GetObject(ScriptableObject config)
    {
        if (availableObjects.Count > 0)
        {
            PoolableObject obj = availableObjects[0];
            if (config != null)
            {
                obj.Config = config;
            }
            availableObjects.RemoveAt(0);
            obj.gameObject.SetActive(true);
            return obj;
        }
        if (spawnOption == ObjectPoolSpawnOption.CreateNew)
        {
            PoolableObject newObj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
            if (config != null)
            {
                newObj.Config = config;
            }
            newObj.parent = this;
            return newObj;
        }
        return null;
    }
}
