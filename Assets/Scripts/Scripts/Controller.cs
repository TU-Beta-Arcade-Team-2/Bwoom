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

    // Start is called before the first frame update
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        Jumping();
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
            Flip();
    }

    private float HorizontalDrag()
    {
        float horizontalVelocity = rb.velocity.x;
        horizontalVelocity += playerInput.actions["Horizontal"].ReadValue<float>();

        if (Mathf.Abs(playerInput.actions["Horizontal"].ReadValue<float>()) < 0.01f)
            horizontalVelocity *= Mathf.Pow(movementSpeed - dampingStop, Time.deltaTime * -drag);
        else if (Mathf.Sign(playerInput.actions["Horizontal"].ReadValue<float>()) != Mathf.Sign(horizontalVelocity))
            horizontalVelocity *= Mathf.Pow(movementSpeed - dampingTurn, Time.deltaTime * -drag);
        else
            horizontalVelocity *= Mathf.Pow(movementSpeed - dampingNormal, Time.deltaTime * -drag);

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
                jumpInputTimer -= Time.deltaTime;
        }

        if (!IsGrounded() && ungroundedTimer > 0)
            ungroundedTimer -= Time.deltaTime;

        if (!IsGrounded())
        {
            holdTimer += Time.deltaTime;
        }

        if (playerInput.actions["Jump"].ReadValue<float>() == 0 || holdTimer > 0.5f)
        {
            rb.AddForce(new Vector2(0, -0.5f), ForceMode2D.Impulse);
        }
    }

    private bool JumpAvaliable()
    {
        if (IsGrounded())
            return true;

        else if (!IsGrounded() && ungroundedTimer > 0)
            return true;

        else
            return false;
    }

    private bool IsGrounded()
    {
        if (Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer))
        {
            ungroundedTimer = 0.2f;
            holdTimer = 0f;
            return true;
        }

        else
        {
            return false;
        }
    }
}
