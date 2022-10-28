using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskAttackHitbox : MonoBehaviour
{
    [SerializeField] private Controller m_playerController;

    [SerializeField] private WarMask m_warMask;
    [SerializeField] private NatureMask m_natureMask;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag(StringConstants.PLAYER_TAG);

        m_playerController = player.GetComponent<Controller>();
        m_warMask = player.GetComponent<WarMask>();
        m_natureMask = player.GetComponent<NatureMask>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(StringConstants.ENEMY_LAYER))
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            // This assert shouldn't ever be hit, if it is, the other code will 
            // give NullReferenceExceptions anyway, so at least it will flag up where it happens! 
            BetterDebugging.Instance.Assert(enemy != null, "Anything on the Enemy Layer should be an enemy!");

            if (enemy != null)
            {
                switch (m_playerController.GetSelectedMask())
                {
                    case Controller.eMasks.War:
                        enemy.TakeDamage(m_warMask.m_specialAttackDamage);
                        m_warMask.SpecialAttackEffect(other.GetComponent<Rigidbody2D>());
                        break;
                    //case Controller.eMasks.nature:
                    //    enemy.TakeDamage(m_natureMask.m_specialAttackDamage);
                    //    m_natureMask.SpecialAttackEffect();
                    //    break;
                    default:
                        BetterDebugging.Instance.DebugLog("Mask is either not added yet, or you messed up!", BetterDebugging.eDebugLevel.Error);
                        break;
                }
            }
        }
    }
}
