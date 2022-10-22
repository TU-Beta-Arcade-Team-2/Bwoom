using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskAttackHitbox : MonoBehaviour
{
    [SerializeField] private Controller m_playerController;

    [SerializeField] private WarMask m_warMask;
    [SerializeField] private NatureMask m_natureMask;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(StringConstants.ENEMY_TAG))
        {
            switch (m_playerController.GetSelectedMask())
            {
                case Controller.eMasks.war:
                    other.GetComponent<EnemyBase>().TakeDamage(m_warMask.m_specialAttackDamage);
                    break;
                case Controller.eMasks.nature:
                    other.GetComponent<EnemyBase>().TakeDamage(m_natureMask.m_specialAttackDamage);
                    break;
            }
        }
    }
}
