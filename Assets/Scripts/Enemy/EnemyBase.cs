using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class EnemyBase : MonoBehaviour
{
#if UNITY_EDITOR 
    public GameObject DamageText;
#endif
    public enum eDirection
    {
        eLeft = -1,
        eRight = 1
    }

    public eDirection FacingDirection
    {
        get => m_facingDirection;
        set
        {
            m_facingDirection = value;
            FlipSprite();
        }
    }

    private eDirection m_facingDirection;


    // In order to damage the player, we'll give the Enemies each a reference to him
    [SerializeField] protected PlayerStats m_playerStats;

    [SerializeField] protected int m_health;
    [SerializeField] protected int m_damage;
    [SerializeField] protected float m_speed;
    [SerializeField] private GameObject m_deathFx;

    protected Animator m_animator;
    protected Rigidbody2D m_rigidbody;

    public virtual void TakeDamage(int damageAmount)
    {
        m_health = m_health -= damageAmount;

#if UNITY_EDITOR 
        GameObject text = GameObject.Instantiate(DamageText, transform.position, transform.rotation);
        text.GetComponent<TextMeshPro>().text = damageAmount.ToString();
        GameObject.Destroy(text, 0.5f);
#endif

        if (m_health <= 0)
        {
            OnDeath();
        }
    }

    private void FlipSprite()
    {
        transform.localRotation = Quaternion.Euler(0f, -180f, 0f);
    }

    // Every enemy has unique Movement, attacking, and events on death
    protected abstract void Attack();

    protected abstract void OnDeath();

    protected abstract void Move();
}
