using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    [SerializeField] private GameObject m_text;

    private void Start()
    {
        m_text.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(StringConstants.PLAYER_TAG))
        {
            m_text.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(StringConstants.PLAYER_TAG))
        {
            m_text.SetActive(false);
        }
    }
}
