using UnityEngine;

public class EnemyState : EnityState
{
    protected Enemy enemy;

    public EnemyState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.enemy = enemy;

        rb = enemy.rb;
        anim = enemy.anim;
    }

    public override void Update()
    {
        base.Update();

        anim.SetFloat("moveAnimSpeedMultiplier", enemy.moveAnimSpeedMultiplier);
    }
}
