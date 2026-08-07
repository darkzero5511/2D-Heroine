using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public float damage = 10;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;

    [SerializeField] private Transform targetCheck3;

    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private Vector2 targetCheckBox = new Vector2(1f, 0.5f);

    [SerializeField] private LayerMask whatIsTarget;

    //Attack 1 & 2
    public void PerformAttack()
    {
        foreach (var target in GetDetectedColider())
        {
            IDamagble damagble = target.GetComponent<IDamagble>();
            damagble?.TakeDamage(damage, transform);
        }
    }

    public void PerformAttack3()
    {
        foreach (var target in GetDetectedColider3())
        {
            IDamagble damagble = target.GetComponent<IDamagble>();
            damagble?.TakeDamage(damage, transform);
        }
    }

    private Collider2D[] GetDetectedColider()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private Collider2D[] GetDetectedColider3()
    {
        return Physics2D.OverlapBoxAll(targetCheck3.position, targetCheckBox, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);

        if (targetCheck3 != null)
            Gizmos.DrawCube(targetCheck3.position, targetCheckBox);
        //Gizmos.DrawWireSphere(targetCheck3.position, targetCheckRadius);
    }
}
