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

    [SerializeField] private float m_attackCooldownDuration;
    private float m_attackCooldownTimer = 0f;

    private bool m_hasRisen = false;

    private Vector2 m_origin;

    private float m_sinCounter;

    public enum eState
    {
        eFlying,
        eDiveBomb,
        eCoolDown,
        eAttack
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
            case eState.eCoolDown:
                CoolDown();
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);

#if UNITY_EDITOR
        Debug.DrawRay(transform.position, Vector3.down, Color.cyan);
#endif

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag(StringConstants.PLAYER_TAG))
            {
                m_state = eState.eAttack;
                Attack();
            }
            else if(hit.collider.gameObject.CompareTag(StringConstants.GROUND_TAG))
            {
                // Move back up to the flying Height if we don't hit the player
                m_state = eState.eCoolDown;
            }
        }
    }

    private void CoolDown()
    {
        m_attackCooldownTimer += Time.deltaTime;

        if (m_attackCooldownTimer < m_attackCooldownDuration)
        {
            if (!m_hasRisen)
            {
                if (m_origin.y - transform.position.y < 0)
                {
                    // Fly diagonally to get to the initial height
                    m_rigidbody.velocity = Vector2.up * m_speed;

                    transform.position = new Vector3(
                        transform.position.x,
                        m_origin.y,
                        transform.position.y
                    );
                }
                else
                {
                    m_hasRisen = true;
                    DebugLog("REACHED THE HIGHEST POINT!");
                }
            }
            else
            {
                Move();
            }

            DebugLog("COOLING DOWN", BetterDebugging.eDebugLevel.Message);
        }
        else
        {
            DebugLog("COOLED DOWN", BetterDebugging.eDebugLevel.Message);
            m_attackCooldownTimer = 0f;
            m_state = eState.eFlying;
        }
    }

    protected override void Attack()
    {
        m_rigidbody.velocity = Vector2.zero;
        // TODO: Play sting animation
        // TODO: Play Sound Effect
        // TODO: Damage the Player

        // TODO: REPLACE WITH PROPER ATTACK STUFF... KEEPING IN PLACE FOR NOW
        StartCoroutine("AttackPlayer");
    }

    private IEnumerator AttackPlayer()
    {
        DebugLog("DAMAGING THE PLAYER!", BetterDebugging.eDebugLevel.Message);
        yield return new WaitForSeconds(1.0f);
        m_state = eState.eCoolDown;
        DebugLog("FINISHED ATTACKING, RISING BACK UP", BetterDebugging.eDebugLevel.Message);
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
        else if (m_state == eState.eDiveBomb && other.gameObject.CompareTag(StringConstants.PLAYER_TAG))
        {
            m_state = eState.eAttack;
            Attack();
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