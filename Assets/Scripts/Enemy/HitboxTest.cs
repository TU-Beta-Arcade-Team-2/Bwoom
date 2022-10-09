using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxTest : MonoBehaviour
{
    private PlayerStats playerStats;

    [SerializeField] private int damage;

    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerStats.TakeDMG(damage);
        }
    }
}
