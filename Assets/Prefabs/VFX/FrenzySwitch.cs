using UnityEngine;

public class FrenzySwitch : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_particleSystem1;
    [SerializeField] private ParticleSystem m_particleSystem2;
    [SerializeField] private ParticleSystem m_particleSystem3;
    public void FrenzySwitcher(bool frenzyOn)
    {
        if (frenzyOn)
        {
            if (m_particleSystem1.isStopped)
            {
                m_particleSystem1.Play();
                m_particleSystem2.Play();
                m_particleSystem3.Play();
            }

            return;
        }

        if (m_particleSystem1.isPlaying)
        {
            m_particleSystem1.Stop();
            m_particleSystem2.Stop();
            m_particleSystem3.Stop();
        }
    }           
}
