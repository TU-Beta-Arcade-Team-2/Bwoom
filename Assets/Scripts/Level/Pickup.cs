using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private int m_pointsToAward;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(StringConstants.PLAYER_TAG))
        {
            PlayerStats stats = FindObjectOfType<PlayerStats>();

            stats.AddPoints(m_pointsToAward);

            Destroy(gameObject);
        }
    }
}
