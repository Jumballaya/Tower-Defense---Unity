using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyManager : MonoBehaviour
{
    static List<Enemy> enemies = new();

    public static void AddEnemy(Enemy e)
    {
        enemies.Add(e);
    }

    public static void RemoveEnemy(Enemy e)
    {
        enemies.Remove(e);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        foreach (Enemy e in enemies)
        {
            DrawCurveTo(e.transform);
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
            Color.red,
            EditorGUIUtility.whiteTexture,
            1f
        );
    }
#endif
}
