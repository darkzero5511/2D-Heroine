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
        
    
    
        dashDir = - player.facingDir;
        stateTimer = player.dashBackDuration;

        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;

    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(- player.dashBackSpeed * dashDir, 0);

        if (stateTimer < 0)
        {
            if (player.groundDetected)
            { stateMachine.ChangeState(player.idleState);
            player.Flip();}
            else{
                stateMachine.ChangeState(player.fallState);
            player.Flip();
        }
    }
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, 0);
        rb.gravityScale = originalGravityScale;

        lastTimeDashed = Time.time;
    }

    
}
