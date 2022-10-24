using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectleBaseClass : MonoBehaviour
{
    protected int m_damage;
    protected int m_speed;


    public delegate void OnCollision(Collider2D other);
    [SerializeField] protected OnCollision collisionDelegate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collisionDelegate != null)
        {
            collisionDelegate(collision);
        }

        Debug.Log("Delegate Name : " + collisionDelegate);

        Destroy(this.gameObject);
    }
}
