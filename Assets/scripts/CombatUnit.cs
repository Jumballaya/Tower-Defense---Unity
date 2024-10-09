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

  [Header("Combat Internals")]
  public GameObject healthBarPrefab;
  public Vector3 healthBarOffset = new();
  private GameObject healthBarInstance;
  private HealthBar healthBar;

  protected void Initialize()
  {
    currentDPS = baseDPS;
    currentArmor = baseArmor;
    currentHealth = baseHealth;
    healthBarInstance = Instantiate(healthBarPrefab, transform);
    healthBar = healthBarInstance.GetComponent<HealthBar>();
    healthBar.SetPercent(1f);
    healthBar.MoveTo(transform.position + healthBarOffset);
  }

  protected void UpdateUnit()
  {
    attackTimer += Time.deltaTime;
  }

  protected void MoveHealthBar(Transform t)
  {
    healthBar.MoveTo(t.position + healthBarOffset);
  }

  public void GetAttackedBy(CombatUnit unit)
  {
    float mult = unit.currentDPS / currentArmor;
    float damage = 1f / unit.attackRate * GetDPS() * mult;
    TakeDamage(damage);
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
    unit.GetAttackedBy(this);
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
    healthBar.SetPercent(currentHealth / baseHealth);
  }
}
