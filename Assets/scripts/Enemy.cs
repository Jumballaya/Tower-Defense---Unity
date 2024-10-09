using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Enemy : CombatUnit
{
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
    if (targeting.HasTarget())
    {
      Attack(targeting.GetTarget());
    }
  }

  void Start()
  {
    Initialize();
  }

  void OnEnable() => EnemyManager.AddEnemy(this);
  void OnDisable() => EnemyManager.RemoveEnemy(this);


  IEnumerator Die()
  {
    Destroy(gameObject);
    yield return null;
  }
}
