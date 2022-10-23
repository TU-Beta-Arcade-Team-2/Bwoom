using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NatureMask : MaskClass
{
    [SerializeField] private GameObject m_projectile;
    [SerializeField] private int m_healAmount;

    private void OnEnable()
    {
        m_maskRenderer = GameObject.Find("Mask").GetComponent<SpriteRenderer>();

        m_maskRenderer.sprite = m_maskSprite;

        m_playerController.movementSpeed *= m_movementMultiplier;
        m_playerStats.m_AttackDamage *= m_attackMultiplier;
        m_playerStats.m_DamageResistance = m_damageResistanceMultiplier;
    }

    private void OnDisable()
    {
        m_playerController.SetDefaultValues();
    }

    public override void SpecialAttack()
    {
        Instantiate(m_projectile,transform.position,transform.rotation);
    }

    public void SpecialAttackEffect()
    {
        m_playerStats.TakeHEAL(m_healAmount);
    }
}
