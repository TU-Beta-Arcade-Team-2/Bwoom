using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectleBaseClass : MonoBehaviour
{
    protected int m_damage;
    protected int m_speed;

    [SerializeField] protected float m_hitstopDuration;

    [SerializeField] protected Shader m_shaderGUItext;
    [SerializeField] protected Shader m_shaderSpritesDefault;

    [SerializeField] protected HitstopManager m_hitstopManager;

    public delegate void OnCollision(Collider2D other);
    [SerializeField] protected OnCollision collisionDelegate;

    protected PlayerStats m_player;

    protected void Init()
    {
        m_player = FindObjectOfType<PlayerStats>();

        BetterDebugging.Assert(m_player != null, "PLAYER SHOULDN'T BE NULL!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collisionDelegate != null)
        {
            collisionDelegate(other);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer(StringConstants.ENEMY_LAYER))
        {
            //hitstop effect

            SpriteRenderer targetSprite = other.GetComponent<SpriteRenderer>();

            targetSprite.material.shader = m_shaderGUItext;
            targetSprite.color = Color.white;

            Debug.Log("SPRITE : " + targetSprite.material.shader);

            m_hitstopManager.SetHitstopDuration(m_hitstopDuration);
            m_hitstopManager.HitStop();

            StartCoroutine(WaitForHitStopResume(targetSprite));
        }
    }


    IEnumerator WaitForHitStopResume(SpriteRenderer targetSprite)
    {
        while (Time.timeScale != 1.0f)
        {
            yield return null;
        }
        targetSprite.material.shader = m_shaderSpritesDefault;
        Debug.Log("SPRITE : " + targetSprite.material.shader);
        targetSprite.color = Color.white;

        Destroy(this.gameObject);
    }
}
