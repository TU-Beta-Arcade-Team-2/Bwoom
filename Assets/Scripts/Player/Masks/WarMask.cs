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
    [SerializeField] private GameObject uppercutVFX;

    [SerializeField] private float m_coolDownTime;
    private bool m_coolDownOver = true;

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
        if (!m_IsJumped && m_coolDownOver)
        {
            m_IsJumped = true;
            m_coolDownOver = false;

            Invoke("OffCoolDown", m_coolDownTime);

            SoundManager.Instance.PlaySfx("UppercutSFX");

            GameObject vfx = Instantiate(uppercutVFX, transform.position, transform.rotation);
            vfx.transform.parent = this.transform;

            m_rb.velocity = new Vector2(m_rb.velocity.x, 0);
            m_rb.AddForce(new Vector2(0, m_uppercutJumpHeight), ForceMode2D.Impulse);
            m_playerAnimator.Play("Warmask Special");
            m_playerAnimator.SetBool("Special", true);
        }
    }

    public void SpecialAttackEffect(Rigidbody2D target)
    {
        target.AddForce(new Vector2(0,m_launchForce), ForceMode2D.Impulse);
    }

    private void OffCoolDown()
    {
        m_coolDownOver = true;
    }
}
