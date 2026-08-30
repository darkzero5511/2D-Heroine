using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;
    private Entity_Stats stats;

    public DamageScaleData damageScale;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] protected LayerMask whatIsTarget;

    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }

    //Attack
    public virtual void PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            IDamgable damegable = target.GetComponent<IDamgable>();

            if (damegable == null)
                continue; // skip target, go to next target

            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();

            AttackData attackData = stats.GetAttackData(damageScale);

            //Attack Data
            float elementalDamage = attackData.elementalDamage;
            float damage = attackData.phyiscalDamage;
            ElementType element = attackData.element;
            bool isCrit = attackData.isCrit;
            //

            bool targetGotHit = damegable.TakeDamage(damage, elementalDamage, element, transform);

            if (element != ElementType.None)
                statusHandler?.ApplyStatusEffect(element, attackData.effectData);

            if (targetGotHit)
            {
                if (element != ElementType.None)
                    vfx.UpdateOnHitElement(target.transform, element);

                vfx.CreateOnHitVFX(target.transform, isCrit, element);
            }
        }
    }

    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
