using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct UnitUpgrade
{
    public float dps;
    public float armor;
    public float health;
}

public class UnitUpgrader : MonoBehaviour
{
    public float upgradeCooldown = 0.5f;
    public List<UnitUpgrade> upgrades = new();
    private float upgradeTimer = 0f;
    private int upgradeLevel = 0;

    public int GetUpgradeLevel()
    {
        return upgradeLevel;
    }

    public void UpdateTimer()
    {
        upgradeTimer += Time.deltaTime;
    }

    public void ResetTimer()
    {
        upgradeTimer = 0f;
    }

    public bool Upgrade(CombatUnit unit)
    {
        if (IsUpgrading)
        {
            return false;
        }
        if (CanUpgrade)
        {
            upgradeLevel += 1;
            UnitUpgrade upgrade = upgrades[upgradeLevel];
            unit.UpgradeDPS(upgrade.dps);
            unit.UpgradeArmor(upgrade.armor);
            unit.UpgradeHealth(upgrade.health);
            return true;
        }
        return false;
    }

    public bool IsUpgrading
    {
        get
        {
            return upgradeTimer < upgradeCooldown;
        }
    }

    public bool CanUpgrade
    {
        get
        {
            return upgradeLevel < upgrades.Count - 1;
        }
    }
}
