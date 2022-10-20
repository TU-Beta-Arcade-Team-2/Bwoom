using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafProjectile : ProjectleBaseClass
{
    [SerializeField] private float m_projectileSpeed;
    [SerializeField] private float m_maxProjectileLifetime;
    private float m_lifeTimer;

    void Awake()
    {
        m_lifeTimer = m_maxProjectileLifetime;
        gameObject.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * m_projectileSpeed;

        m_lifeTimer -= Time.deltaTime;
        if (m_lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
