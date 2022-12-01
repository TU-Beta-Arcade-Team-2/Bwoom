using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        Flying,
        DiveBomb,
        CoolDown,
        Attack
    }

    private eState m_state;

    // Start is called before the first frame update
    void Start()
    {
        m_origin = transform.position;

        Init("WaspEnemy");
        m_facingDirection = eDirection.Right;
        SetWaspState(eState.Flying);
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case eState.Flying:
                FlyingState();
                break;
            case eState.DiveBomb:
                DiveBombState();
                break;
            case eState.CoolDown:
                CoolDownState();
                break;
            default:
                DebugLog($"UNHANDLED CASE {m_state}");
                break;
        }
    }

    void LateUpdate()
    {
        DebugLog($"m_stateProperty: {m_state}");
    }

    public void SetWaspState(eState state)
    {
        m_state = state;

        switch (m_state)
        {
            case eState.Flying:
            case eState.DiveBomb:
            case eState.CoolDown:
                m_animator.SetTrigger(StringConstants.WASP_FLY);
                break;
            case eState.Attack:
                m_animator.SetTrigger(StringConstants.WASP_STING);
                break;
            default:
                DebugLog($"UNHANDLED CASE {m_state}");
                break;
        }
    }

    private void FlyingState()
    {
        float distanceToPlayer = FindSqrDistanceToPlayer();

        if (distanceToPlayer <= m_minDistanceToPlayer * m_minDistanceToPlayer && (m_playerStats.gameObject.transform.position.y <= transform.position.y))
        {
            SetWaspState(eState.DiveBomb);
        }
        else
        {
            Move();
        }
    }

    private void DiveBombState()
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
                SetWaspState(eState.Attack);
                Attack();
            }
            else if (hit.collider.gameObject.CompareTag(StringConstants.GROUND_TAG))
            {
                // Move back up to the flying Height if we don't hit the player
                SetWaspState(eState.CoolDown);
            }
        }
    }

    private void CoolDownState()
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
            SetWaspState(eState.Flying);
        }
    }

    protected override void Attack()
    {
        m_rigidbody.velocity = Vector2.zero;
        StartCoroutine("AttackPlayer");
    }

    private IEnumerator AttackPlayer()
    {
        DebugLog("DAMAGING THE PLAYER!", BetterDebugging.eDebugLevel.Message);
        SoundManager.Instance.PlaySfx(m_attackSfxName);
        m_playerStats.TakeDamage(m_damage);
        SetWaspState(eState.Attack);
        yield return new WaitForSeconds(2.0f);
        SetWaspState(eState.CoolDown);
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
        // TODO: ENSURE THAT THE PLAYER HAS A TRIGGER BOX THAT ISN'T ON THE PLAYER LAYER SINCE WE'RE IGNORING PHYSICS COLLISIONS NOW AND PREFERRING TRIGGERS!
        else if (m_state == eState.DiveBomb && other.gameObject.CompareTag(StringConstants.PLAYER_TAG))
        {
            SetWaspState(eState.Attack);
            Attack();
        }
    }

    [CustomEditor(typeof(WaspEnemy))]
    public class WaspEnemyEditor : Editor
    {
        private int m_animationIndex = 0;
        private int m_previousAnimationIndex;

        private readonly string[] m_states =
        {
            "Flying",
            "DiveBomb",
            "CoolDown",
            "Attack"
        };

        public override void OnInspectorGUI()
        {
            WaspEnemy myTarget = (WaspEnemy)target;

            // Needs to be initialised otherwise null reference exceptions happen for 
            // components
            myTarget.Init("Wasp");

            // Turn around button for the wasp, sets the facing direction and also flips the sprite!
            if (GUILayout.Button("Turn Around"))
            {
                myTarget.TurnAround();
            }

            // Change the animation state of the wasp
            m_animationIndex = EditorGUILayout.Popup(m_animationIndex, m_states);

            if (m_animationIndex != m_previousAnimationIndex)
            {
                myTarget.SetWaspState((WaspEnemy.eState)m_animationIndex);
            }

            m_previousAnimationIndex = m_animationIndex;

            DrawDefaultInspector();
        }
    }
}
