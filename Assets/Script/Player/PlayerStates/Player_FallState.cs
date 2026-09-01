public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        //Double Jump
        if (input.Player.Jump.WasPerformedThisFrame() && player.doubleJump > 0)
            stateMachine.ChangeState(player.doubleJumpState);

        //Grab
        if (player.GrabDetected)
            stateMachine.ChangeState(player.grabState);

        //Grounded
        if (player.groundDetected)
            stateMachine.ChangeState(player.idleState);

        //Touch Wall
        if (player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);
    }
}
