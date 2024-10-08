using UnityEngine;

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

  protected void Initialize()
  {
    currentDPS = baseDPS;
    currentArmor = baseArmor;
    currentHealth = baseHealth;
  }

  protected void UpdateUnit()
  {
    attackTimer += Time.deltaTime;
  }

  public void GetAttacked(CombatUnit unit)
  {
    float damage = (1f / attackRate) * GetDPS() * Time.deltaTime;
    unit.TakeDamage(damage);
  }

  public float GetDPS()
  {
    return currentDPS;
  }

  protected void Attack(CombatUnit unit)
  {
    if (attackTimer < 1f / attackRate)
    {
      return;
    }
    attackTimer = 0f;
    unit.GetAttacked(this);
  }

  protected float GetHealth()
  {
    return currentHealth;
  }

  protected void AdjustHealth(float amount)
  {
    baseHealth += amount;
  }

  protected float GetArmor()
  {
    return currentArmor;
  }

  protected void AdjustArmor(float amount)
  {
    baseArmor += amount;
  }

  private void TakeDamage(float damage)
  {
    currentHealth -= damage;
  }
}
