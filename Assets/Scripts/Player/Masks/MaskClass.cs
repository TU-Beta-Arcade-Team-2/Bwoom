using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MaskClass : MonoBehaviour
{
    [SerializeField] protected PlayerStats m_playerStats;
    [SerializeField] protected PlayerController m_playerController;
    [SerializeField] protected Sprite m_maskSprite;
    [SerializeField] protected Animator m_playerAnimator;
    [SerializeField] protected float m_movementMultiplier;
    [SerializeField] protected float m_jumpHeightMultiplier = 1.2f;
    [SerializeField] protected float m_attackMultiplier;
    [SerializeField] protected float m_damageResistanceMultiplier;
    public int m_specialAttackDamage;
    protected SpriteRenderer m_maskRenderer;

    public bool Unlocked { get; set; }

    public abstract void SpecialAttack();

    protected void InitMask()
    {
        m_maskRenderer = GameObject.Find("Mask").GetComponent<SpriteRenderer>();

        m_maskRenderer.sprite = m_maskSprite;


        PlayerStats.Stats maskMultiplier = new(m_movementMultiplier, m_jumpHeightMultiplier,
            m_attackMultiplier, m_damageResistanceMultiplier);

        m_playerStats.MultiplyStats(maskMultiplier);
    }
}
