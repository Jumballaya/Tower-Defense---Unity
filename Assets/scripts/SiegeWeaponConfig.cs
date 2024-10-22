using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Siege Weapon Configuration", menuName = "Tower Defense/Siege Weapon Configuration")]
public class SiegeWeaponConfig : ScriptableObject
{
    public float dps;
    public ProjectileType projectileType;

    [Header("Render Settings")]
    public GameObject weaponBase;
    public GameObject weaponAttachment;

    private List<GameObject> _parts;
    private GameObject weaponBaseInstance;
    private GameObject weaponAttachmentInstance;
    public List<GameObject> Parts
    {
        get
        {
            if (_parts != null)
            {
                return _parts;
            }

            Queue<GameObject> queue = new();
            List<GameObject> parts = new();
            if (weaponBase)
            {
                queue.Enqueue(weaponBase);
                while (queue.Count > 0)
                {
                    var obj = queue.Dequeue();
                    parts.Add(obj);
                    for (int i = 0; i < obj.transform.childCount; i++)
                    {
                        queue.Enqueue(obj.transform.GetChild(i).gameObject);
                    }
                }
            }
            if (weaponAttachment)
            {
                queue.Enqueue(weaponAttachment);
                while (queue.Count > 0)
                {
                    var obj = queue.Dequeue();
                    parts.Add(obj);
                    for (int i = 0; i < obj.transform.childCount; i++)
                    {
                        queue.Enqueue(obj.transform.GetChild(i).gameObject);
                    }
                }
            }
            _parts = parts;
            return parts;
        }
    }

    public void SetupSiegeWeapon(SiegeWeapon weapon)
    {
        weapon.dps = dps;
        if (weaponBase != null)
        {
            if (weaponBaseInstance == null)
            {
                weaponBaseInstance = Instantiate(weaponBase, weapon.transform);
            }
            weapon.weaponBase = weaponBaseInstance;

        }
        if (weaponAttachment != null)
        {
            if (weaponAttachmentInstance == null)
            {
                weaponAttachmentInstance = Instantiate(weaponAttachment, weapon.transform);
            }
            weapon.weaponBase = weaponAttachmentInstance;
        }
        weapon.parts = Parts;
        weapon.projectileType = projectileType;
    }
}
