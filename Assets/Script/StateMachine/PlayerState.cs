using UnityEngine;

public abstract class PlayerState : EnityState
{
    protected Player player;
    protected PlayerInputSet input;

    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;

        anim = player.anim;
        rb = player.rb;
        input = player.input;
    }

    public override void Update()
    {
        base.Update();

        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            if (player.moveInput.x == 0 && player.groundDetected)
                stateMachine.ChangeState(player.dashBackwardState);
            else

                stateMachine.ChangeState(player.dashState);
        }

        if (player.groundDetected)
            player.doubleJump = 1;
        else if (!player.groundDetected && player.wallDetected)
            player.doubleJump = 0;
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    public bool CanDash()
    {
        if (player.wallDetected)
            return false;

        if (stateMachine.currentState == player.dashState)
            return false;

        if (Time.time < player.dashState.lastTimeDashed + player.dashCooldown)
            return false;

        return true;
    }
}
