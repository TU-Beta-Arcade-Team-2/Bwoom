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
        switch (m_playerController.GetSelectedMask())
        {
            case Controller.eMasks.war:
                other.GetComponent<EnemyBase>().TakeDamage(m_warMask.m_specialAttackDamage);
                m_warMask.SpecialAttackEffect(other.GetComponent<Rigidbody2D>());
                break;
            case Controller.eMasks.nature:
                other.GetComponent<EnemyBase>().TakeDamage(m_natureMask.m_specialAttackDamage);
                break;
        }
    }
}
