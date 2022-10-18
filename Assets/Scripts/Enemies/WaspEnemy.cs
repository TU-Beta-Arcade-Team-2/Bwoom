using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspEnemy : EnemyBase
{
    [SerializeField] private float m_bobHeight;
    [SerializeField] private float m_bobSpeed;
    [SerializeField] private bool m_limitedToPlatform;

    private float m_sinCounter;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        m_facingDirection = eDirection.eRight;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    protected override void Attack()
    {

    }

    protected override void Move()
    {
        // Make the wasp float in the air with a sine curve
        m_sinCounter += Time.deltaTime * m_bobSpeed;
        if(m_sinCounter > 180f)
        {
            m_sinCounter = -180f;
        }

        m_rigidbody.velocity = new Vector2(
            (int)m_facingDirection * m_speed, 
            Mathf.Sin(m_sinCounter) * m_speed * m_bobHeight
    
        );
    }

    protected override void OnDeath()
    {

    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("Wall"))
        {
            TurnAround();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(m_limitedToPlatform)
        {
            if (other.gameObject.CompareTag("End_Of_Platform"))
            {
                TurnAround();
            }
        }
    }
}
