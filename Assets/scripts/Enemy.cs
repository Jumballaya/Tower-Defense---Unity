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

    if (GetHealth() <= 0f)
    {
      StartCoroutine(Die());
      return;
    }

    targeting.AcquireTarget();

    if (!targeting.HasTarget())
    {
      return;
    }
    CombatUnit currTarget = targeting.GetTarget();
    AttackState state = GetAttackState();
    if (currTarget != null && state == AttackState.CanAttack)
    {
      StartCoroutine(AttackTarget(currTarget));
    }
  }


  void Start()
  {
    Initialize();
  }

  void OnEnable() => EnemyManager.AddEnemy(this);
  void OnDisable() => EnemyManager.RemoveEnemy(this);


  private IEnumerator AttackTarget(CombatUnit currTarget)
  {
    StartAttacking();
    yield return projectileManager.FireProjectile(transform, currTarget, projectileType, 10f);
    DoDamage(currTarget);
  }

  IEnumerator Die()
  {
    Destroy(gameObject);
    yield return null;
  }
}
