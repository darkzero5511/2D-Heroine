using System.Collections;
using UnityEngine;

public class Enemy_DeathState : EnemyState
{
    private Collider2D col;
    private Enemy_VFX vfx;

    public Enemy_DeathState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        col = enemy.GetComponent<Collider2D>();
        vfx = enemy.GetComponent<Enemy_VFX>();
    }

    public override void Enter()
    {
        base.Enter();

        //Incase, death when counterable
        vfx.EnableAttackAlert(false);
        enemy.EnableCounterWindow(false);

        //anim.enabled = false;
        //col.enabled = false;

        //rb.gravityScale = 12;
        //rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);
    }
}
