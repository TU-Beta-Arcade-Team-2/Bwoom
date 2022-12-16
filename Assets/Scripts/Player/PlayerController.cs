#undef UNUSED_ABILITES
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /// <summary> Player References </summary>
    private PlayerInput m_playerInput;
    private PlayerStats m_playerStats;
    private Rigidbody2D m_rigidbody;

    [SerializeField] private GameObject m_bodyGameObject;
    [SerializeField] private GameObject m_maskGameObject;
    private Animator m_animator;


    /// <summary> Player Hidden Variables </summary>
    private float m_jumpInputTimer;
    private float m_ungroundedTimer;
    private float m_holdTimer;
    private bool m_doubleJumped;
    private bool m_facingRight = true;
#if UNITY_ANDROID || UNITY_IOS
    private MobileScreenControls m_screenControls;
#endif

    [Header("Ground Checking")]
    /// <summary> Player Physics </summary>
    [SerializeField] private Transform m_groundCheck;
    [SerializeField] private LayerMask m_groundLayer;
    [Space(5)]

    [SerializeField] private float m_fallingSpeed;

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

#if UNUSED_ABILITIES
    [Header("Ability Bools")]
    /// <summary> Abilitiy Booleans </summary>
    public bool wallSlideOn;
    public bool groundPoundOn;
    public bool shootingOn;
    public bool healingOn;
    [Space(5)]
#endif
    public bool doubleJumpOn;


    [SerializeField] private float m_airTime;
    [SerializeField] private float m_inputTime;
    private float m_jumpHeight;
    private float m_movementSpeed;
    private bool m_natureMaskSelected = false;

#if UNUSED_ABILITES
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

   public eMasks masks;
    private bool m_canTakeInput = true;

    #region Main Functions
    // Start is called before the first frame update
    private void Start()
    {
        m_playerInput = GetComponent<PlayerInput>();
        m_playerStats = GetComponent<PlayerStats>();

        m_animator = GetComponentInChildren<Animator>();

        m_rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

#if UNITY_ANDROID || UNITY_IOS
        m_screenControls = FindObjectOfType<MobileScreenControls>();
#endif

        //set default mask
        RemoveMasks();
        m_warMask.enabled = true;
    }

    private void Update()
    {
        if (m_canTakeInput)
        {
            Jumping();
            SpecialAttack();
            MaskInputs();

#if UNUSED_ABILITES
        if (wallSlideOn)
        {
            WallSlide();
        }

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
#endif

            if (m_playerInput.actions["Pause"].triggered)
            {
                GameManager.Instance.OnPauseButtonPressed();
            }
#if UNITY_ANDROID || UNITY_IOS
            if (m_playerInput.actions["Attack"].triggered)
            {
                m_screenControls.AttackButtonAnim();
            }
#endif
            m_animator.SetFloat("Movement", Mathf.Abs(m_playerInput.actions["Horizontal"].ReadValue<float>()));
        }

        m_animator.SetBool("IsGrounded", IsGrounded());
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (m_canTakeInput)
        {
            Movement();
        }
    }

    #endregion

    #region Basic Movement Functions
    private void Movement()
    {
        m_rigidbody.velocity = new Vector2(HorizontalDrag(), m_rigidbody.velocity.y);

        Vector2 clamper = m_rigidbody.velocity;
        clamper.x = Mathf.Clamp(clamper.x, -m_movementSpeed, m_movementSpeed);
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

        if (m_playerInput.actions["Horizontal"].ReadValue<float>() > 0)
        {
            horizontalVelocity += 1;
        }
        else if (m_playerInput.actions["Horizontal"].ReadValue<float>() < 0)
        {
            horizontalVelocity -= 1;
        }

        float subValue;

        if (Mathf.Abs(m_playerInput.actions["Horizontal"].ReadValue<float>()) < 0.01f)
        {
            subValue = m_dampingStop;
        }
        else if (Mathf.Sign(m_playerInput.actions["Horizontal"].ReadValue<float>()) != Mathf.Sign(horizontalVelocity))
        {
            subValue = m_dampingTurn;
        }
        else
        {
            subValue = m_dampingNormal;
        }

        return horizontalVelocity * Mathf.Pow(m_movementSpeed - subValue, Time.fixedDeltaTime * -m_drag);
    }

    private void Flip()
    {
        m_facingRight = !m_facingRight;
        //m_playerStats.m_ComboAttackEnemyLaunchVector1.x *= -1;
        //m_playerStats.m_ComboAttackEnemyLaunchVector2.x *= -1;
        //m_playerStats.m_ComboAttackEnemyLaunchVector3.x *= -1;
        //m_playerStats.m_ComboAttackPlayerLaunchVector1.x *= -1;
        //m_playerStats.m_ComboAttackPlayerLaunchVector2.x *= -1;
        //m_playerStats.m_ComboAttackPlayerLaunchVector3.x *= -1;
        transform.Rotate(0f, 180f, 0f);
    }

    #endregion

    #region Jumping Functions

    private void Jumping()
    {
        if (m_playerInput.actions["Jump"].triggered)
        {
#if UNITY_ANDROID || UNITY_IOS
            m_screenControls.JumpButtonAnim();
#endif
            m_jumpInputTimer = m_airTime;
        }


        if (m_jumpInputTimer > 0)
        {
            if (JumpAvaliable()
#if UNUSED_ABILITES
                && !m_wallSliding
#endif
                )
            {
                m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, 0);
                m_rigidbody.AddForce(new Vector2(0, m_jumpHeight), ForceMode2D.Impulse);

                SoundManager.Instance.PlaySfx("PlayerJumpSFX");
                m_ungroundedTimer = 0;
                m_jumpInputTimer = 0;
            }
#if UNUSED_ABILITES
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

        if (m_playerInput.actions["Jump"].ReadValue<float>() == 0 || m_holdTimer > m_inputTime)
        {
#if UNUSED_ABILITES
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
            BetterDebugging.Log("AIRBORNE");
            return false;

        }

        BetterDebugging.Log("GROUNDED");

        if (m_ungroundedTimer > 0)
        {
            m_rigidbody.sharedMaterial = stickyMat;
            boxCollider.sharedMaterial = stickyMat;
        }

        m_ungroundedTimer = 0.075f;
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
            masks = eMasks.War;
            m_animator.SetTrigger(StringConstants.WAR_MASK);

#if UNITY_ANDROID || UNITY_IOS
            m_screenControls.Switch(true);
#endif
        }

        if (m_playerInput.actions["NatureMask"].triggered) //will also include an if statement checking if the selected mask has been unlocked
        {
            masks = eMasks.Nature;
            m_animator.SetTrigger(StringConstants.NATURE_MASK);

#if UNITY_ANDROID || UNITY_IOS
            m_screenControls.Switch(false);
#endif
        }

        if (m_playerInput.actions["EnergyMask"].triggered) //will also include an if statement checking if the selected mask has been unlocked
        {
            masks = eMasks.Energy;
        }

        if (m_playerInput.actions["SeaMask"].triggered) //will also include an if statement checking if the selected mask has been unlocked
        {
            masks = eMasks.Sea;
        }

        if (m_playerInput.actions["Switch"].triggered) //will also include an if statement checking if the selected mask has been unlocked
        {
#if UNITY_ANDROID || UNITY_IOS
            m_screenControls.SwitchButtonAnim();
#endif

            if (m_natureMaskSelected)
            {
                masks = eMasks.War;
                m_animator.SetTrigger(StringConstants.WAR_MASK);

#if UNITY_ANDROID || UNITY_IOS
                m_screenControls.Switch(true);
#endif
            }

            else
            {
                masks = eMasks.Nature;
                m_animator.SetTrigger(StringConstants.NATURE_MASK);

#if UNITY_ANDROID || UNITY_IOS
                m_screenControls.Switch(false);
#endif
            }

            m_natureMaskSelected = !m_natureMaskSelected;
        }

        MaskChange();
    }

    private void MaskChange()
    {
        //TODO : Add other m_masks

        switch (masks)
        {
            case eMasks.War:
                if (!m_warMask.enabled)
                {
                    RemoveMasks();
                    m_warMask.enabled = true;
                    GameHUD.Instance.UpdateMaskIcon(eMasks.War);
                    SoundManager.Instance.PlaySfx("MaskChangeSFX");
                }
                break;
            case eMasks.Nature:
                if (!m_natureMask.enabled)
                {
                    RemoveMasks();
                    m_natureMask.enabled = true;
                    GameHUD.Instance.UpdateMaskIcon(eMasks.Nature);
                    SoundManager.Instance.PlaySfx("MaskChangeSFX");
                }
                break;
        }
    }

    public eMasks GetSelectedMask()
    {
        return masks;
    }

    private void SpecialAttack()
    {
        if (m_playerInput.actions["Special"].triggered)
        {
#if UNITY_ANDROID || UNITY_IOS
            m_screenControls.SpecialButtonAnim();
#endif

            switch (masks)
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

    #region Extra Movement Functions
#if UNUSED_ABILITES
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

    public void SetMovementValues(PlayerStats.Stats stats)
    {
        m_jumpHeight = stats.JumpHeight;
        Debug.Log("Jump height is now: " + m_jumpHeight);
        m_movementSpeed = stats.MovementSpeed;
    }

    public void SetCanTakeInput(bool canTakeInput)
    {
        m_canTakeInput = canTakeInput;
    }
}
