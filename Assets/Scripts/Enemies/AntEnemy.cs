using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AntEnemy : EnemyBase
{
    // Start is called before the first frame update
    void Start()
    {
        Init("AntEnemy");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    protected override void Attack()
    {
        // From the brief, the ant doesn't attack, it just idly walks around
        // This could change though
    }

    protected override void Move()
    {
        m_rigidbody.velocity = new Vector2(
            (int)m_facingDirection * m_speed,
            0f
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(StringConstants.END_OF_PLATFORM_TAG))
        {
            TurnAround();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(StringConstants.WALL_TAG))
        {
            TurnAround();
        }
    }
}
