using UnityEngine;

public class Enity : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb { get; private set; }

    protected StateMachine stateMachine;

    private bool facingRight = true;
    public int facingDir { get; private set; } = 1;

    [Header("Collision Detection")]
    [SerializeField] private float groundCheckDistance;

    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float grabCheckDistance;

    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;
    [SerializeField] private Transform primaryGrabCheck;
    [SerializeField] private Transform secondaryGrabCheck;

    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }
    public bool GrabDetected { get; private set; }

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {
        //Empty
    }

    private void Update()
    {
        HandleCollisionDetected();
        stateMachine.UpdateActiveState();
    }

    public void CallAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    private void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && facingRight == false)
            Flip();
        else if (xVelocity < 0 && facingRight == true)
            Flip();
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir = facingDir * -1;
    }

    private void HandleCollisionDetected()
    {
        // Ground Detected
        groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);

        // Wall Detected
        wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround)
                    && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

        // Grab Detected
        GrabDetected = Physics2D.Raycast(primaryGrabCheck.position, Vector2.right * facingDir, grabCheckDistance, whatIsGround)
                        && !Physics2D.Raycast(secondaryGrabCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        // Ground
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));

        // Wall
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));
        Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));

        // Grab
        Gizmos.DrawLine(primaryGrabCheck.position, primaryGrabCheck.position + new Vector3(grabCheckDistance * facingDir, 0));
        Gizmos.DrawLine(secondaryGrabCheck.position, secondaryGrabCheck.position + new Vector3(grabCheckDistance * facingDir, 0));
    }
}
