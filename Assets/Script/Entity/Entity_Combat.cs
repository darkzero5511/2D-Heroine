using System;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public event Action<float> OnDoingPhysicalDamage;

    private Entity_VFX vfx;
    private Entity_Stats stats;

    public DamageScaleData damageScale;

    [Header("Target Detection")]
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected Transform farTargetCheck;

    [SerializeField] protected float targetCheckRadius = 1;
    [SerializeField] protected float farTargetCheckRadius = 1f;

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
            AttackTarget(target);
        }
    }

    public virtual void PerformFarAttack()
    {
        foreach (var target in GetFarDetectedColliders())
        {
            AttackTarget(target);
        }
    }

    private void AttackTarget(Collider2D target)
    {
        IDamageable damegable = target.GetComponent<IDamageable>();

        if (damegable == null)
            return; // skip target, go to next target

        Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();

        AttackData attackData = stats.GetAttackData(damageScale);

        //Attack Data
        float elementalDamage = attackData.elementalDamage;
        float physicalDamage = attackData.phyiscalDamage;
        ElementType element = attackData.element;
        bool isCrit = attackData.isCrit;
        //

        bool targetGotHit = damegable.TakeDamage(physicalDamage, elementalDamage, element, transform);

        if (element != ElementType.None)
            statusHandler?.ApplyStatusEffect(element, attackData.effectData);

        if (targetGotHit)
        {
            OnDoingPhysicalDamage?.Invoke(physicalDamage);

            if (element != ElementType.None)
                vfx.UpdateOnHitElement(target.transform, element);

            vfx.CreateOnHitVFX(target.transform, isCrit, element);
        }
    }

    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(
            targetCheck.position,
            targetCheckRadius,
            whatIsTarget
        );
    }

    protected Collider2D[] GetFarDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(
            farTargetCheck.position,
            farTargetCheckRadius,
            whatIsTarget
        );
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
        if (farTargetCheck != null)
            Gizmos.DrawWireSphere(farTargetCheck.position, farTargetCheckRadius);
    }
}
