using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyMask : MaskClass
{
    [SerializeField] private PlayerController m_playerPlayerController;

    private void OnEnable()
    {
        m_maskRenderer = GameObject.Find("Mask").GetComponent<SpriteRenderer>();

        m_maskRenderer.sprite = m_maskSprite;
    }

    public override void SpecialAttack()
    {

    }
}
