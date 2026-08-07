using UnityEngine;

public class Chest : MonoBehaviour, IDamagble
{
    private Rigidbody2D rb => GetComponentInChildren<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();
    private Entity_VFX fx => GetComponent<Entity_VFX>();

    [Header("Open Detail")]
    [SerializeField] private Vector2 knockback;

    public void TakeDamage(float damage, Transform damageDealer)
    {
        fx.PlayOnDamageVfx();
        if (ValueChest())
            anim.SetBool("open_gold", true);
        else
            anim.SetBool("open_empty", true);

        rb.linearVelocity = knockback;

        rb.angularVelocity = Random.Range(-200f, 200f);
    }

    private bool ValueChest()
    {
        if (Random.value <= .75f)
            return true;
        return false;
    }
}
