public class Player_JumpState : Player_AiredState
{
    public Player_JumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    
    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(rb.linearVelocity.x, player.jumpForce);

    }

    public override void Update()
    {
        base.Update();

        //Double Jump
        if (input.Player.Jump.WasPerformedThisFrame() && player.doubleJump > 0)
            stateMachine.ChangeState(player.doubleJumpState);


        if (rb.linearVelocity.y < 0 && stateMachine.currentState != player.jumpAttackState)
            stateMachine.ChangeState(player.fallState);
    }
}
