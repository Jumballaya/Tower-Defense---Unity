using UnityEngine;


[ExecuteAlways]
public class Enemy : CombatUnit
{
  [Header("Internals")]
  public Targeting targeting;


  void Update()
  {
    UpdateUnit();
  }

  void Start()
  {
    Initialize();
  }

  void OnEnable() => EnemyManager.AddEnemy(this);
  void OnDisable() => EnemyManager.RemoveEnemy(this);

}
