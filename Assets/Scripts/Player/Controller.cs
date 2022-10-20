#undef WALL_SLIDE
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    /// <summary> Player References </summary>
    private PlayerInput playerInput;
    private PlayerStats playerStats;
    private Rigidbody2D rb;
    private Animator anim;

    /// <summary> Player Hidden Variables </summary>
    private float jumpInputTimer;
    private float ungroundedTimer;
    private float holdTimer;
    private bool doubleJumped;
    private bool facingRight = true;

    [Header("Ground Checking")]
    /// <summary> Player Physics </summary>
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [Space(5)]

    [Header("Speed Variables")]
    /// <summary> Player Movement Stats </summary>
    [SerializeField] private float movementSpeed;
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float jumpHeight;
    [Range(0, 1)]
    [SerializeField] private float dampingNormal = 0.5f;
    [Range(0, 1)]
    [SerializeField] private float dampingStop = 0.5f;
    [Range(0, 1)]
    [SerializeField] private float dampingTurn = 0.5f;
    [SerializeField] private float drag;
    [Space(5)]

    [Header("Physics Materials")]
    ///<summary> Physic Materials </summary>
    [SerializeField] private PhysicsMaterial2D slipperyMat;
    [SerializeField] private PhysicsMaterial2D stickyMat;
    [Space(5)]

    [Header("Ability Bools")]
    /// <summary> Abilitiy Booleans </summary>
    [SerializeField] private bool wallSlideOn;
    [SerializeField] private bool groundPoundOn;
    [SerializeField] private bool shootingOn;
    [SerializeField] private bool doubleJumpOn;
    [SerializeField] private bool healingOn;
    [Space(5)]

    [Header("Wall Jumping Values")]
    /// <summary> Wall Jump Values </summary>
    [SerializeField] private float xWallForce;
    [SerializeField] private float yWallForce;
    [SerializeField] private float wallJumpTime;
    [SerializeField] private Transform frontCheck;
    [SerializeField] private LayerMask wallLayer;
    private bool isTouchingFront;
    private bool wallSliding;

    #region Main Functions
    // Start is called before the first frame update
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerStats = GetComponent<PlayerStats>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        /*
        BELOW IS SOME EXAMPLE USAGE OF HOW THE BETTERDEBUGGING CLASS WILL WORK 
        HOPEFULLY IT SHOULD ALLEVIATE SOME OF THE ISSUES WE HAVE DOWN THE LINE WITH OUR CODE AS THIS GAME
        EXPANDS ITS FEATURES. 


        bool failed = playerInput == null || playerStats == null || anim == null || rb == null;
        if (failed)
        {
            BetterDebugging.Instance.DebugLog("PLAYER FAILED TO GRAB COMPONENTS... CHECK THE GAMEOBJECT", BetterDebugging.eDebugLevel.Error);
        }

        BetterDebugging.Instance.SpawnDebugText("STARTING PLAYER", transform.position, 3f, BetterDebugging.eDebugLevel.Error);

        BetterDebugging.Instance.Assert(rb != null, "RigidBody is null");
        */
    }

    private void Update()
    {
        ///<Note> An exit function will need to made when switching on and off the booleans so the variables return back to default
        /// values that may cause weird behaviours in future </Note>
        Jumping();
        Attack();

#if WALL_SLIDE
        if (wallSlideOn)
        {
            WallSlide();
        }
#endif
        if (groundPoundOn)
        {

        }

        if (shootingOn)
        {

        }

        if (doubleJumpOn)
        {
            DoubleJump();
        }

        if (healingOn)
        {

        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Movement();
    }

    #endregion

    #region Basic Movement Functions
    private void Movement()
    {
        rb.velocity = new Vector2(HorizontalDrag(), rb.velocity.y);

        Vector2 clamper = rb.velocity;
        clamper.x = Mathf.Clamp(clamper.x, -movementSpeed, movementSpeed);
        clamper.y = Mathf.Clamp(clamper.y, -fallingSpeed, jumpHeight);

        rb.velocity = clamper;

        if ((playerInput.actions["Horizontal"].ReadValue<float>() > 0 && !facingRight) || (playerInput.actions["Horizontal"].ReadValue<float>() < 0 && facingRight))
        {
            Flip();
        }
    }

    private float HorizontalDrag()
    {
        float horizontalVelocity = rb.velocity.x;
        horizontalVelocity += playerInput.actions["Horizontal"].ReadValue<float>();

        if (Mathf.Abs(playerInput.actions["Horizontal"].ReadValue<float>()) < 0.01f)
        {
            horizontalVelocity *= Mathf.Pow(movementSpeed - dampingStop, Time.fixedDeltaTime * -drag);
        }
            
        else if (Mathf.Sign(playerInput.actions["Horizontal"].ReadValue<float>()) != Mathf.Sign(horizontalVelocity))
        {
            horizontalVelocity *= Mathf.Pow(movementSpeed - dampingTurn, Time.fixedDeltaTime * -drag);
        }

        else
        {
            horizontalVelocity *= Mathf.Pow(movementSpeed - dampingNormal, Time.fixedDeltaTime * -drag);
        }

        return horizontalVelocity;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    #endregion

    #region Jumping Functions

    private void Jumping()
    {
        if (playerInput.actions["Jump"].triggered)
        {
            jumpInputTimer = 0.2f;
        }

        if (jumpInputTimer > 0)
        {
            if (JumpAvaliable() && !wallSliding)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);

                ungroundedTimer = 0;
            }

            else if (wallSliding)
            {
                rb.velocity = new Vector2(xWallForce * -playerInput.actions["Horizontal"].ReadValue<float>(), yWallForce);
            }

            else
            {
                jumpInputTimer -= Time.fixedDeltaTime;
            }  
        }

        if (!IsGrounded() && ungroundedTimer > 0)
        {
            ungroundedTimer -= Time.fixedDeltaTime;
        }

        if (!IsGrounded())
        {
            holdTimer += Time.fixedDeltaTime;
        }

        if (playerInput.actions["Jump"].ReadValue<float>() == 0 || holdTimer > 0.5f)
        {
            if (!wallSliding)
            {
                rb.AddForce(new Vector2(0, -0.5f), ForceMode2D.Impulse);
            }

            else
            {
                rb.AddForce(new Vector2(0, -0.25f), ForceMode2D.Impulse);
            }
        }
    }

    private bool JumpAvaliable()
    {
        return IsGrounded() || (!IsGrounded() && ungroundedTimer > 0);
    }

    private bool IsGrounded()
    {
        if (!Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer))
        {
            rb.sharedMaterial = slipperyMat;
            return false;
        }

        rb.sharedMaterial = stickyMat;
        ungroundedTimer = 0.2f;
        holdTimer = 0f;
        return true;
    }

    #endregion

    #region Base Combat Functions

    private void Attack()
    {
        if (playerInput.actions["Attack"].triggered)
        {
            anim.SetTrigger("Attack");
        }
    }

    #endregion

    #region Extra Movement Functions

    private void WallSlide()
    {
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, 0.1f, wallLayer);

        if (isTouchingFront == true && IsGrounded() == false && playerInput.actions["Horizontal"].ReadValue<float>() != 0)
            wallSliding = true;
        else
            wallSliding = false;
    }

    private void DoubleJump()
    {
        if (!JumpAvaliable() && playerInput.actions["Jump"].triggered && !doubleJumped)
        {
            Debug.Log("Double Jump Triggered");

            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);

            ungroundedTimer = 0;

            doubleJumped = true;
        }
    }

    public void AddImpulse(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    #endregion
}
