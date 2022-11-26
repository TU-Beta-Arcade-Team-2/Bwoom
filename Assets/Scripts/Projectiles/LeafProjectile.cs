using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafProjectile : ProjectleBaseClass
{
    [SerializeField] private float m_projectileSpeed;
    [SerializeField] private float m_maxProjectileLifetime;
    [SerializeField] private PlayerStats m_playerStats;
    [SerializeField] private NatureMask m_natureMask;
    private float m_lifeTimer;

    void Awake()
    {
        m_lifeTimer = m_maxProjectileLifetime;

        GameObject player = GameObject.FindGameObjectWithTag(StringConstants.PLAYER_TAG);

        m_shaderGUItext = Shader.Find("UI/Default Font");
        m_shaderSpritesDefault = Shader.Find("Sprites/Default");

        m_hitstopManager = GameObject.Find("Hitstop Manager").GetComponent<HitstopManager>();

        m_playerStats = player.GetComponentInParent<PlayerStats>();
        m_natureMask = player.GetComponentInParent<NatureMask>();
        gameObject.transform.parent = null;

        collisionDelegate += OnProjectileHit;

        Debug.Log("Delegate Name : " + collisionDelegate);
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

            BetterDebugging.Instance.Assert(enemy != null, "Not colliding with the Enemy!");

            if (enemy != null)
            {
                enemy.TakeDamage(m_natureMask.m_specialAttackDamage);
                m_playerStats.HealPlayer(m_natureMask.m_HealAmount);
            }
        }
    }
}
