using UnityEngine;

public class Player_DashBackwardState : PlayerState
{
    public Player_DashBackwardState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    private float originalGravityScale;
    private float dashDir;
    public float lastTimeDashed;

    public override void Enter()
    {
        base.Enter();

        skillManager.dash.OnStartEffect();

        player.vFx.DoImageEchoEffect(player.dashBackDuration);

        dashDir = -player.facingDir;
        stateTimer = player.dashBackDuration;

        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;

        player.health.SetCanTakeDamage(false);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(-player.dashBackSpeed * dashDir, 0);

        if (stateTimer < 0)
        {
            if (player.groundDetected)
            {
                player.Flip();
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                player.Flip();
                stateMachine.ChangeState(player.fallState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        skillManager.dash.OnEndEffect();

        player.SetVelocity(0, 0);
        rb.gravityScale = originalGravityScale;

        lastTimeDashed = Time.time;

        player.health.SetCanTakeDamage(true);
    }
}
