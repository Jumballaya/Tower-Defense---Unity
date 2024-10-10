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
    [Header("Attributes")]
    public float dps;

    [Header("Internals")]
    public List<GameObject> parts;
    public Transform weaponBase;
    public Transform weaponAttachment;

    public List<GameObject> GetGameObjects()
    {
        return parts;
    }
}
