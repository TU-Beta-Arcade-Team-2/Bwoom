using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AntEnemy : EnemyBase
{
    [SerializeField] private float m_coolDownDuration;
    private float m_coolDownTimer;

    private enum eAntState
    {
        Move,
        CoolDown
    }

    private eAntState m_antState = eAntState.Move;

    private eAntState AntState
    {
        get => m_antState; 
        
        set
        {
            switch(value)
            {
                case eAntState.Move:
                    m_animator.SetTrigger(StringConstants.ANT_WALK_CYCLE);
                    break;
            }
            m_antState = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Init("AntEnemy");
    }

    // Update is called once per frame
    void Update()
    {
        switch(AntState)
        {
            case eAntState.Move:
                Move();
                break;
            case eAntState.CoolDown:
                m_coolDownTimer += Time.deltaTime;
                if(m_coolDownTimer > m_coolDownDuration)
                {
                    AntState = eAntState.Move;
                    m_coolDownTimer = 0f;
                }
                break;
        }
    }

    protected override void Attack()
    {
        m_playerStats.TakeDamage(m_damage);

        m_animator.SetTrigger(StringConstants.ANT_BITE);

        AntState = eAntState.CoolDown;
    }

    protected override void Move()
    {
        m_rigidbody.velocity = new Vector2(
            (int)m_facingDirection * m_speed,
            m_rigidbody.velocity.y
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(StringConstants.END_OF_PLATFORM_TAG))
        {
            TurnAround();
        }

        if(m_antState != eAntState.CoolDown && other.gameObject.CompareTag(StringConstants.PLAYER_TAG))
        {
            // Get the distance to the player
            Vector2 d = other.transform.position - transform.position;

            if (d.x > 0 && m_facingDirection == eDirection.Right || 
                d.x < 0 && m_facingDirection == eDirection.Left)
            {
                Attack();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(StringConstants.WALL_TAG))
        {
            TurnAround();
        }
    }
}
