using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public float damage = 10;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;

    [SerializeField] private Transform targetCheck3;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;

    //Attack 1 & 2
    public void PerformAttack()
    {
        GetDetectedColider();

        foreach (var target in GetDetectedColider())
        {
            Entity_Health targetHealth = target.GetComponent<Entity_Health>();

            targetHealth?.TakeDamage(damage);
        }
    }

    public void PerformAttack3()
    {
        GetDetectedColider3();
        foreach (var target in GetDetectedColider3())
        {
            Debug.Log("Attacking 3 " + target.name);
        }
    }

    private Collider2D[] GetDetectedColider()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private Collider2D[] GetDetectedColider3()
    {
        return Physics2D.OverlapCircleAll(targetCheck3.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
        if (targetCheck3 != null)
            Gizmos.DrawWireSphere(targetCheck3.position, targetCheckRadius);
    }
}
