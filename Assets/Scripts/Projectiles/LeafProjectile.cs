using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafProjectile : ProjectleBaseClass
{
    [SerializeField] private float m_projectileSpeed;
    [SerializeField] private float m_maxProjectileLifetime;
    [SerializeField] private NatureMask m_natureMask;
    [SerializeField] private GameObject m_vFX;
    private float m_lifeTimer;

    void Awake()
    {
        Init();

        m_lifeTimer = m_maxProjectileLifetime;
        m_shaderGUItext = Shader.Find("UI/Default Font");
        m_shaderSpritesDefault = Shader.Find("Sprites/Default");

        m_hitstopManager = GameObject.Find("Hitstop Manager").GetComponent<HitstopManager>();

        m_natureMask = m_player.gameObject.GetComponent<NatureMask>();
        gameObject.transform.parent = null;

        collisionDelegate += OnProjectileHit;
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

    private void OnProjectileHit(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(StringConstants.ENEMY_LAYER))
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();

            BetterDebugging.Assert(enemy != null, "Not colliding with the Enemy!");

            if (enemy != null)
            {
                enemy.TakeDamage(m_natureMask.m_specialAttackDamage);
                m_player.HealPlayer(m_natureMask.m_HealAmount);
                GameObject.Instantiate(m_vFX, transform.position, transform.rotation);
            }
        }
    }
}
