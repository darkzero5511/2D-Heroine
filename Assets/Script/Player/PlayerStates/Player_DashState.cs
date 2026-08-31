using UnityEngine;

public class Player_DashState : PlayerState
{
    public Player_DashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    private float originalGravityScale;
    private float dashDir;

    public override void Enter()
    {
        base.Enter();

        skillManager.dash.OnStartEffect();
        player.vFX.DoImageEchoEffect(player.dashDuration);

        dashDir = player.facingDir;
        stateTimer = player.dashDuration;

        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;

        player.health.SetCanTakeDamage(false);
    }

    public override void Update()
    {
        base.Update();

        CancelDashIfNeeded();

        player.SetVelocity(player.dashSpeed * dashDir, 0);

        if (stateTimer < 0)
        {
            if (player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        skillManager.dash.OnEndEffect();

        player.SetVelocity(0, 0);
        rb.gravityScale = originalGravityScale;
        player.health.SetCanTakeDamage(true);
    }

    private void CancelDashIfNeeded()
    {
        if (player.wallDetected)
        {
            if (player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.wallSlideState);
        }
    }
}
