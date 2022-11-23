using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NatureMask : MaskClass
{
    [SerializeField] private GameObject m_projectile;
    [SerializeField] private Transform m_projectileSpawner;
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
        Instantiate(m_projectile, m_projectileSpawner.position, transform.rotation);
    }
}
