using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NatureMask : MaskClass
{
    [SerializeField] private GameObject m_projectile;
    [SerializeField] private Transform m_projectileSpawner;
    [SerializeField] private float m_coolDownTime;
    private bool coolDownOver = true;
    public int m_HealAmount;

    private void OnEnable()
    {
        InitMask();
    }

    private void OnDisable()
    {
        m_playerStats.ResetPlayerStats();
    }

    public override void SpecialAttack()
    {
        if (coolDownOver)
        {
            m_playerAnimator.Play("NatureMask_Special");
            SoundManager.Instance.PlaySfx("HealShootSFX");
            Instantiate(m_projectile, m_projectileSpawner.position, transform.rotation);
            coolDownOver = false;
            Invoke("OffCoolDown", m_coolDownTime);
        }
    }

    private void OffCoolDown()
    {
        coolDownOver = true;
    }
}
