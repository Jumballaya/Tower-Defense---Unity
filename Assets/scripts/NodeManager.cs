
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class NodeManager : MonoBehaviour
{
  private static NodeManager instance;
  public static NodeManager GetInstance() => instance;
  private List<Node> nodes = new();
  private Node selectedNode = null;

  void Awake()
  {
    if (instance != null)
    {
      Debug.Log("More than one Node Manager in the scene");
    }
    instance = this;
  }

  public void AddNode(Node n)
  {
    nodes.Add(n);
  }

  public void RemoveNode(Node n)
  {
    nodes.Remove(n);
  }

  public void SelectNode(Node n)
  {
    selectedNode = n;
  }

  public void DeselectNode()
  {
    selectedNode = null;
  }

  public Node GetSelected()
  {
    return selectedNode;
  }



#if UNITY_EDITOR
  void OnDrawGizmosSelected()
  {
    foreach (Node n in nodes)
    {
      DrawCurveTo(n.transform);
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
        Color.green,
        EditorGUIUtility.whiteTexture,
        1f
    );
  }
#endif

}