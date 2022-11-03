#undef WALL_SLIDE
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    /// <summary> Player References </summary>
    private PlayerInput m_playerInput;
    private PlayerStats m_playerStats;
    private Rigidbody2D m_rigidbody;

    [SerializeField] private GameObject m_bodyGameObject;
    [SerializeField] private GameObject m_maskGameObject;
    private Animator m_bodyAnimator;
    private Animator m_maskAnimator;


    /// <summary> Player Hidden Variables </summary>
    private float m_jumpInputTimer;
    private float m_ungroundedTimer;
    private float m_holdTimer;
    private bool m_doubleJumped;
    private bool m_facingRight = true;

    [Header("Ground Checking")]
    /// <summary> Player Physics </summary>
    [SerializeField] private Transform m_groundCheck;
    [SerializeField] private LayerMask m_groundLayer;
    [Space(5)]

    [Header("Speed Variables")]
    /// <summary> Player Movement Stats </summary>
    public float MovementSpeed;
    [SerializeField] private float m_fallingSpeed;
    [SerializeField] private float m_jumpHeight;
    [Range(0, 1)]
    [SerializeField] private float m_dampingNormal = 0.5f;
    [Range(0, 1)]
    [SerializeField] private float m_dampingStop = 0.5f;
    [Range(0, 1)]
    [SerializeField] private float m_dampingTurn = 0.5f;
    [SerializeField] private float m_drag;
    [Space(5)]

    [Header("Physics Materials")]
    ///<summary> Physic Materials </summary>
    [SerializeField] private PhysicsMaterial2D slipperyMat;
    [SerializeField] private PhysicsMaterial2D stickyMat;
    [SerializeField] private BoxCollider2D boxCollider;
    [Space(5)]

    [Header("Ability Bools")]
    /// <summary> Abilitiy Booleans </summary>
    public bool wallSlideOn;
    public bool groundPoundOn;
    public bool shootingOn;
    public bool doubleJumpOn;
    public bool healingOn;
    [Space(5)]

#if WALL_SLIDE
    [Header("Wall Jumping Values")]
    /// <summary> Wall Jump Values </summary>
    [SerializeField] private float m_xWallForce;
    [SerializeField] private float m_yWallForce;
    [SerializeField] private float m_wallJumpTime;
    [SerializeField] private Transform m_frontCheck;
    [SerializeField] private LayerMask m_wallLayer;
    private bool m_isTouchingFront;
    private bool m_wallSliding;
#endif

    [Header("Mask Values")]
    [SerializeField] private WarMask m_warMask;
    [SerializeField] private NatureMask m_natureMask;
    public enum eMasks
    {
        War,
        Nature,
        Sea,
        Energy
    }

    [SerializeField] private eMasks m_masks;

#region Main Functions
    // Start is called before the first frame update
    private void Start()
    {
        m_playerInput = GetComponent<PlayerInput>();
        m_playerStats = GetComponent<PlayerStats>();

        m_bodyAnimator = m_bodyGameObject.GetComponent<Animator>();
        m_maskAnimator = m_maskGameObject.GetComponent<Animator>();

        m_rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        SetDefaultValues();
        //set default mask
        RemoveMasks();
        m_warMask.enabled = true;
    }

    public void SetDefaultValues()
    {
        MovementSpeed = m_playerStats.m_CurrentMovementSpeed;
        m_playerStats.m_AttackDamage = m_playerStats.m_DefaultAttackDamage;
        m_jumpHeight = m_playerStats.m_CurrentJumpHeight;
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

        // HACK TO GET THE WALKING AND IDLE ANIMATIONS WORKING! REMOVE THIS AND DO PROPERLY AFTER THE CA MEETING PLEASE - TOM :)
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            m_bodyAnimator.SetTrigger(StringConstants.PLAYER_RUN);

            m_maskAnimator.SetTrigger(m_masks == eMasks.War
                ? StringConstants.WAR_MASK_RUN
                : StringConstants.NATURE_MASK_RUN);
        }
        
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            m_bodyAnimator.SetTrigger(StringConstants.PLAYER_IDLE);

            m_maskAnimator.SetTrigger(m_masks == eMasks.War
                ? StringConstants.WAR_MASK_IDLE
                : StringConstants.NATURE_MASK_IDLE);
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
        m_rigidbody.velocity = new Vector2(HorizontalDrag(), m_rigidbody.velocity.y);

        Vector2 clamper = m_rigidbody.velocity;
        clamper.x = Mathf.Clamp(clamper.x, -MovementSpeed, MovementSpeed);
        clamper.y = Mathf.Clamp(clamper.y, -m_fallingSpeed, m_jumpHeight);

        m_rigidbody.velocity = clamper;

        if ((m_playerInput.actions["Horizontal"].ReadValue<float>() > 0 && !m_facingRight) || (m_playerInput.actions["Horizontal"].ReadValue<float>() < 0 && m_facingRight))
        {
            Flip();
        }
    }

    private float HorizontalDrag()
    {
        float horizontalVelocity = m_rigidbody.velocity.x;
        horizontalVelocity += m_playerInput.actions["Horizontal"].ReadValue<float>();

        if (Mathf.Abs(m_playerInput.actions["Horizontal"].ReadValue<float>()) < 0.01f)
        {
            horizontalVelocity *= Mathf.Pow(MovementSpeed - m_dampingStop, Time.fixedDeltaTime * -m_drag);
        }
            
        else if (Mathf.Sign(m_playerInput.actions["Horizontal"].ReadValue<float>()) != Mathf.Sign(horizontalVelocity))
        {
            horizontalVelocity *= Mathf.Pow(MovementSpeed - m_dampingTurn, Time.fixedDeltaTime * -m_drag);
        }

        else
        {
            horizontalVelocity *= Mathf.Pow(MovementSpeed - m_dampingNormal, Time.fixedDeltaTime * -m_drag);
        }

        return horizontalVelocity;
    }

    private void Flip()
    {
        m_facingRight = !m_facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

#endregion

#region Jumping Functions

    private void Jumping()
    {
        if (m_playerInput.actions["Jump"].triggered)
        {
            m_jumpInputTimer = 0.2f;
        }


        if (m_jumpInputTimer > 0)
        {
            if (JumpAvaliable()
#if WALL_SLIDE
                && !m_wallSliding
#endif
                )
            {
                m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, 0);
                m_rigidbody.AddForce(new Vector2(0, m_jumpHeight), ForceMode2D.Impulse);

                m_ungroundedTimer = 0;
            }
#if WALL_SLIDE
            else if (m_wallSliding)
            {
                m_rigidbody.velocity = new Vector2(m_xWallForce * -m_playerInput.actions["Horizontal"].ReadValue<float>(), m_yWallForce);
            }
#endif

            else
            {
                m_jumpInputTimer -= Time.fixedDeltaTime;
            }  
        } 
        
        if (!IsGrounded())
        {
            m_holdTimer += Time.fixedDeltaTime;

            if (m_ungroundedTimer > 0)
            {
                m_ungroundedTimer -= Time.fixedDeltaTime;
            }
        }

        if (m_playerInput.actions["Jump"].ReadValue<float>() == 0 || m_holdTimer > 0.5f)
        {
#if WALL_SLIDE
            if (!m_wallSliding)
            {
                m_rigidbody.AddForce(new Vector2(0, -0.5f), ForceMode2D.Impulse);
            }

            else
#endif
            {
                m_rigidbody.AddForce(new Vector2(0, -0.25f), ForceMode2D.Impulse);
            }
        }
    }

    private bool JumpAvaliable()
    {
        return IsGrounded() || (!IsGrounded() && m_ungroundedTimer > 0);
    }

    private bool IsGrounded()
    {
        if (!Physics2D.OverlapCircle(m_groundCheck.position, 0.1f, m_groundLayer))
        {
            m_rigidbody.sharedMaterial = slipperyMat;
            boxCollider.sharedMaterial = slipperyMat;
            Debug.Log("AIRBORNE");
            return false;
            
        }

        Debug.Log("GROUNDED");
        
        if (m_ungroundedTimer > 0)
        {
            m_rigidbody.sharedMaterial = stickyMat;
            boxCollider.sharedMaterial = stickyMat;
        }
        
        m_ungroundedTimer = 0.2f;
        m_holdTimer = 0f;
        m_warMask.m_IsJumped = false;
        m_doubleJumped = false;
        return true;
    }

#endregion

#region Mask Input Function
    private void MaskInputs()
    {
        if (m_playerInput.actions["WarMask"].triggered) //will also include an if statement checking if the selected mask has been unlocked
        {
            m_masks = eMasks.War;
        }

        if (m_playerInput.actions["NatureMask"].triggered) //will also include an if statement checking if the selected mask has been unlocked
        {
            m_masks = eMasks.Nature;
        }

        if (m_playerInput.actions["EnergyMask"].triggered) //will also include an if statement checking if the selected mask has been unlocked
        {
            m_masks = eMasks.Energy; ;
        }

        if (m_playerInput.actions["SeaMask"].triggered) //will also include an if statement checking if the selected mask has been unlocked
        {
            m_masks = eMasks.Sea;
        }

        MaskChange();
    }

    private void MaskChange()
    {
        //TODO : Add other m_masks

        switch (m_masks)
        {
            case eMasks.War:
                if (!m_warMask.enabled)
                {
                    RemoveMasks();
                    m_warMask.enabled = true;
                    m_playerStats.MaskIconImage.sprite = m_playerStats.WarMaskIcon;
                }
                break;
            case eMasks.Nature:
                if (!m_natureMask.enabled)
                {
                    RemoveMasks();
                    m_natureMask.enabled = true;
                    m_playerStats.MaskIconImage.sprite = m_playerStats.NatureMaskIcon;
                }
                break;
        }
    }

    public eMasks GetSelectedMask()
    {
        return m_masks;
    }

    private void SpecialAttack()
    {
        if (m_playerInput.actions["Special"].triggered)
        {
            switch (m_masks)
            {
                case eMasks.War:
                    m_warMask.SpecialAttack();
                    break;
                case eMasks.Nature:
                    m_natureMask.SpecialAttack();
                    break;
            }
        }
    }

    private void RemoveMasks()
    {
        m_warMask.enabled = false;
        m_natureMask.enabled = false;
    }
#endregion

#region Base Combat Functions

    private void Attack()
    {
        if (m_playerInput.actions["Attack"].triggered)
        {
            m_bodyAnimator.SetTrigger("Attack");
        }
    }

#endregion

#region Extra Movement Functions
#if WALL_SLIDE
    private void WallSlide()
    {
        m_isTouchingFront = Physics2D.OverlapCircle(m_frontCheck.position, 0.1f, m_wallLayer);

        if (m_isTouchingFront == true && IsGrounded() == false && m_playerInput.actions["Horizontal"].ReadValue<float>() != 0)
            m_wallSliding = true;
        else
            m_wallSliding = false;
    }
#endif
    
    private void DoubleJump()
    {
        if (!JumpAvaliable() && m_playerInput.actions["Jump"].triggered && !m_doubleJumped)
        {
            Debug.Log("Double Jump Triggered");

            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, 0);
            m_rigidbody.AddForce(new Vector2(0, m_jumpHeight), ForceMode2D.Impulse);

            m_ungroundedTimer = 0;
            m_holdTimer = 0f;

            m_doubleJumped = true;
        }
    }

    public void AddImpulse(Vector2 force)
    {
        m_rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

#endregion
}
