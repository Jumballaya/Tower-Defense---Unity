using System.Collections.Generic;
using UnityEngine;

//
//  @TODO: Rework the heirarchy
//
//          - Have Siege Weapon attack separately from the tower's archers
//          - Have a RotatePoint
//          - The main transform should always be at the bottom of the model
//

public class SiegeWeapon : MonoBehaviour
{
    [Header("Configuration")]
    public SiegeWeaponConfig config;

    [Header("Attributes")]
    public float dps = 10f;
    public ProjectileType projectileType = ProjectileType.Bolt;

    [Header("Internals")]
    public List<GameObject> parts;
    public GameObject weaponBase;
    public GameObject weaponAttachment;

    public void Start()
    {
        config.SetupSiegeWeapon(this);

    }

    public void OnEnable()
    {
        config.SetupSiegeWeapon(this);
    }

    public List<GameObject> GetGameObjects()
    {
        return parts;
    }
}
