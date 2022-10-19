using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public sealed class RhinoEnemy : EnemyBase
{
    [SerializeField] private float m_preChargeDuration;
    [SerializeField] private float m_chargeDuration;
    [SerializeField] private float m_headbuttDuration;
    [SerializeField] private float m_coolDownDuration;
    [SerializeField] private Vector2 m_minDistanceToPlayer;

    public enum eState
    {
        Idle,
        PreCharge,
        Charging,
        Headbutt,
        CoolDown,
        Dead
    }

    private eState m_stateProperty
    {
        get => m_state;
        set
        {
            m_state = value;
            switch (value)
            {
                case eState.Idle:
                    m_animator.SetTrigger(StringConstants.RHINO_WALK_CYCLE);
                    break;
                case eState.PreCharge:
                    m_animator.SetTrigger(StringConstants.RHINO_PRE_CHARGE);
                    break;
                case eState.Charging:
                    m_animator.SetTrigger(StringConstants.RHINO_CHARGE);
                    break;
                case eState.Headbutt:
                    DebugLog("HEADBUTT!", BetterDebugging.eDebugLevel.Warning);
                    m_animator.SetTrigger(StringConstants.RHINO_HEADBUTT);
                    break;
                case eState.CoolDown:
                    m_animator.ResetTrigger(StringConstants.RHINO_WALK_CYCLE);
                    m_animator.SetTrigger(StringConstants.RHINO_COOL_DOWN);
                    break;
                case eState.Dead:
                    m_animator.SetTrigger(StringConstants.RHINO_DEATH);
                    break;
                default:
                    DebugLog($"UNKNOWN ENUM VALUE {nameof(value)}", BetterDebugging.eDebugLevel.Error);
                    break;
            }
        }
    }

    // TODO: Better encapsulate this variable so that m_stateProperty can only be changed through the m_stateProperty property above
    private eState m_state;

    private float m_preChargeTimer;
    private float m_chargeTimer;
    private float m_headbuttTimer;
    private float m_coolDownTimer;

    [SerializeField] private AnimationCurve m_chargeCurve;

    [SerializeField] private float m_chargeSpeed;

    // Start is called before the first frame update
    private void Start()
    {
        Init("RhinoEnemy");
        TurnAround();
        m_stateProperty = eState.Idle;
    }

    // Update is called once per frame
    private void Update()
    {
        switch (m_stateProperty)
        {
            case eState.Idle:
                {
                    // If the player is within range, begin charging
                    if (FindSqrDistanceToPlayer() <= m_minDistanceToPlayer.x * m_minDistanceToPlayer.x)
                    {
                        // DebugLog("PLAYER IS IN RANGE!");
                        Vector2 vectorToPlayer = GetVectorToPlayer();

                        // DebugLog($"PLAYER VECTOR {vectorToPlayer.x}  {vectorToPlayer.y}");

                        // If we are facing the correct direction, and around the right height
                        // range, charge at the player
                        if ((vectorToPlayer.x <= 0 && m_facingDirection == eDirection.eLeft ||
                            vectorToPlayer.x >= 0 && m_facingDirection == eDirection.eRight) &&
                            vectorToPlayer.y <= m_minDistanceToPlayer.y)
                        {
                            // If we've started charging, reset the cooldown timer
                            m_coolDownTimer = 0f;

                            StartCharge();
                        }
                        else
                        {
                            DebugLog("PLAYER ISN'T IN FRONT");
                            Move();
                        }
                    }
                    else
                    {
                        DebugLog("PLAYER ISN'T IN RANGE!");
                        Move();
                    }
                }
                break;
            case eState.PreCharge:
                {
                    DebugLog("PRE-CHARGE");
                    m_preChargeTimer += Time.deltaTime;
                    if (m_preChargeTimer >= m_preChargeDuration)
                    {
                        m_stateProperty = eState.Charging;

                        m_preChargeTimer = 0f;
                    }
                }
                break;
            case eState.Charging:
                {
                    DebugLog("CHARGING!");

                    m_chargeTimer += Time.deltaTime;

                    Attack();

                    // If we have charged and not hit the player
                    if (m_chargeTimer >= m_chargeDuration)
                    {
                        DebugLog("CHARGE DURATION ENDED!");

                        StopCharge();
                    }
                }
                break;
            case eState.Headbutt:
                {
                    m_headbuttTimer += Time.deltaTime;
                    if (m_headbuttTimer >= m_headbuttDuration)
                    {
                        DebugLog("HEADBUTTING THE PLAYER!");
                        m_headbuttTimer = 0f;
                        StopCharge();
                    }
                }
                break;
            case eState.CoolDown:
                m_coolDownTimer += Time.deltaTime;
                DebugLog("COOLING DOWN AFTER A CHARGE!");

                if (m_coolDownTimer >= m_coolDownDuration)
                {
                    m_stateProperty = eState.Idle;
                }
                break;
            case eState.Dead:
                break;
            default:
                DebugLog("UNHANDLED STATE IN UPDATE!", BetterDebugging.eDebugLevel.Warning);
                break;
        }
    }

    void LateUpdate()
    {
        DebugLog($"m_stateProperty: {m_stateProperty}");
    }

    // The Rhino's attack is to charge toward the player
    // When it stops its charge, it will be dazed for a few seconds whilst it
    // cools down to get ready to attack again
    protected override void Attack()
    {
        // Read the speed multiplier from the curve
        // X is from 0 to 1, so find out how far along the curve
        // we are to read the Y
        float valueFromCurve = m_chargeCurve.Evaluate(m_chargeTimer / m_chargeDuration);

        m_rigidbody.velocity = new Vector2(
            (int)m_facingDirection * (m_chargeSpeed * valueFromCurve),
            m_rigidbody.velocity.y
        );
    }

    protected override void OnDeath()
    {
        m_stateProperty = eState.Dead;
        // TODO: PLAY SFX
        // Award Points
        // Drop Items
    }

    protected override void Move()
    {
        m_rigidbody.velocity = new Vector2(
            (int)m_facingDirection * m_speed,
            0f
        );
    }


    private void StartCharge()
    {
        m_stateProperty = eState.PreCharge;
        m_chargeTimer = 0f;
        // TODO: Play animation
        // TODO: Play sound effect
        DebugLog("STARTING TO CHARGE");
    }

    private void StopCharge()
    {
        m_stateProperty = eState.CoolDown;
        // TODO: Play animation
        // TODO: Play sound effect
        DebugLog("STOPPING THE CHARGE");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(StringConstants.WALL_TAG))
        {
            TurnAround();
        }
        else if (other.gameObject.CompareTag(StringConstants.PLAYER_TAG) && m_stateProperty == eState.Charging) // If we are attacking and hit the player, play the headbutt animation
        {
            DebugLog("HIT THE PLAYER WHILST CHARGING!");
            m_stateProperty = eState.Headbutt;
            DamagePlayer(m_damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(StringConstants.END_OF_PLATFORM_TAG))
        {
            TurnAround();

            // Not sure if we want him to stop at the end of the platform
            // when we reach the end, or if we want him to continue charging
            if (m_stateProperty == eState.Charging)
            {
                m_stateProperty = eState.CoolDown;
            }
        }
    }
}
