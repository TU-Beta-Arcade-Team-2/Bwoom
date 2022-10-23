using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    [SerializeField] private PlayerStats m_playerStats;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<EnemyBase>())
        {
            other.GetComponent<EnemyBase>().TakeDamage((int)(m_playerStats.m_AttackDamage));
            Debug.Log("Player Damage : " + (int)(m_playerStats.m_AttackDamage));
        }
    }
}
