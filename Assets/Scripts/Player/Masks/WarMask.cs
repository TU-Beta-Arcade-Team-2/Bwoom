using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WarMask : MaskClass
{
    private float m_jumpHeight = 10;
    public bool m_IsJumped;
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private Controller m_playerController;
    [SerializeField] private PlayerStats m_playerStats;
    [SerializeField] private Animator m_attackAnim;

    private void Start()
    {
        m_IsJumped = false;
    }

    private void OnEnable()
    {
        m_maskRenderer = GameObject.Find("Mask").GetComponent<SpriteRenderer>();

        m_maskRenderer.sprite = m_maskSprite;

        m_playerController.movementSpeed *= m_movementMultiplier;
    }

    private void OnDisable()
    {
        m_playerController.SetDefaultValues();
    }

    //Warmask special attack, an uppercut that sends the player and enemies up in the air
    public override void SpecialAttack()
    {
        if (!m_IsJumped)
        {
            m_IsJumped = true;

            m_rb.velocity = new Vector2(m_rb.velocity.x, 0);
            m_rb.AddForce(new Vector2(0, m_jumpHeight), ForceMode2D.Impulse);
            m_attackAnim.Play("Warmask Special");
        }
    }
}
