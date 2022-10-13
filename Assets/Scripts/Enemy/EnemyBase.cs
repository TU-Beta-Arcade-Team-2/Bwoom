using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class EnemyBase : MonoBehaviour
{
#if DEBUG
    public GameObject DamageText;
#endif

    [SerializeField] private int m_health;
    [SerializeField] private float m_damage;
    [SerializeField] private GameObject m_deathFX;
    private Animator m_animator;

    public void TakeDamage(int damageAmount)
    {
        m_health = m_health -= damageAmount;

#if DEBUG
        GameObject text = GameObject.Instantiate(DamageText, transform.position, transform.rotation);
        text.GetComponent<TextMeshPro>().text = damageAmount.ToString();
        GameObject.Destroy(text, 0.5f);
#endif

        if (m_health <= 0)
        {
            OnDeath();
        }
    }

    public abstract void Attack();

    public abstract void OnDeath();

    protected abstract void Move();
}
