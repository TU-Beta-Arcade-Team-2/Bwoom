using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectleBaseClass : MonoBehaviour
{
    protected int m_damage;
    protected int m_speed;
    protected string m_targetTag;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(m_targetTag))
        {
            //TODO : Damage target
            Destroy(this.gameObject);
        }
    }

    protected virtual void SecondaryEffect()
    {

    }

}
