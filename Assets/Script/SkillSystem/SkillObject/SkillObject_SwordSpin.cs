using UnityEngine;

public class SkillObject_SwordSpin : SkillObject_Sword
{
    private int maxDistance;
    private float attacksPerSecond;
    private float attackTimer;

    public override void SetupSword(Skill_SwordThrow swordManager, Vector2 direction)
    {
        base.SetupSword(swordManager, direction);

        SpinTrigger();

        maxDistance = swordManager.maxDistance;
        attacksPerSecond = swordManager.attacksPerSecond;

        //Invoke(nameof(GetSwordBackToPlayer), swordManager.maxSpinDuration);
        //Invoke(nameof(SpinTrigger), swordManager.maxSpinDuration);
        Invoke(nameof(DestroySword), swordManager.maxSpinDuration);
    }

    private void SpinTrigger()
    {
        anim?.SetTrigger("spin");
    }

    protected override void Update()
    {
        HandleAttack();
        HandleStopping();
        //HandleComeback();
    }

    private void HandleStopping()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > maxDistance && rb.simulated == true)
            rb.simulated = false;
    }

    private void HandleAttack()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer < 0)
        {
            DamageEnemiesInRadius(transform, 1);
            attackTimer = 1 / attacksPerSecond;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        rb.simulated = false;
    }
}
