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

public class Tower : CombatUnit
{
    [Header("Tower Attributes")]
    [Range(0, 3)]
    public int upgradeLevel;
    public GameObject siegeWeaponPrefab;
    private SiegeWeapon siegeWeapon;

    [Header("Tower Internals")]
    public Transform buildSpot;
    public Dissolver dissolver;
    public Targeting targeting;
    public List<TowerPiece> pieces = new();
    public GameObject towerWeapon; // arrows, cannon balls, etc. just not the siege weapon on top

    public float upgradeDebounceTime = 0.5f;
    private float upgradeTimer = 0f;

    private Transform attackSpot;

    void Update()
    {
        UpdateUnit();
        upgradeTimer += Time.deltaTime;
        targeting.AcquireTarget();
        if (IsUpgrading)
        {
            return;
        }
        if (!targeting.HasTarget())
        {
            return;
        }
        CombatUnit currTarget = targeting.GetTarget();
        AttackState state = Attack(currTarget);
        if (state == AttackState.CanAttack)
        {
            projectileManager.FireProjectile(attackSpot, currTarget, towerWeapon, 10f);
        }
    }

    void Start()
    {
        Initialize();
        targeting.targetTag = "Enemy";
        attackSpot = buildSpot;
        StartCoroutine(BuildTower());
    }

    void OnEnable() => TowerManager.AddTower(this);
    void OnDisable() => TowerManager.RemoveTower(this);

    public void Upgrade()
    {
        if (IsUpgrading)
        {
            return;
        }
        if (CanUpgrade)
        {
            upgradeLevel += 1;
            TowerPiece piece = pieces[upgradeLevel];
            baseDPS += piece.dpsIncrease;
            baseArmor += piece.armorIncrease;
            baseHealth += piece.healthIncrease;
            StartCoroutine(DissolveTowerAndRebuild());
        }
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
        if (siegeWeaponPrefab)
        {
            GameObject weaponInstance = Instantiate(siegeWeaponPrefab, buildSpot);
            weaponInstance.transform.position = spot.position;
            SiegeWeapon sw = weaponInstance.GetComponent<SiegeWeapon>();
            siegeWeapon = sw;
            objects.AddRange(siegeWeapon.GetGameObjects());
        }
        MoveHealthBar(spot);
        attackSpot = spot;

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

    private bool IsUpgrading
    {
        get
        {
            return upgradeTimer < (upgradeDebounceTime * 2f);
        }
    }

    private bool CanUpgrade
    {
        get
        {
            return upgradeLevel < pieces.Count - 1;
        }
    }
}