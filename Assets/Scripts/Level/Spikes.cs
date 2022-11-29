using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int m_playerDamage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(StringConstants.PLAYER_TAG))
        {
            PlayerStats stats = FindObjectOfType<PlayerStats>();
            stats.TakeDamage(m_playerDamage);

            // Tell the GameManager to respawn the player at the new location
            GameManager.Instance.RespawnAtLastCheckpoint();
        }
    }
}
