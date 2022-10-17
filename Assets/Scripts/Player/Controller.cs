#undef WALL_SLIDE
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    /// <summary> Player References </summary>
    private PlayerInput playerInput;
    private PlayerStats playerStats;
    private MaskClass maskClass;
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

    [Header("Mask Values")]
    [SerializeField] private WarMask warMask;
    [SerializeField] private NatureMask natureMask;
    public enum eMasks
    {
        war,
        nature,
        sea,
        energy
    }

    [SerializeField] private eMasks masks;

    #region Main Functions
    // Start is called before the first frame update
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerStats = GetComponent<PlayerStats>();
        maskClass = GetComponent<MaskClass>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        //set default mask
        RemoveMasks();
        warMask.enabled = true;
    }

    private void Update()
    {
        ///<Note> An exit function will need to made when switching on and off the booleans so the variables return back to default
        /// values that may cause weird behaviours in future </Note>
        Jumping();
        Attack();
        SpecialAttack();
        MaskInputs();

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
    #region Mask Input Function
    private void MaskInputs()
    {
        if (playerInput.actions["OptionOne"].triggered) //will also include an if statement checking if the selected mask has been unlocked
        {
            masks = eMasks.war;
        }

        if (playerInput.actions["OptionTwo"].triggered) //will also include an if statement checking if the selected mask has been unlocked
        {
            masks = eMasks.nature;
        }

        if (playerInput.actions["OptionThree"].triggered) //will also include an if statement checking if the selected mask has been unlocked
        {
            masks = eMasks.energy;;
        }

        if (playerInput.actions["OptionFour"].triggered) //will also include an if statement checking if the selected mask has been unlocked
        {
            masks = eMasks.sea;
        }

        MaskChange();
    }

    private void MaskChange()
    {
        //TODO : Add other masks

        switch (masks)
        {
            case eMasks.war:
                if (!warMask.enabled)
                {
                    RemoveMasks();
                    warMask.enabled = true;
                }
                break;
            case eMasks.nature:
                if (!natureMask.enabled)
                {
                    RemoveMasks();
                    natureMask.enabled = true;
                }
                break;
        }
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

    private void SpecialAttack()
    {
        if (playerInput.actions["Special"].triggered)
        {
            switch (masks)
            {
                case eMasks.war:
                    warMask.SpecialAttack();
                    break;
                case eMasks.nature:
                    natureMask.SpecialAttack();
                    break;
            }
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

    #endregion

    #region Mask Functions
    private void RemoveMasks()
    {
        warMask.enabled = false;
        natureMask.enabled = false;
    }
    #endregion
}
