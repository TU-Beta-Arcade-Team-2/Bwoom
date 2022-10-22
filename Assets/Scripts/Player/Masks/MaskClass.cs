using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MaskClass : MonoBehaviour
{
    [SerializeField] protected PlayerStats m_playerStats;
    [SerializeField] protected Controller m_playerController;
    [SerializeField] protected Sprite m_maskSprite;
    [SerializeField] protected float m_movementMultiplier;
    [SerializeField] protected float m_attackMultiplier;
    [SerializeField] protected float m_damageResistanceMultiplier;
    public int m_specialAttackDamage;
    protected SpriteRenderer m_maskRenderer;
    private bool m_unlocked;

    public bool Unlocked
    {
        get { return m_unlocked; }
        set { m_unlocked = value; }
    }

    public abstract void SpecialAttack();
}
