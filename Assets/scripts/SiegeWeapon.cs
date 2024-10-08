using System.Collections.Generic;
using UnityEngine;

//
//  @TODO: Rework the heirarchy
//
//          1. Dedicated child for meshes
//          2. Build a list of children for dissolve purposes
//          3. Have Siege Weapon attack separately from the tower's archers
//          4. Have a RotatePoint
//          5. The main transform should always be at the bottom of the model
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
