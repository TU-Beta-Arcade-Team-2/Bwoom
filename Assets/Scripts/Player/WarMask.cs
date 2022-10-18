using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WarMask : MaskClass
{
    private float m_jumpHeight = 10;
    public bool isJumped;
    private float m_ungroundedTimer;
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private Controller m_playerController;
    [SerializeField] private Animator attackAnim;

    private void Start()
    {
        isJumped = false;
    }

    private void OnEnable()
    {
        m_maskRenderer = GameObject.Find("Mask").GetComponent<SpriteRenderer>();

        m_maskRenderer.sprite = m_maskSprite;
    }
    //Warmask special attack, an uppercut that sends the player and enemies up in the air
    public override void SpecialAttack()
    {
        if (!m_playerController.JumpAvaliable() && !isJumped)
        {
            m_rb.velocity = new Vector2(m_rb.velocity.x, 0);
            m_rb.AddForce(new Vector2(0, m_jumpHeight), ForceMode2D.Impulse);

            isJumped = true;
            attackAnim.Play("Warmask Special");
        }
    }
}
