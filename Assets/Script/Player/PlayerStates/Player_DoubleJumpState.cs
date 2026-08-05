public class Player_DoubleJumpState : Player_AiredState
{
    public Player_DoubleJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(rb.linearVelocity.x, player.jumpForce * player.doubleJumpMultiplier);
        player.doubleJump = player.doubleJump - 1;
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0 && stateMachine.currentState != player.jumpAttackState)
            stateMachine.ChangeState(player.fallState);
    }
}
