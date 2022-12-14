using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    [SerializeField] private TextMesh m_text;
    private Animator m_animator;

    [TextArea][SerializeField] private string m_pcPrompt;
    [TextArea][SerializeField] private string m_mobilePrompt;

    private void Start()
    {
        m_animator = GetComponent<Animator>();

        m_text.text =
#if UNITY_STANDALONE_WIN
        m_pcPrompt;
#elif UNITY_ANDROID
        m_mobilePrompt;
#endif
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(StringConstants.PLAYER_TAG))
        {
            CancelInvoke("SetInactiveInTime");
            m_animator.Play("InteractedSign");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(StringConstants.PLAYER_TAG))
        {
            Invoke("SetInactiveInTime", 2f);
        }
    }

    private void SetInactiveInTime()
    {
        m_animator.Play("IdleSign");
    }
}
