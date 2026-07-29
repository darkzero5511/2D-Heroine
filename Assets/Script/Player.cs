using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public PlayerInputSet input { get; private set; }

    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_DashBackwardState dashBackwardState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_GrabState grabState { get; private set; }
    public Player_DoubleJumpState doubleJumpState { get; private set; }

    ///Attack
    [Header("Attack Details")]
    public Vector2[] attackVelocity;

    public float attackVelocityDuration = .1f;
    public float comboResetTime = 1;
    private Coroutine queuedAttackCo;
    ///Attack

    ///Movement
    [Header("Movement Detail")]
    public float moveSpeed;

    public float jumpForce = 5;
    [Range(0, 1)] public float doubleJumpMultiplier = 0.6f;
    ///Movement

    /// Jump
    [Space]
    public Vector2 wallJumpForce;

    public Vector2 wallBoundForce;
    public Vector2 jumpAttackForce;
    public int doubleJump = 1;

    [Range(0, 1)] public float inAirMoveMultiplier = .7f;
    [Range(0, 1)] public float wallSlideSlowMultiplier = 0.9f;
    /// Jump

    /// Dash
    [Space]
    public float dashDuration = .25f;

    public float dashBackDuration = .9f;

    public float dashSpeed = 20;
    public float dashBackSpeed = -20;

    public float dashCooldown = 2;
    /// Dash

    public Vector2 moveInput { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        input = new PlayerInputSet();

        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "move");
        jumpState = new Player_JumpState(this, stateMachine, "jumpFall");
        fallState = new Player_FallState(this, stateMachine, "jumpFall");
        wallSlideState = new Player_WallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
        dashState = new Player_DashState(this, stateMachine, "dash");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
        grabState = new Player_GrabState(this, stateMachine, "grab");
        dashBackwardState = new Player_DashBackwardState(this, stateMachine, "dashBackward");
        doubleJumpState = new Player_DoubleJumpState(this, stateMachine, "doubleJump");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    //Attack
    public void EnterAttackStateWithDelay()
    {
        if (queuedAttackCo != null)
            StopCoroutine(queuedAttackCo);

        queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
    }

    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public bool IsMovingAgainstFacingDir()
    {
        return moveInput.x != 0 && Mathf.Sign(moveInput.x) != facingDir;
    }
}
