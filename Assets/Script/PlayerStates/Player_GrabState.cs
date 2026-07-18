using UnityEngine;

public class Player_GrabState : EntityState
{
    private Vector2 grabPosition;
    private float originalGravityScale;

    public Player_GrabState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        grabPosition = rb.position;

        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();

        rb.MovePosition(grabPosition);

        if (input.Player.Jump.WasPressedThisFrame())
            stateMachine.ChangeState(player.jumpState);
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, 0);
        rb.gravityScale = originalGravityScale;
    }
}
