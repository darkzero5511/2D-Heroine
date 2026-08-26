using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public static event Action OnPlayerDeath;

    private UI ui;

    public PlayerInputSet input { get; private set; }
    public Player_SkillManager skillManager { get; private set; }
    //
    //Player State
    //

    #region Player State
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
    public Player_DeathState deathState { get; private set; }
    public Player_CounterAttackState counterAttackState { get; private set; }

    #endregion Player State

    //Attack

    #region Attack

    [Header("Attack Details")]
    public Vector2[] attackVelocity;

    public float attackVelocityDuration = .1f;
    public float comboResetTime = 1;
    private Coroutine queuedAttackCo;

    #endregion Attack

    //Hurt
    [Space]
    public float recoverTime = 1;

    public bool isHurt = false;

    //Movement
    [Header("Movement Detail")]
    public float moveSpeed;

    // Jump

    #region Jump
    public float jumpForce = 5;

    [Range(0, 1)] public float doubleJumpMultiplier = 0.6f;

    [Space]
    public Vector2 wallJumpForce;

    public Vector2 wallBoundForce;
    public Vector2 jumpAttackVelocity;
    public int doubleJump = 1;

    [Range(0, 1)] public float inAirMoveMultiplier = .7f;
    [Range(0, 1)] public float wallSlideSlowMultiplier = 0.9f;

    #endregion Jump

    // Dash

    #region dash

    [Space]
    public float dashDuration = .25f;

    public float dashBackDuration = .9f;

    public float dashSpeed = 20;
    public float dashBackSpeed = -20;

    #endregion dash

    public Vector2 moveInput { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        ui = FindAnyObjectByType<UI>();
        skillManager = GetComponent<Player_SkillManager>();

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
        deathState = new Player_DeathState(this, stateMachine, "death");
        counterAttackState = new Player_CounterAttackState(this, stateMachine, "counterAttack");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        float originalMoveSpeed = moveSpeed;
        float originalJumpForce = jumpForce;
        float originalAnimSpeed = anim.speed;
        float originalDashSpeed = dashSpeed;
        float originalDashBackSpeed = dashBackSpeed;
        Vector2 originalWallJump = wallJumpForce;
        Vector2 originalJumpAttack = jumpAttackVelocity;
        Vector2[] originalAttackVelocity = new Vector2[attackVelocity.Length];
        Array.Copy(attackVelocity, originalAttackVelocity, attackVelocity.Length);

        float speedMultiplier = 1 - slowMultiplier;

        moveSpeed *= speedMultiplier;
        jumpForce *= speedMultiplier;
        anim.speed *= speedMultiplier;
        wallJumpForce *= speedMultiplier;
        jumpAttackVelocity *= speedMultiplier;
        dashSpeed *= speedMultiplier;
        dashBackSpeed *= speedMultiplier;

        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] *= speedMultiplier;
        }

        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        jumpForce = originalJumpForce;
        anim.speed = originalAnimSpeed;
        wallJumpForce = originalWallJump;
        jumpAttackVelocity = originalJumpAttack;
        dashSpeed = originalDashSpeed;
        dashBackSpeed = originalDashBackSpeed;

        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = originalAttackVelocity[i];
        }
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

    //Death
    public override void EntityDeath()
    {
        base.EntityDeath();

        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(deathState);
    }

    public void Recover()
    {
        isHurt = false;
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;

        input.Player.ToggleSkillTreeUI.performed += ctx => ui.ToggleSkillTreeUI();
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
