#if UNITY_ANDROID || UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileScreenControls : MonoBehaviour
{
    [SerializeField] private GameObject m_MobileScreen;
    private void Awake()
    {
        m_MobileScreen.SetActive(true);
    }
}
#endif