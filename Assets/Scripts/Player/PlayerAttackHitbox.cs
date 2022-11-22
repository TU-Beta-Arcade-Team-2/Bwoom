using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    [SerializeField] private PlayerStats m_playerStats;
    [SerializeField] private HitstopManager m_hitstopManager;
    [SerializeField] private Shader m_shaderGUItext;
    [SerializeField] private Shader m_shaderSpritesDefault;

    private float m_damage;
    private Vector3 m_enemyForce;
    private Vector3 m_playerForce;

    private void OnEnable()
    {
        m_shaderGUItext = Shader.Find("GUI/Text Shader");
        m_shaderSpritesDefault = Shader.Find("Sprites/Default");
        m_hitstopManager = GameObject.Find("Hitstop Manager").GetComponent<HitstopManager>();
    }

    public void GetDamage(float damage)
    {
        m_damage = damage;
    }

    public void SetLaunchForce(Vector3 force, Vector3 playerForce)
    {
        if (playerForce == null)
        {
            playerForce = new Vector3(0, 0, 0);
        }
        else
        {
            m_playerForce = playerForce;
        }
        m_enemyForce = force;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(StringConstants.ENEMY_LAYER))
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            SpriteRenderer enemySprite = other.GetComponent<SpriteRenderer>();
            Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
            // This assert shouldn't ever be hit, if it is, the other code will 
            // give NullReferenceExceptions anyway, so at least it will flag up where it happens! 
            BetterDebugging.Instance.Assert(enemy != null, "Anything on the Enemy Layer should be an enemy!");

            //make the enemy take damage and add force
            enemy.TakeDamage((int)(m_damage));
            enemyRb.AddForce(m_enemyForce,ForceMode2D.Impulse);
            m_playerStats.GetComponent<Rigidbody2D>().AddForce(m_playerForce, ForceMode2D.Impulse);

            //hitstop effect
            enemySprite.material.shader = m_shaderGUItext;
            enemySprite.color = Color.white;

            m_hitstopManager.HitStop();

            StartCoroutine(WaitForHitStopResume(enemySprite));

            BetterDebugging.Instance.DebugLog($"Player Damage:  {m_damage}");
        }

        IEnumerator WaitForHitStopResume(SpriteRenderer enemySprite)
        {
            while (Time.timeScale != 1.0f)
            {
                yield return null;
            }
            enemySprite.material.shader = m_shaderSpritesDefault;
            enemySprite.color = Color.white;
        }
    }
}
