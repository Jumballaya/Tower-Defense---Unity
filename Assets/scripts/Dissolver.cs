using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

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


public class Dissolver : MonoBehaviour
{
    public Material dissolveMaterial;

    // @TODO: Figure out how to get a different 't' for each object.
    public IEnumerator Dissolve(int start, int end, float duration, List<GameObject> objects)
    {
        // Set their material to the dissolve material
        // Save the original material
        List<Material> originalMats = new();
        foreach (GameObject obj in objects)
        {
            originalMats.Add(obj.GetComponent<Renderer>().material);
            obj.GetComponent<Renderer>().material = dissolveMaterial;
        }

        // Run the dissolve 
        float elapse = 0f;
        float str;
        while (elapse < duration)
        {
            elapse += Time.deltaTime;
            str = Mathf.Lerp(start, end, elapse / duration);
            dissolveMaterial.SetFloat("_t", str);
            yield return null;
        }

        // Switch the material back
        for (int i = 0; i < originalMats.Count; i++)
        {
            objects[i].GetComponent<Renderer>().material = originalMats[i];
        }
    }
}
