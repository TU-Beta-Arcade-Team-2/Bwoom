using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WarMask : MaskClass
{
    private float m_jumpHeight = 10;
    [SerializeField] private Rigidbody2D m_rb;

    private void OnEnable()
    {
        m_maskRenderer = GameObject.Find("Mask").GetComponent<SpriteRenderer>();

        m_maskRenderer.sprite = m_maskSprite;
    }
    //Warmask special attack, an uppercut that sends the player and enemies up in the air
    public override void SpecialAttack()
    {
        m_rb.AddForce(new Vector2(0, m_jumpHeight), ForceMode2D.Impulse);
    }
}
