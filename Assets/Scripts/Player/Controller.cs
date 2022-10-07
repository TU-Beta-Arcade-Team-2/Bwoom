using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    /// <summary> Player References </summary>
    private PlayerInput playerInput;

    /// <summary> Player Physics </summary>
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool facingRight = true;

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

    /// <summary> Player Jump Timers </summary>
    private float jumpInputTimer;
    private float ungroundedTimer;
    private float holdTimer;

    /// <summary> Abilitiy Booleans </summary>
    [SerializeField] private bool wallSlideOn;
    [SerializeField] private bool groundPoundOn;
    [SerializeField] private bool shootingOn;
    [SerializeField] private bool doubleJumpOn;
    [SerializeField] private bool healingOn;

    /// <summary> Wall Jump Values </summary>
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private float xWallForce;
    [SerializeField] private float yWallForce;
    [SerializeField] private float wallJumpTime;

    [SerializeField] private Transform frontCheck;
    [SerializeField] private LayerMask wallLayer;
    private bool isTouchingFront;
    private bool wallSliding;
    

    // Start is called before the first frame update
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Jumping();

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

    private void Jumping()
    {
        if (playerInput.actions["Jump"].triggered)
        {
            jumpInputTimer = 0.2f;
        }

        if (jumpInputTimer > 0)
        {
            if (JumpAvaliable())
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);

                ungroundedTimer = 0;
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
            if (wallSliding)
            {
                rb.velocity = new Vector2(xWallForce * -playerInput.actions["Horizontal"].ReadValue<float>(), yWallForce);
            }

            else
            {
                rb.AddForce(new Vector2(0, -0.5f), ForceMode2D.Impulse);
            }
        }
    }

    private void WallSlide()
    {
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, 0.1f, wallLayer);

        if (isTouchingFront == true && IsGrounded() == false && playerInput.actions["Horizontal"].ReadValue<float>() != 0)
            wallSliding = true;
        else
            wallSliding = false;

        if (wallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
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
            return false;
        }

        ungroundedTimer = 0.2f;
        holdTimer = 0f;
        return true;
    }
}
