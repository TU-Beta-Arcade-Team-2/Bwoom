using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    [SerializeField] private PlayerStats m_playerStats;
    [SerializeField] private WarMask m_warMask;
    [SerializeField] private NatureMask m_natureMask;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringConstants.ENEMY_TAG))
        {
            other.GetComponent<EnemyBase>().TakeDamage((int)m_playerStats.m_DefaultAttackDamage);
        }
    }
}
