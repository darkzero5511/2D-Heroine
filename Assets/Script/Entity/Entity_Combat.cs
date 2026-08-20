using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;
    private Entity_Stats stats;

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
    public void PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            IDamgable damegable = target.GetComponent<IDamgable>();

            if (damegable == null)
                continue; // skip target, go to next target

            float elementalDamage = stats.GetElementalDamage(out ElementType element, .6f);
            float damage = stats.GetPhysicalDamage(out bool isCrit);

            bool targetGotHit = damegable.TakeDamage(damage, elementalDamage, element, transform);

            if (element != ElementType.None)
                ApplyStatusEffect(target.transform, element);

            if (targetGotHit)
            {
                vfx.UpdateOnHitElement(target.transform, element);
                vfx.CreateOnHitVFX(target.transform, isCrit);
            }
        }
    }

    public void ApplyStatusEffect(Transform target, ElementType element, float scaleFactor = 1f)
    {
        Entity_StatusHandle statusHandle = target.GetComponent<Entity_StatusHandle>();

        if (statusHandle == null)
            return;

        //Ice
        if (element == ElementType.Ice && statusHandle.CanBeApplied(ElementType.Ice))
            statusHandle.ApplyChilledEffect(stats.statusEffect.defaultDuration, stats.statusEffect.chillSlowMultiplier);

        //Fire
        if (element == ElementType.Fire && statusHandle.CanBeApplied(ElementType.Fire))
        {
            float fireDamage = stats.offense.fireDamage.GetValue();
            statusHandle.ApplyBurnEffect(stats.statusEffect.defaultDuration, stats.statusEffect.burnMultiplier * fireDamage);
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
