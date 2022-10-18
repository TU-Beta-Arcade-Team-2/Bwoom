using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class EnemyBase : MonoBehaviour
{
    public enum eDirection
    {
        eLeft = -1,
        eRight = 1
    }

    protected eDirection m_facingDirection;

    private SpriteRenderer m_spriteRenderer;

    // In order to damage the player, we'll give the Enemies each a reference to him
    [SerializeField] protected PlayerStats m_playerStats;

    private string m_name;
    [SerializeField] protected int m_health;
    [SerializeField] protected int m_damage;
    [SerializeField] protected float m_speed;
    [SerializeField] private GameObject m_deathFx;

    protected Animator m_animator;
    protected Rigidbody2D m_rigidbody;

    // TO BE CALLED ON START OR AWAKE OF THE CHILD CLASSES
    protected void Init(string enemyName)
    {
        m_name = enemyName;
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    public virtual void TakeDamage(int damageAmount)
    {
        m_health = m_health -= damageAmount;

        BetterDebugging.Instance.SpawnDebugText(
            $"{m_name.ToUpper()} TAKING {damageAmount} DAMAGE", 
            transform.position + new Vector3(0, 2), 
            0.3f, 
            null,
            BetterDebugging.eDebugLevel.Message
        );

        if (m_health <= 0)
        {
            OnDeath();
        }
    }

    protected void TurnAround()
    {
        m_spriteRenderer.flipX = !m_spriteRenderer.flipX;

        if (m_facingDirection == eDirection.eLeft)
        {
            m_facingDirection = eDirection.eRight;
        }
        else
        {
            m_facingDirection = eDirection.eLeft;
        }
    }

    // Every enemy has unique Movement, attacking, and events on death
    protected abstract void Attack();

    protected abstract void OnDeath();

    protected abstract void Move();

    // Return a vector pointing to the player
    protected Vector2 GetVectorToPlayer()
    {
        return new Vector2(
            m_playerStats.gameObject.transform.position.x - transform.position.x,
            m_playerStats.gameObject.transform.position.y - transform.position.y
        );
    }

    // Return the SqrMag distance of the player, 
    protected float FindSqrDistanceToPlayer()
    {
        Vector2 aToB = GetVectorToPlayer();

        return aToB.x * aToB.x + aToB.y * aToB.y;
    }

    public void DebugLog(string debugMessage, BetterDebugging.eDebugLevel level = BetterDebugging.eDebugLevel.Log)
    {
        BetterDebugging.Instance.DebugLog($"{m_name.ToUpper()}:  {debugMessage}", level);
    }
}
