using System;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class TowerManager : MonoBehaviour
{
    [Serializable]
    public struct TowerPrefabMap
    {
        public GameObject basicTower;
        public GameObject basicMagicTower;
    };
    public TowerPrefabMap prefabMap;

    private static TowerManager instance;
    public static TowerManager GetInstance() => instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one Tower Manager in the scene");
        }
        instance = this;
    }

    private List<Tower> towers = new();

    public void AddTower(Tower t)
    {
        towers.Add(t);
    }

    public void RemoveTower(Tower t)
    {
        towers.Remove(t);
    }

    public GameObject CreateTower(Transform parent)
    {
        Destroy(parent.GetChild(0).gameObject);
        return Instantiate(prefabMap.basicTower, parent);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        foreach (Tower t in towers)
        {
            DrawCurveTo(t.transform);
        }
    }

    private void DrawCurveTo(Transform t)
    {
        Vector3 managerPos = transform.position;
        Vector3 towerPos = t.position;
        float halfHeight = (managerPos.y - towerPos.y) * 0.5f;
        Vector3 offset = Vector3.up * halfHeight;
        Handles.DrawBezier(
            managerPos,
            towerPos,
            managerPos - offset,
            towerPos + offset,
            Color.blue,
            EditorGUIUtility.whiteTexture,
            1f
        );
    }
#endif
}
