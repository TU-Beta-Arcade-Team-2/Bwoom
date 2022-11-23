using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WarMask : MaskClass
{
    [SerializeField] private float m_uppercutJumpHeight;
    public bool m_IsJumped;
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private Animator m_attackAnim;
    [SerializeField] private float m_launchForce;

    private void Start()
    {
        m_IsJumped = false;
    }

    private void OnEnable()
    {
        InitMask();
    }

    private void OnDisable()
    {
        m_playerStats.ResetPlayerStats();
    }

    //Warmask special attack, an uppercut that sends the player and enemies up in the air
    public override void SpecialAttack()
    {
        if (!m_IsJumped)
        {
            m_IsJumped = true;

            m_rb.velocity = new Vector2(m_rb.velocity.x, 0);
            m_rb.AddForce(new Vector2(0, m_uppercutJumpHeight), ForceMode2D.Impulse);
            m_attackAnim.Play("Warmask Special");
        }
    }

    public void SpecialAttackEffect(Rigidbody2D target)
    {
        target.AddForce(new Vector2(0,m_launchForce), ForceMode2D.Impulse);
    }
}
