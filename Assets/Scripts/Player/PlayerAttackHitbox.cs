using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    [SerializeField] private PlayerStats m_playerStats;

    private float m_damage;

    public void GetDamage(float damage)
    {
        m_damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(StringConstants.ENEMY_LAYER))
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            // This assert shouldn't ever be hit, if it is, the other code will 
            // give NullReferenceExceptions anyway, so at least it will flag up where it happens! 
            BetterDebugging.Instance.Assert(enemy != null, "Anything on the Enemy Layer should be an enemy!");

            enemy.TakeDamage((int)(m_playerStats.m_AttackDamage));
            BetterDebugging.Instance.DebugLog($"Player Damage:  {m_playerStats.m_AttackDamage}");
        }
    }
}
