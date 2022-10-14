using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class RhinoEnemy : EnemyBase
{
    [SerializeField] private float m_chargeDuration;
    [SerializeField] private float m_coolDownDuration;
    [SerializeField] private float m_minDistanceToPlayer;

    public enum eState
    {
        Idle,
        Charging,
        CoolDown,
        Dead
    }

    private eState m_state;

    private float m_chargeTimer;
    private float m_coolDownTimer;

    [SerializeField] private float m_chargeMultiplier;
    [SerializeField] private AnimationCurve m_chargeCurve;

    // Start is called before the first frame update
    private void Start()
    {
        Init();
        FacingDirection = eDirection.eLeft;
        m_state = eState.Idle;
    }

    // Update is called once per frame
    private void Update()
    {
        switch (m_state)
        {
            case eState.Idle:
                {
                    // If the player is within range, begin charging
                    if (FindSqrDistanceToPlayer() <= m_minDistanceToPlayer * m_minDistanceToPlayer)
                    {
                        Debug.Log("RHINO: PLAYER IS IN RANGE!");
                        Vector2 vectorToPlayer = GetVectorToPlayer();

                        Debug.Log($"RHINO: PLAYER VECTOR {vectorToPlayer.x}  {vectorToPlayer.y}");

                        // If we are facing the correct direction, charge at the player
                        if (vectorToPlayer.x >= 0 && FacingDirection == eDirection.eLeft ||
                            vectorToPlayer.x <= 0 && FacingDirection == eDirection.eRight)
                        {
                            // If we've started charging, reset the cooldown timer
                            m_coolDownTimer = 0f;

                            StartCharge();
                        }
                    }
                    else
                    {
                        // TODO: Idly walk around
                        Debug.Log("RHINO: PLAYER ISN'T IN RANGE!");
                        Move();
                    }
                }
                break;
            case eState.Charging:
                {
                    Debug.Log("RHINO: CHARGING!");

                    m_chargeTimer += Time.deltaTime;

                    Attack();

                    if (m_chargeTimer >= m_chargeDuration)
                    {
                        Debug.Log("RHINO: CHARGE DURATION ENDED!");

                        StopCharge();
                    }
                }
                break;
            case eState.CoolDown:
                m_coolDownTimer += Time.deltaTime;
                Debug.Log("RHINO: COOLING DOWN AFTER A CHARGE!");

                if (m_coolDownTimer >= m_coolDownDuration)
                {
                    m_state = eState.Idle;
                }
                break;
            case eState.Dead:
                break;
            default:
                Debug.Log("RHINO: UNHANDLED STATE IN UPDATE!");
                break;
        }

        ShowDebugText($"State: {m_state}", false);
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
            (int)FacingDirection * (m_speed + m_chargeMultiplier * valueFromCurve),
            m_rigidbody.velocity.y
        );
    }

    protected override void OnDeath()
    {
        throw new System.NotImplementedException();
    }

    protected override void Move()
    {
        // TODO: Walk side to side
    }

    // Return a vector pointing to the player
    private Vector2 GetVectorToPlayer()
    {
        return new Vector2(
            transform.position.x - m_playerStats.gameObject.transform.position.x,
            transform.position.y - m_playerStats.gameObject.transform.position.y
        );
    }

    // Return the SqrMag distance of the player, 
    private float FindSqrDistanceToPlayer()
    {
        Vector2 aToB = GetVectorToPlayer();

        return aToB.x * aToB.x + aToB.y * aToB.y;
    }

    private void StartCharge()
    {
        m_state = eState.Charging;
        m_chargeTimer = 0f;
        // Play animation
        // Play sound effect
        Debug.Log("RHINO: STARTING TO CHARGE");
    }

    private void StopCharge()
    {
        m_state = eState.CoolDown;
        // Come to a stop
        Debug.Log("RHINO: STOPPING THE CHARGE");
    }

}
