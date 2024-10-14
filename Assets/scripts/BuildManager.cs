
using UnityEngine;

class BuildManager : MonoBehaviour
{
  private static BuildManager instance;
  public static BuildManager GetInstance() => instance;

  public GameObject buildMenuPrefab;
  public Vector3 buildMenuOffset;
  private GameObject buildMenu;


  void Awake()
  {
    if (instance != null)
    {
      Debug.Log("More than one Build Manager in the scene");
    }
    instance = this;
  }

  void Start()
  {
    buildMenu = Instantiate(buildMenuPrefab, transform);
    buildMenu.SetActive(false);
  }

  public void ShowMenu(Transform transform)
  {
    buildMenu.SetActive(true);
    buildMenu.transform.position = transform.position + buildMenuOffset;
  }

  public void HideMenu()
  {
    buildMenu.SetActive(false);
  }

  public void UpgradeUnit()
  {
    HideMenu();
  }

  public void BuySeigeWeapon()
  {
    HideMenu();
  }
}