using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileScreenControls : MonoBehaviour
{
#if UNITY_ANDROID || UNITY_IOS
    [SerializeField] private GameObject m_MobileScreen;
    private void Awake()
    {
        m_MobileScreen.SetActive(true);
    }
#endif
}
