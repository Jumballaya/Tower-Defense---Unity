using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

//
//
//  @TODO: Think about moving the dissolve functionality to another class
//
//
public class Tower : CombatUnit
{
    [Serializable]
    public struct TowerPiece
    {
        public List<TowerPart> parts;
    }

    [Header("Tower Attributes")]
    public GameObject siegeWeaponPrefab;
    private SiegeWeapon siegeWeapon;
    public ProjectileType towerWeapon; // arrows, cannon balls, etc. just not the siege weapon on top

    [Header("Tower Internals")]
    public Transform buildSpot;
    public Dissolver dissolver;
    public Targeting targeting;

    public UnitUpgrader upgrader;
    public List<TowerPiece> pieces = new();

    void Update()
    {
        UpdateUnit();
        upgrader.UpdateTimer();
        if (upgrader.IsUpgrading)
        {
            return;
        }
        targeting.AcquireTarget();
        if (!targeting.HasTarget())
        {
            return;
        }
        CombatUnit currTarget = targeting.GetTarget();
        InitiateAttack(currTarget, towerWeapon);
    }

    void Start()
    {
        Initialize();
        targeting.targetTag = "Enemy";
        StartCoroutine(BuildTower());
    }

    void OnEnable() => TowerManager.AddTower(this);
    void OnDisable() => TowerManager.RemoveTower(this);

    public void Upgrade()
    {
        bool upgraded = upgrader.Upgrade(this);
        if (upgraded)
        {
            StartCoroutine(DissolveTowerAndRebuild());
        }
    }

    // BUILDING/ASSEMBLY LOGIC (dissolve)
    private IEnumerator BuildTower()
    {
        List<GameObject> objects = new();
        Transform spot = buildSpot;
        int upgradeLevel = upgrader.GetUpgradeLevel();
        for (int i = 0; i < pieces[upgradeLevel].parts.Count; i++)
        {
            var child = Instantiate(pieces[upgradeLevel].parts[i], buildSpot);
            child.transform.position = spot.transform.position;
            spot = child.buildSpot;
            objects.Add(child.gameObject);
        }
        if (siegeWeaponPrefab)
        {
            GameObject weaponInstance = Instantiate(siegeWeaponPrefab, buildSpot);
            weaponInstance.transform.position = spot.position;
            SiegeWeapon sw = weaponInstance.GetComponent<SiegeWeapon>();
            siegeWeapon = sw;
            objects.AddRange(siegeWeapon.GetGameObjects());
        }
        MoveHealthBar(spot);
        attackSpot.position = spot.position;

        yield return StartCoroutine(dissolver.Dissolve(1, 0, 1f, objects));
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
        yield return StartCoroutine(dissolver.Dissolve(0, 1, 1f, objectsToDelete));

        // Delete the items
        foreach (GameObject obj in objectsToDelete)
        {
            Destroy(obj);
        }
    }

    private IEnumerator DissolveTowerAndRebuild()
    {
        upgrader.ResetTimer();
        yield return StartCoroutine(DissolveTower());
        yield return StartCoroutine(BuildTower());
    }

    // From CombatUnit
    protected override IEnumerator Die()
    {
        Destroy(gameObject);
        yield return null;
    }
}