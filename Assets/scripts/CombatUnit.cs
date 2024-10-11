using System.Collections;
using UnityEngine;

public enum AttackState
{
  Attacking, // in the middle of the swing timer
  CanAttack, // swing timer done
}

public class CombatUnit : MonoBehaviour
{
  [Header("Combat Attributes")]
  public float baseDPS;
  private float currentDPS;
  public float baseHealth;
  private float currentHealth;
  public float baseArmor;
  private float currentArmor;
  public float attackRate;
  private float attackTimer = 0f;

  [Header("Combat Internals")]
  public Transform attackSpot;
  public GameObject healthBarPrefab;
  public Vector3 healthBarOffset = new();
  public Vector3 healthBarScale = new Vector3(1f, 1f, 1f);
  private GameObject healthBarInstance;
  private HealthBar healthBar;

  protected ProjectileManager projectileManager;


  protected void Initialize()
  {
    currentDPS = baseDPS;
    currentArmor = baseArmor;
    currentHealth = baseHealth;
    healthBarInstance = Instantiate(healthBarPrefab, transform);
    healthBar = healthBarInstance.GetComponent<HealthBar>();
    healthBar.SetPercent(1f);
    healthBar.MoveTo(transform.position + healthBarOffset);
    healthBar.Scale(healthBarScale);
    projectileManager = ProjectileManager.GetInstance();
  }

  protected void UpdateUnit()
  {
    attackTimer += Time.deltaTime;
  }

  protected void MoveHealthBar(Transform t)
  {
    healthBar.MoveTo(t.position + healthBarOffset);
  }

  protected void InitiateAttack(CombatUnit target, ProjectileType type)
  {
    AttackState state = GetAttackState();
    if (target != null && state == AttackState.CanAttack)
    {
      StartCoroutine(AttackTarget(target, type));
    }
  }

  private IEnumerator AttackTarget(CombatUnit currTarget, ProjectileType type)
  {
    StartAttacking();
    yield return projectileManager.FireProjectile(attackSpot, currTarget.attackSpot, type, 10f);
    DoDamage(currTarget);
  }

  protected AttackState GetAttackState()
  {
    if (attackTimer < 1f / attackRate)
    {
      return AttackState.Attacking;
    }
    return AttackState.CanAttack;
  }

  protected void GetAttackedBy(CombatUnit attacker)
  {
    float mult = attacker.currentDPS / currentArmor;
    float damage = 1f / attacker.attackRate * attacker.GetDPS() * mult;
    TakeDamage(damage);
  }


  protected void StartAttacking()
  {
    AttackState state = GetAttackState();
    if (state == AttackState.CanAttack)
    {
      attackTimer = 0f;
    }
  }

  protected void DoDamage(CombatUnit target)
  {
    if (GetAttackState() != AttackState.Attacking)
    {
      return;
    }
    target.GetAttackedBy(this);
    return;
  }

  public float GetHealth()
  {
    return currentHealth;
  }

  public void UpgradeHealth(float amount)
  {
    baseHealth += amount;
    currentHealth += amount;
  }

  public float GetArmor()
  {
    return currentArmor;
  }

  public void UpgradeArmor(float amount)
  {
    baseArmor += amount;
    currentArmor += amount;
  }

  public float GetDPS()
  {
    return currentDPS;
  }

  public void UpgradeDPS(float amount)
  {
    baseDPS += amount;
    currentDPS += amount;
  }

  private void TakeDamage(float damage)
  {
    currentHealth -= damage;
    healthBar.SetPercent(currentHealth / baseHealth);
  }

  protected virtual IEnumerator Die()
  {
    Destroy(gameObject);
    yield return null;
  }
}
