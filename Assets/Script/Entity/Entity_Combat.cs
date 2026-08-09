using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;
    public float damage = 10;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;

    [SerializeField] private float targetCheckRadius = 1;

    [SerializeField] protected LayerMask whatIsTarget;

    //[Header("Player")]
    //[SerializeField] private float targetCheckRadius3 = 1;

    //[SerializeField] private Transform targetCheck3;

    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
    }

    //Attack 1 & 2
    public void PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            IDamgable damgable = target.GetComponent<IDamgable>();

            if (damgable == null)
                continue; // skip target, go to next target

            damgable.TakeDamage(damage, transform);
            vfx.CreateOnHitVFX(target.transform);
        }
    }

    //public void PerformAttack3()
    //{
    //    foreach (var target in GetDetectedColider3())
    //    {
    //        IDamgable damagble = target.GetComponent<IDamgable>();

    //        if (damagble != null)
    //            continue;

    //        damagble.TakeDamage(damage, transform);
    //        vfx.CreateOnHitVFX(target.transform);
    //    }
    //}

    //private Collider2D[] GetDetectedColider3()
    //{
    //    return Physics2D.OverlapCircleAll(targetCheck3.position, targetCheckRadius3, whatIsTarget);
    //}

    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
        //if (targetCheck3 != null)
        //    Gizmos.DrawWireSphere(targetCheck3.position, targetCheckRadius3);
    }
}
