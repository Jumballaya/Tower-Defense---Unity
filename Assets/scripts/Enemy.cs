using UnityEngine;
using System.Collections;

public class Enemy : CombatUnit
{
  [Header("Enemy Attributes")]
  public ProjectileType projectileType;
  [Header("Internals")]
  public Targeting targeting;

  void Update()
  {
    UpdateUnit();
    targeting.AcquireTarget();
    if (!targeting.HasTarget())
    {
      return;
    }
    CombatUnit currTarget = targeting.GetTarget();
    InitiateAttack(currTarget, projectileType);
  }


  void Start()
  {
    Initialize();
  }

  void OnEnable() => EnemyManager.AddEnemy(this);
  void OnDisable() => EnemyManager.RemoveEnemy(this);


  protected override IEnumerator Die()
  {
    Destroy(gameObject);
    yield return null;
  }
}
