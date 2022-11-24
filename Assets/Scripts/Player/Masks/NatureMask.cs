using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NatureMask : MaskClass
{
    [SerializeField] private GameObject m_projectile;
    [SerializeField] private Transform m_projectileSpawner;
    public int m_HealAmount;

    private void OnEnable()
    {
        m_maskRenderer = GameObject.Find("Mask").GetComponent<SpriteRenderer>();

        m_maskRenderer.sprite = m_maskSprite;

        m_playerController.MovementSpeed *= m_movementMultiplier;
        m_playerStats.m_AttackDamage *= m_attackMultiplier;
        m_playerStats.m_DamageResistance = m_damageResistanceMultiplier;
    }

    private void OnDisable()
    {
        m_playerController.SetDefaultValues();
    }

    public override void SpecialAttack()
    {
        m_playerAnimator.Play("NatureMask_Special");
        Instantiate(m_projectile, m_projectileSpawner.position, transform.rotation);
    }
}
