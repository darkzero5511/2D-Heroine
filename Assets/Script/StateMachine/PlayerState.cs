using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInputSet input;
    protected Player_SkillManager skills;

    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;

        anim = player.anim;
        rb = player.rb;
        input = player.input;

        stats = player.stats;
        skills = player.skillManager;
    }

    public override void Update()
    {
        base.Update();

        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            skills.dash.SetSkillOnCooldonw();

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
        if (skills.dash.CanUseSkill() == false)
            return false;

        if (player.wallDetected)
            return false;

        if (stateMachine.currentState == player.dashState)
            return false;

        return true;
    }
}
