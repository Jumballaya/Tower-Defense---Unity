using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{

    public GameObject indicatorPrefab;
    private GameObject indicator;

    public GameObject levelManager;

    public void Start()
    {
        var pos = transform.position + new Vector3(0f, 1f, 0f);
        indicator = Instantiate(indicatorPrefab, pos, Quaternion.identity);
        var mesh = indicator.GetComponent<MeshRenderer>();
        mesh.enabled = false;

        if (levelManager == null)
        {
            var levelManagers = GameObject.FindGameObjectsWithTag("LevelManager");
            if (levelManagers.Length == 0)
            {
                // PANIC!
                print("We don't have a level manager");
                return;
            }
            levelManager = levelManagers[0];
        }
    }

    public void OnMouseDown()
    {
        var comp = levelManager.GetComponent<LevelManager>();
        print(comp);
        comp.RotateTile(0, 0, true);
        print("Rotate");
    }

    public void OnMouseEnter()
    {
        var mesh = indicator.GetComponent<MeshRenderer>();
        mesh.enabled = true;
    }

    public void OnMouseExit()
    {
        var mesh = indicator.GetComponent<MeshRenderer>();
        mesh.enabled = false;
    }
}
