using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspEnemy : EnemyBase
{
    [SerializeField] private float m_bobHeight;
    [SerializeField] private float m_bobSpeed;
    [SerializeField] private bool m_limitedToPlatform;

    [SerializeField] private float m_minDistanceToPlayer;

    [SerializeField] private float m_diveBombSpeed;

    private Vector2 m_origin;

    private float m_sinCounter;

    public enum eState
    {
        eFlying,
        eDiveBomb,
        eRiseUp,
        eAttack,
        eDead
    }

    private eState m_state;

    // Start is called before the first frame update
    void Start()
    {
        m_origin = transform.position;

        Init("WaspEnemy");
        m_facingDirection = eDirection.eRight;
        m_state = eState.eFlying;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case eState.eFlying:
                {
                    float distanceToPlayer = FindSqrDistanceToPlayer();
                    if (distanceToPlayer <= m_minDistanceToPlayer * m_minDistanceToPlayer)
                    {
                        m_state = eState.eDiveBomb;
                        DiveBomb();
                    }
                    else
                    {
                        Move();
                    }
                }
                break;
            case eState.eDiveBomb:
                DiveBomb();
                break;
            case eState.eRiseUp:
                RiseUp();
                break;
            default:
                DebugLog($"UNHANDLED CASE {m_state}");
                break;
        }
    }

    void LateUpdate()
    {
        DebugLog($"State: {m_state}");
    }

    private void DiveBomb()
    {
        // Find vector to player
        Vector2 pointTowardsPlayer = GetVectorToPlayer();

        // Normalise to get direction
        pointTowardsPlayer.Normalize();

        m_rigidbody.velocity = pointTowardsPlayer * m_diveBombSpeed;

        // TODO: Attack the player with the stinger

        // Stop from hitting the ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.3f);

#if UNITY_EDITOR
        Debug.DrawRay(transform.position, Vector3.down, Color.cyan);
#endif

        if (hit.collider != null)
        {
            // Move back up to the flying Height
            m_state = eState.eRiseUp;
        }
    }

    private void RiseUp()
    {
        // Fly diagonally upwards to get to the initial height
        Vector2 toOrigin = new Vector2(
            m_origin.x - transform.position.x,
            m_origin.y - transform.position.y
        );

        toOrigin.Normalize();

        m_rigidbody.velocity = toOrigin * m_speed;

        if (Vector2.Distance(transform.position, m_origin) <= 0.5)
        {
            m_state = eState.eFlying;
        }
    }

    protected override void Attack()
    {

    }

    protected override void Move()
    {
        // Make the wasp float in the air with a sine curve
        m_sinCounter += Time.deltaTime * m_bobSpeed;
        if (m_sinCounter > 180f)
        {
            m_sinCounter = -180f;
        }

        m_rigidbody.velocity = new Vector2(
            (int)m_facingDirection * m_speed,
            Mathf.Sin(m_sinCounter) * m_speed * m_bobHeight

        );
    }

    protected override void OnDeath()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(StringConstants.WALL_TAG))
        {
            TurnAround();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_limitedToPlatform && other.gameObject.CompareTag(StringConstants.END_OF_PLATFORM_TAG))
        {
            TurnAround();
        }
    }
}
