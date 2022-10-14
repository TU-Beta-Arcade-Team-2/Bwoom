using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class EnemyBase : MonoBehaviour
{
#if UNITY_EDITOR 
    public TextMeshPro DebugText;
#endif
    public enum eDirection
    {
        eLeft = -1,
        eRight = 1
    }

    protected eDirection m_facingDirection;

    private SpriteRenderer m_spriteRenderer;

    // In order to damage the player, we'll give the Enemies each a reference to him
    [SerializeField] protected PlayerStats m_playerStats;

    [SerializeField] protected int m_health;
    [SerializeField] protected int m_damage;
    [SerializeField] protected float m_speed;
    [SerializeField] private GameObject m_deathFx;

    protected Animator m_animator;
    protected Rigidbody2D m_rigidbody;

    // TO BE CALLED ON START OR AWAKE OF THE CHILD CLASSES
    protected void Init()
    {
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    public virtual void TakeDamage(int damageAmount)
    {
        m_health = m_health -= damageAmount;

#if UNITY_EDITOR 
        ShowDebugText($"DAMAGE: {damageAmount}", true);
#endif

        if (m_health <= 0)
        {
            OnDeath();
        }
    }

    protected void TurnAround()
    {
        m_spriteRenderer.flipX = !m_spriteRenderer.flipX;
        m_facingDirection = m_facingDirection == eDirection.eLeft ? eDirection.eRight : eDirection.eLeft;
    }

    // Every enemy has unique Movement, attacking, and events on death
    protected abstract void Attack();

    protected abstract void OnDeath();

    protected abstract void Move();

#if UNITY_EDITOR
    public void ShowDebugText(string debugString, bool hideAfterTime)
    {
        DebugText.text = debugString;
        DebugText.gameObject.SetActive(true);

        if (hideAfterTime)
        {
            StartCoroutine("HideDebugTextAfterTime", 3f);
        }
    }

    public IEnumerator HideDebugTextAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        DebugText.gameObject.SetActive(false);
    }
#endif
}
