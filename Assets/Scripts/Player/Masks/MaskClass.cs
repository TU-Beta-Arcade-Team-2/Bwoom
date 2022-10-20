using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class MaskClass : MonoBehaviour
{
    [SerializeField] protected Sprite m_maskSprite;
    protected SpriteRenderer m_maskRenderer;
    [SerializeField] protected float m_movementMultiplier;

    [SerializeField]
    private bool m_unlocked;

    public bool Unlocked
    {
        get { return m_unlocked; }
        set { m_unlocked = value; }
    }

    public abstract void SpecialAttack();
}
