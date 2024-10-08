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
    private readonly int shaderId = Shader.PropertyToID("_t");

    private MaterialPropertyBlock mpb;

    public MaterialPropertyBlock Mpb
    {
        get
        {
            mpb ??= new MaterialPropertyBlock();
            return mpb;
        }
    }

    struct DissolveEntry
    {
        public Material material;
        public Renderer renderer;
    };
    public IEnumerator Dissolve(int start, int end, float duration, List<GameObject> objects)
    {
        // Set their material to the dissolve material
        // Save the original material
        List<DissolveEntry> entries = new();
        foreach (GameObject obj in objects)
        {
            if (obj.TryGetComponent(out Renderer rend))
            {
                rend.sharedMaterial = dissolveMaterial;
                rend.SetPropertyBlock(Mpb);
                entries.Add(new DissolveEntry()
                {
                    material = rend.sharedMaterial,
                    renderer = rend,
                });
            }
        }

        // Run the dissolve 
        float elapse = 0f;
        float str;
        while (elapse < duration)
        {
            elapse += Time.deltaTime;
            str = Mathf.Lerp(start, end, elapse / duration);
            Mpb.SetFloat("_t", str);
            foreach (var entry in entries)
            {
                entry.renderer.SetPropertyBlock(Mpb);
            }

            yield return null;
        }

        // Switch the material back
        for (int i = 0; i < entries.Count; i++)
        {
            if (objects[i].TryGetComponent(out Renderer rend))
            {
                rend.sharedMaterial = entries[i].material;
            }
        }
    }
}
