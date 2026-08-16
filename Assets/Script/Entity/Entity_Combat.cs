using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;
    public float damage = 10;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;

    [SerializeField] private float targetCheckRadius = 1;

    [SerializeField] protected LayerMask whatIsTarget;

    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
    }

    //Attack
    public void PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            IDamgable damgable = target.GetComponent<IDamgable>();

            if (damgable == null)
                continue; // skip target, go to next target

            bool targetGotHit = damgable.TakeDamage(damage, transform);
            if (targetGotHit)
                vfx.CreateOnHitVFX(target.transform);
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
