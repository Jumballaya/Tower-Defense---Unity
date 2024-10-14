using UnityEngine;
using UnityEngine.UI;

public class TowerMenu : MonoBehaviour
{
    public Button upgradeButton;
    public Button siegeWeaponButton;
    private BuildManager buildManager;

    void Awake()
    {
        buildManager = BuildManager.GetInstance();
        upgradeButton.onClick.AddListener(buildManager.UpgradeUnit);
        siegeWeaponButton.onClick.AddListener(buildManager.BuySeigeWeapon);
    }
}
