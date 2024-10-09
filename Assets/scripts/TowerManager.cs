using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Rendering;



#if UNITY_EDITOR
using UnityEditor;
#endif

public class TowerManager : MonoBehaviour
{
    static List<Tower> towers = new();

    public static void AddTower(Tower t)
    {
        towers.Add(t);
    }

    public static void RemoveTower(Tower t)
    {
        towers.Remove(t);
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
