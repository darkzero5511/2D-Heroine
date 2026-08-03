using UnityEngine;

public class Entity : MonoBehaviour
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

    [SerializeField] protected LayerMask whatIsGround;

    //Ground
    [Space]
    [SerializeField] private Transform groundCheck;

    //Wall
    [Space]
    [SerializeField] private Transform primaryWallCheck;

    [SerializeField] private Transform secondaryWallCheck;

    //Grab
    [Space]
    [SerializeField] private Transform primaryGrabCheck;

    [SerializeField] private Transform secondaryGrabCheck;

    public bool groundDetected { get; private set; }
    public bool groundAboveDetected { get; private set; }
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

    protected virtual void Update()
    {
        HandleCollisionDetected();
        stateMachine.UpdateActiveState();
    }

    public void CurrentStateAnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    public void HandleFlip(float xVelocity)
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
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

        // Wall Detected
        if (secondaryWallCheck != null)
        {
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround)
                    && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
        }
        else
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

        // Grab Detected
        if (primaryGrabCheck != null && secondaryGrabCheck != null)
            GrabDetected = Physics2D.Raycast(primaryGrabCheck.position, Vector2.right * facingDir, grabCheckDistance, whatIsGround)
                            && !Physics2D.Raycast(secondaryGrabCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    protected virtual void OnDrawGizmos()
    {
        // Ground
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));

        // Wall
        Gizmos.color = Color.green;
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));

        if (secondaryWallCheck != null)
            Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));

        // Grab
        Gizmos.color = Color.red;
        if (primaryGrabCheck != null && secondaryGrabCheck != null)
        {
            Gizmos.DrawLine(primaryGrabCheck.position, primaryGrabCheck.position + new Vector3(grabCheckDistance * facingDir, 0));
            Gizmos.DrawLine(secondaryGrabCheck.position, secondaryGrabCheck.position + new Vector3(grabCheckDistance * facingDir, 0));
        }
    }
}
