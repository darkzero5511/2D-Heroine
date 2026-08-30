using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    private Entity_VFX vfx;
    [SerializeField] private GameObject onHitVfx;

    [Space]
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 1;

    protected Entity_Stats playerStats;
    protected DamageScaleData damageScaleData;
    protected bool targetGotHit;

    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
    }

    protected void DamageEnemiesInRadius(Transform t, float radius)
    {
        foreach (var target in EnemiesAround(t, radius))
        {
            IDamgable damgable = target.GetComponent<IDamgable>();

            if (damgable == null)
                continue;

            AttackData attackData = playerStats.GetAttackData(damageScaleData);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();

            float physcalDamage = playerStats.GetPhysicalDamage(out bool isCrit, damageScaleData.physical);
            float elementalDamage = playerStats.GetElementalDamage(out ElementType element, damageScaleData.elemental);

            targetGotHit = damgable.TakeDamage(physcalDamage, elementalDamage, element, transform);

            if (element != ElementType.None)
            {
                statusHandler?.ApplyStatusEffect(element, attackData.effectData);
            }

            if (targetGotHit)
            {
                Instantiate(onHitVfx, target.transform.position, Quaternion.identity);
            }
        }
    }

    protected Transform FindClosestTarget()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in EnemiesAround(transform, 10))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                target = enemy.transform;
                closestDistance = distance;
            }
        }

        return target;
    }

    protected Collider2D[] EnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }

    protected virtual void OnDrawGizmos()
    {
        if (targetCheck == null)
            targetCheck = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(targetCheck.position, checkRadius);
    }
}
