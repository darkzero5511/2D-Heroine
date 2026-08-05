using System.Collections;
using UnityEngine;

public class Enemy_DeathState : EnemyState
{
    private Collider2D col;

    public Enemy_DeathState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        col = enemy.GetComponent<Collider2D>();
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("Death");
        //anim.enabled = false;
        //col.enabled = false;

        //rb.gravityScale = 12;
        //rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);
    }
}
