using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using UnityEngine;
using UnityEditor;

public abstract class EnemyBase : MonoBehaviour
{
    public enum eDirection
    {
        Left = -1,
        Right = 1
    }

    protected eDirection m_facingDirection = eDirection.Right;

    private SpriteRenderer m_spriteRenderer;

    // In order to damage the player, we'll give the Enemies each a reference to him
    [SerializeField] protected PlayerStats m_playerStats;

    private string m_name;
    [SerializeField] protected int m_health;
    [SerializeField] protected int m_damage;
    [SerializeField] protected int m_pointsToAward;
    [SerializeField] protected string m_deathSoundFxName; // This is probably going to change in the future, I'm just assuming we'll be using some sort of Dictionary to store the SFX with a string name being the key!
    [SerializeField] protected float m_speed;
    [SerializeField] protected GameObject m_deathParticleFx;
    [SerializeField] protected GameObject m_deathDropItem;

    protected Animator m_animator;
    protected Rigidbody2D m_rigidbody;

    // TO BE CALLED ON START OR AWAKE OF THE CHILD CLASSES
    public void Init(string enemyName)
    {
        m_name = enemyName;

        // Double check all the components have been set up!
        if (m_playerStats == null)
        {
            DebugLog("PLAYER REFERENCE NOT SET UP!", BetterDebugging.eDebugLevel.Error);
        }

        m_animator = GetComponent<Animator>();
        if (m_animator == null)
        {
            DebugLog("NO ANIMATOR COMPONENT ON THE GAMEOBJECT", BetterDebugging.eDebugLevel.Error);
        }

        m_spriteRenderer = GetComponent<SpriteRenderer>();
        if (m_spriteRenderer == null)
        {
            DebugLog("NO SPRITE RENDERER COMPONENT ON THE GAMEOBJECT", BetterDebugging.eDebugLevel.Error);
        }

        m_rigidbody = GetComponent<Rigidbody2D>();
        if (m_rigidbody == null)
        {
            DebugLog("NO RIGID BODY 2D COMPONENT ON THE GAMEOBJECT", BetterDebugging.eDebugLevel.Error);
        }

        m_spriteRenderer.flipX = m_facingDirection == eDirection.Left;
    }

    public virtual void TakeDamage(int damageAmount)
    {
        m_health = m_health -= damageAmount;

        BetterDebugging.SpawnDebugText(
            $"{m_name.ToUpper()} TAKING {damageAmount} DAMAGE",
            transform.position + new Vector3(0, 2),
            0.3f,
            null,
            BetterDebugging.eDebugLevel.Message
        );

        if (m_health <= 0)
        {
            OnDeath(m_deathSoundFxName, m_pointsToAward, m_deathParticleFx, m_deathDropItem);
        }
    }

    public void SetPlayer(PlayerStats player)
    {
        m_playerStats = player;
    }

    public virtual void DamagePlayer(int damageAmount)
    {
        BetterDebugging.SpawnDebugText(
            $"{m_name.ToUpper()} DEALING {damageAmount} DAMAGE",
            transform.position + new Vector3(0, 2),
            0.3f,
            null,
            BetterDebugging.eDebugLevel.Message
        );

        m_playerStats.TakeDamage(damageAmount);
    }

    [ExecuteInEditMode]
    public void TurnAround()
    {
        m_spriteRenderer.flipX = !m_spriteRenderer.flipX;

        if (m_facingDirection == eDirection.Left)
        {
            m_facingDirection = eDirection.Right;
        }
        else
        {
            m_facingDirection = eDirection.Left;
        }
    }

    // Every enemy has unique Movement, attacking, and events on death
    protected abstract void Attack();

    protected void OnDeath(string soundFxName, int pointsToAward, GameObject deathParticleFx,
        [CanBeNull] GameObject itemToDrop = null)
    {
        m_playerStats.ActivateFrenzyMode();

        m_playerStats.AddPoints(pointsToAward);
        Destroy(gameObject);
        Instantiate(deathParticleFx);
    }

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
        BetterDebugging.Log($"{m_name.ToUpper()}:  {debugMessage}", level);
    }
}
