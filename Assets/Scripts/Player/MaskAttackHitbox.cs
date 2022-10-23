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
        if (other.GetComponent<EnemyBase>() != null)
        {
            switch (m_playerController.GetSelectedMask())
            {
                case Controller.eMasks.war:
                    other.GetComponent<EnemyBase>().TakeDamage(m_warMask.m_specialAttackDamage);
                    m_warMask.SpecialAttackEffect(other.GetComponent<Rigidbody2D>());
                    break;
                case Controller.eMasks.nature:
                    other.GetComponent<EnemyBase>().TakeDamage(m_natureMask.m_specialAttackDamage);
                    m_natureMask.SpecialAttackEffect();
                    break;
            }
        }
    }
}
