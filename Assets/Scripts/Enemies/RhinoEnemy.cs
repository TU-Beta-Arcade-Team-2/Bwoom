using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class RhinoEnemy : EnemyBase
{
    [SerializeField] private float m_preChargeDuration;
    [SerializeField] private float m_chargeDuration;
    [SerializeField] private float m_headbuttDuration;
    [SerializeField] private float m_knockbackAmount;
    [SerializeField] private float m_knockbackHeight;
    [SerializeField] private float m_coolDownDuration;
    [SerializeField] private Vector2 m_minDistanceToPlayer;

    public enum eState
    {
        Idle,
        PreCharge,
        Charging,
        Headbutt,
        CoolDown,
    }

    private eState m_state;

    private float m_preChargeTimer;
    private float m_chargeTimer;
    private float m_headbuttTimer;
    private float m_coolDownTimer;

    [SerializeField] private AnimationCurve m_chargeCurve;

    [SerializeField] private float m_chargeSpeed;

    [SerializeField] private GameObject chargeVFX;

    // Start is called before the first frame update
    private void Start()
    {
        Init("RhinoEnemy");
        SetRhinoState(eState.Idle);
    }

    // Update is called once per frame
    private void Update()
    {
        switch (m_state)
        {
            case eState.Idle:
                IdleState();
                break;
            case eState.PreCharge:
                PreChargeState();
                break;
            case eState.Charging:
                ChargeState();
                break;
            case eState.Headbutt:
                HeadbuttState();
                break;
            case eState.CoolDown:
                CoolDownState();
                break;
            default:
                DebugLog("UNHANDLED STATE IN UPDATE!", BetterDebugging.eDebugLevel.Warning);
                break;
        }
    }

    void LateUpdate()
    {
        DebugLog($"m_stateProperty: {m_state}");
    }

    [ExecuteInEditMode]
    public void SetRhinoState(eState state)
    {
        m_state = state;
        switch (m_state)
        {
            case eState.Idle:
                m_animator.SetTrigger(StringConstants.RHINO_WALK_CYCLE);
                break;
            case eState.PreCharge:
                m_animator.SetTrigger(StringConstants.RHINO_PRE_CHARGE);
                break;
            case eState.Charging:
                m_animator.SetTrigger(StringConstants.RHINO_CHARGE);
                SoundManager.Instance.PlaySfx("RhinoChargeSFX");
                break;
            case eState.Headbutt:
                DebugLog("HEADBUTT!", BetterDebugging.eDebugLevel.Warning);
                m_animator.SetTrigger(StringConstants.RHINO_HEADBUTT);
                SoundManager.Instance.PlaySfx(m_attackSfxName);
                break;
            case eState.CoolDown:
                m_animator.ResetTrigger(StringConstants.RHINO_WALK_CYCLE);
                m_animator.SetTrigger(StringConstants.RHINO_COOL_DOWN);
                break;
            default:
                DebugLog($"UNKNOWN ENUM VALUE {nameof(m_state)}", BetterDebugging.eDebugLevel.Error);
                break;
        }

        ResetTimers();
    }
    private void ResetTimers()
    {
        m_preChargeTimer = 0f;
        m_chargeTimer = 0f;
        m_headbuttTimer = 0f;
        m_coolDownTimer = 0f;
    }

    private void IdleState()
    {
        // If the player is within range, begin charging
        if (FindSqrDistanceToPlayer() <= m_minDistanceToPlayer.x * m_minDistanceToPlayer.x)
        {
            // Log("PLAYER IS IN RANGE!");
            Vector2 vectorToPlayer = GetVectorToPlayer();

            // Log($"PLAYER VECTOR {vectorToPlayer.x}  {vectorToPlayer.y}");

            // If we are facing the correct direction, and around the right height
            // range, charge at the player
            if ((vectorToPlayer.x <= 0 && m_facingDirection == eDirection.Left ||
                 vectorToPlayer.x >= 0 && m_facingDirection == eDirection.Right) &&
                vectorToPlayer.y <= m_minDistanceToPlayer.y)
            {
                // If we've started charging, reset the cooldown timer
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

    private void PreChargeState()
    {
        m_preChargeTimer += Time.deltaTime;

        if (m_preChargeTimer <= m_preChargeDuration) { return; }

        SetRhinoState(eState.Charging);

        GameObject vfx = GameObject.Instantiate(chargeVFX, transform.position, transform.rotation);
        vfx.transform.parent = transform;

        m_preChargeTimer = 0f;
    }

    private void ChargeState()
    {
        m_chargeTimer += Time.deltaTime;

        Attack();

        // If we have charged and not hit the player
        if (m_chargeTimer <= m_chargeDuration) { return; }

        DebugLog("CHARGE DURATION ENDED!");

        StopCharge();
    }

    private void HeadbuttState()
    {
        // TODO: Make him slide to a halt instead of zeroing out
        m_rigidbody.velocity = Vector2.zero;
        m_headbuttTimer += Time.deltaTime;
        if (m_headbuttTimer <= m_headbuttDuration) { return; }

        DebugLog("HEADBUTTING THE PLAYER!");

        StopCharge();
    }

    private void CoolDownState()
    {
        m_coolDownTimer += Time.deltaTime;
        DebugLog("COOLING DOWN AFTER A CHARGE!");

        if (m_coolDownTimer <= m_coolDownDuration) { return; }

        SetRhinoState(eState.Idle);
    }

    private void HeadbuttPlayer()
    {
        Vector2 force = new Vector2(
            (int)m_facingDirection * m_knockbackAmount,
            m_knockbackHeight
        );

        // HACK - DON'T USE THIS IN FINAL GAME
        m_playerStats.gameObject.GetComponent<PlayerController>().AddImpulse(force);

        DamagePlayer(m_damage);
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

    protected override void Move()
    {
        m_rigidbody.velocity = new Vector2(
            (int)m_facingDirection * m_speed,
            m_rigidbody.velocity.y
        );
    }


    private void StartCharge()
    {
        SetRhinoState(eState.PreCharge);
        // TODO: Play animation
        // TODO: Play sound effect
        DebugLog("STARTING TO CHARGE");
    }

    private void StopCharge()
    {
        SetRhinoState(eState.CoolDown);
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(StringConstants.END_OF_PLATFORM_TAG))
        {
            TurnAround();

            // Not sure if we want him to stop at the end of the platform
            // when we reach the end, or if we want him to continue charging
            if (m_state == eState.Charging)
            {
                SetRhinoState(eState.CoolDown);
            }
        }
        // TODO: ENSURE THAT THE PLAYER HAS A TRIGGER BOX THAT ISN'T ON THE PLAYER LAYER SINCE WE'RE IGNORING PHYSICS COLLISIONS NOW AND PREFERRING TRIGGERS!
        else if (other.gameObject.CompareTag(StringConstants.PLAYER_TAG) && m_state == eState.Charging) // If we are attacking and hit the player, play the headbutt animation
        {
            DebugLog("HIT THE PLAYER WHILST CHARGING!");
            HeadbuttPlayer();
            SetRhinoState(eState.Headbutt);
        }
    }
}

#if UNITY_EDITOR
    [CustomEditor(typeof(RhinoEnemy))]
    public class RhinoEnemyEditor : Editor
    {
        private int m_animationIndex = 0;
        private int m_previousAnimationIndex;

        private readonly string[] m_states =
        {
            "Idle",
            "PreCharge",
            "Charging",
            "Headbutt",
            "CoolDown"
        };

        public override void OnInspectorGUI()
        {
            RhinoEnemy myTarget = (RhinoEnemy)target;

            // Needs to be initialised otherwise null reference exceptions happen for 
            // components
            myTarget.Init("Rhino");

            // Turn around button for the rhino, sets the facing direction and also flips the sprite!
            if (GUILayout.Button("Turn Around"))
            {
                myTarget.TurnAround();
            }

            // Change the animation state of the rhino
            m_animationIndex = EditorGUILayout.Popup(m_animationIndex, m_states);

            if (m_animationIndex != m_previousAnimationIndex)
            {
                myTarget.SetRhinoState((RhinoEnemy.eState)m_animationIndex);
            }

            m_previousAnimationIndex = m_animationIndex;

            DrawDefaultInspector();
        }
    }
#endif
