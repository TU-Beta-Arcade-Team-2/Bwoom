using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitstopManager : MonoBehaviour
{
    private bool m_isWaiting;

    private float m_duration;

    public void SetHitstopDuration(float duration)
    {
        m_duration = duration;
    }

    public void HitStop()
    {
        if (m_isWaiting)
        {
            return;
        }
        Time.timeScale = 0;
        StartCoroutine(Wait(m_duration));
    }

    IEnumerator Wait(float duration)
    {
        m_isWaiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        m_isWaiting = false;
    }
}
