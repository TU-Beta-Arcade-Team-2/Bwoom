using UnityEngine;

public class ParticleDieOnFinish : MonoBehaviour
{
    private ParticleSystem m_particleSystem;

    private void Start()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(m_particleSystem != null && !m_particleSystem.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
