using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

//
//  @TODO: Move this into its own script that exists on the tower prefab
//
//
//  Idea is to use coroutines to increase the 't' value in the shader
//
//  Creation:
//      1. Get a list of objects to fade in (@TODO: figure out where to cache this for easy ref, TowerPart?)
//      2. Go through the list and set the material to the dissolve material
//      3. Run coroutine to drive 't' from 0 -> 1
//      4. Go through the list and set the material back to normal
//
//
//  Deletion:
//      1. Get that same list from step 1 of creation
//      2. Go through the list and set the material to the dissolve material
//      3. Run coroutine to drive 't' from 1 -> 0
//      4. Go through the list and destroy the objects
//


[Serializable]
public struct TowerPiece
{
    public List<TowerPart> layers;
}

public class Tower : MonoBehaviour
{
    public Transform buildSpot;
    public GameObject weapon;
    private GameObject weaponInstance;
    public Material dissolveMaterial;

    [Range(0, 3)]
    public int level;

    public List<TowerPiece> pieces = new();

    public float upgradeDebounceTime = 1f;
    private float upgradeTimer = 0f;

    public void IncLevel()
    {
        if (upgradeTimer < upgradeDebounceTime)
        {
            return;
        }
        level += 1;
        level %= pieces.Count;
        StartCoroutine(DissolveTowerAndRebuild());
    }

    public void DecLevel()
    {
        if (upgradeTimer < upgradeDebounceTime)
        {
            return;
        }
        if (level == 0)
        {
            level = pieces.Count - 1;
        }
        else
        {
            level -= 1;
        }
        StartCoroutine(DissolveTowerAndRebuild());
    }

    void Update()
    {
        upgradeTimer += Time.deltaTime;
    }

    void Start()
    {
        StartCoroutine(BuildTower());
    }

    private IEnumerator BuildTower()
    {
        // Get a list of items to delete
        List<GameObject> objects = new();
        Transform spot = buildSpot;
        Material mat = null;
        for (int i = 0; i < pieces[level].layers.Count; i++)
        {
            var child = Instantiate(pieces[level].layers[i], buildSpot);
            child.transform.position += spot.transform.position;
            spot = child.buildSpot;
            objects.Add(child.gameObject);
        }
        if (weapon)
        {
            weaponInstance = Instantiate(weapon, buildSpot);
            weaponInstance.transform.position = spot.position;
            objects.Add(weaponInstance);
            objects.Add(weaponInstance.transform.GetChild(0).gameObject);
        }

        // Set their material to the dissolve material
        foreach (GameObject obj in objects)
        {
            if (mat == null)
            {
                mat = obj.GetComponent<Renderer>().material;
            }
            obj.GetComponent<Renderer>().material = dissolveMaterial;
        }

        // Run the dissolve 
        yield return StartCoroutine(Dissolve(1, 0));

        // Switch the material back
        foreach (GameObject obj in objects)
        {
            obj.GetComponent<Renderer>().material = mat;
        }
    }

    // @TODO: Figure out how to get a different 't' for each object.
    private IEnumerator DissolveTower()
    {
        // Get a list of items to delete
        List<GameObject> objectsToDelete = new();
        if (weaponInstance)
        {
            objectsToDelete.Add(weaponInstance);
            objectsToDelete.Add(weaponInstance.transform.GetChild(0).gameObject);
        }
        foreach (Transform child in buildSpot.transform)
        {
            objectsToDelete.Add(child.gameObject);
        }

        // Set their material to the dissolve material
        foreach (GameObject obj in objectsToDelete)
        {
            obj.GetComponent<Renderer>().material = dissolveMaterial;
        }

        // Run the dissolve 
        yield return StartCoroutine(Dissolve(0, 1));

        // Delete the items
        foreach (GameObject obj in objectsToDelete)
        {
            Destroy(obj);
        }
    }

    private IEnumerator DissolveTowerAndRebuild()
    {
        upgradeTimer = 0f;
        yield return StartCoroutine(DissolveTower());
        yield return StartCoroutine(BuildTower());
    }

    private IEnumerator Dissolve(int start, int end)
    {
        float elapse = 0f;
        float str;
        float duration = upgradeDebounceTime / 2f;
        while (elapse < duration)
        {
            elapse += Time.deltaTime;
            str = Mathf.Lerp(start, end, elapse / duration);
            dissolveMaterial.SetFloat("_t", str);
            yield return null;
        }
    }
}
