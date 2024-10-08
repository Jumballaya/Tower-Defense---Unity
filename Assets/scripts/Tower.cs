using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;


[Serializable]
public struct TowerPiece
{
    public float dpsIncrease;
    public float healthIncrease;
    public float armorIncrease;
    public List<TowerPart> layers;
}

//
//
//  @TODO: Have Tower and Enemy inherit from a GameUnit class that
//         can be used for combat based stuff. It will hold DPS, HP, Armor, etc.
//         This will be the class that we can use for a health bar and for combat
//         and anything else that might be shared between the different units
//

public class Tower : CombatUnit
{
    [Header("Tower Attributes")]
    [Range(0, 3)]
    public int upgradeLevel;
    public GameObject weapon;
    private SiegeWeapon siegeWeapon;
    [Header("Internals")]
    public Transform buildSpot;
    public Dissolver dissolver;
    public Targeting targeting;
    public List<TowerPiece> pieces = new();

    public float upgradeDebounceTime = 0.5f;
    private float upgradeTimer = 0f;


    public void IncLevel()
    {
        if (upgradeTimer < (upgradeDebounceTime * 2f))
        {
            return;
        }
        upgradeLevel += 1;
        if (upgradeLevel > pieces.Count - 1)
        {
            upgradeLevel = pieces.Count - 1;
            return;
        }
        StartCoroutine(DissolveTowerAndRebuild());
    }

    public void DecLevel()
    {
        if (upgradeTimer < (upgradeDebounceTime * 2f))
        {
            return;
        }
        if (upgradeLevel == 0)
        {
            return;
        }
        else
        {
            upgradeLevel -= 1;
        }
        StartCoroutine(DissolveTowerAndRebuild());
    }

    void Update()
    {
        UpdateUnit();
        upgradeTimer += Time.deltaTime;
        targeting.AcquireTarget();
        if (targeting.HasTarget())
        {
            Attack(targeting.GetTarget());
        }
    }

    void Start()
    {
        Initialize();
        targeting.targetTag = "Enemy";
        StartCoroutine(BuildTower());
    }

    private IEnumerator BuildTower()
    {
        List<GameObject> objects = new();
        Transform spot = buildSpot;
        for (int i = 0; i < pieces[upgradeLevel].layers.Count; i++)
        {
            var child = Instantiate(pieces[upgradeLevel].layers[i], buildSpot);
            child.transform.position = spot.transform.position;
            spot = child.buildSpot;
            objects.Add(child.gameObject);
        }
        if (weapon)
        {
            GameObject weaponInstance = Instantiate(weapon, buildSpot);
            weaponInstance.transform.position = spot.position;
            SiegeWeapon sw = weaponInstance.GetComponent<SiegeWeapon>();
            siegeWeapon = sw;
            objects.AddRange(siegeWeapon.GetGameObjects());
        }

        yield return StartCoroutine(dissolver.Dissolve(1, 0, upgradeDebounceTime, objects));
    }

    private IEnumerator DissolveTower()
    {
        // Get a list of items to delete
        List<GameObject> objectsToDelete = new();
        if (siegeWeapon)
        {
            objectsToDelete.AddRange(siegeWeapon.GetGameObjects());
        }
        foreach (Transform child in buildSpot.transform)
        {
            objectsToDelete.Add(child.gameObject);
        }

        // Run the dissolve 
        yield return StartCoroutine(dissolver.Dissolve(0, 1, upgradeDebounceTime, objectsToDelete));

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

}