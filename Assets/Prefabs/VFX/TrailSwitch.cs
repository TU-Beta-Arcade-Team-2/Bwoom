using UnityEngine;

public class TrailSwitch : MonoBehaviour
{
    [SerializeField] private LayerMask m_groundMask;

    private ParticleSystem m_groundTrailVFX;

    private void Start()
    {
        m_groundTrailVFX = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if ((!Physics2D.OverlapCircle(transform.position, 0.1f, m_groundMask)))
        {
            if (m_groundTrailVFX.isPlaying)
            {
                m_groundTrailVFX.Stop();
            }
        }

        else
        {
            if (m_groundTrailVFX.isStopped)
            {
                m_groundTrailVFX.Play();
            }
        }
    }
}
