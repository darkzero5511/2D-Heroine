using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamgable
{
    private Slider healthBar;
    private Entity_VFX entityVfx;
    private Entity entity;

    private Entity_Stats stats;

    [SerializeField] protected float currentHp;
    [SerializeField] protected bool isDead;

    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);

    [SerializeField] private Vector2 heavyKnockbackPower = new Vector2(7, 7);
    [SerializeField] private float knockbackDuration = .2f;
    [SerializeField] private float heavyKnockbackDuration = .5f;

    [Header("On Heavy Damage ")]
    [SerializeField] private float heavyDamageThreshold = .3f; // Percentage of health you should lose to consider damage as heavy

    protected virtual void Awake()
    {
        entityVfx = GetComponent<Entity_VFX>();
        entity = GetComponent<Entity>();
        stats = GetComponent<Entity_Stats>();

        healthBar = GetComponentInChildren<Slider>();

        currentHp = stats.GetMaxHealth();
        UpdateHealthBar();
    }

    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (isDead)
            return false;

        if (AttackEvaded())
        {
            Debug.Log($"{gameObject.name} evaded the attack!");
            return false;
        }

        //Amor Peneration
        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorPenetration = attackerStats != null ? attackerStats.GetArmorPenetration() : 0;

        //Armor
        float mitigation = stats.GetArmorMitigation(armorPenetration);
        float physicalDamageTaken = damage * (1 - mitigation);

        //Element
        float resistance = stats.GetElementalResistance(element);
        float elementalDamageTaken = elementalDamage * (1 - resistance);

        // Final Damage
        float finalDamage = physicalDamageTaken + elementalDamageTaken;

        //knockback
        TakeKnockback(damageDealer, finalDamage);

        ReduceHp(finalDamage);

        return true;
    }

    private void TakeKnockback(Transform damageDealer, float finalDamage)
    {
        Vector2 knockback = CalculateKnockback(finalDamage, damageDealer);
        float duration = CalculateDuration(finalDamage);
        entity?.ReciveKnockback(knockback, duration);
    }

    private bool AttackEvaded()
        => Random.Range(0, 100) < stats.GetEvasion();

    public virtual void ReduceHp(float damage)
    {
        entityVfx?.PlayOnDamageVfx();
        currentHp -= damage;
        UpdateHealthBar();

        if (currentHp <= 0)
            Die();
    }

    public virtual void Die()
    {
        isDead = true;
        entity?.EntityDeath();
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null)
            return;

        healthBar.value = currentHp / stats.GetMaxHealth();
    }

    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;

        Vector2 knockback = IsHeavyDamage(damage) ? heavyKnockbackPower : knockbackPower;
        knockback.x = knockback.x * direction;

        return knockback;
    }

    private float CalculateDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;

    private bool IsHeavyDamage(float damage) => damage / stats.GetMaxHealth() > heavyDamageThreshold;
}
