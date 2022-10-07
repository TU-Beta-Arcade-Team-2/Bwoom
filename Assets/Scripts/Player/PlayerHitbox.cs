using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public int Damage { get; set; }
    public ushort DamageType { get; set; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyStats>().TakeDMG(Damage);
        }
    }
}
