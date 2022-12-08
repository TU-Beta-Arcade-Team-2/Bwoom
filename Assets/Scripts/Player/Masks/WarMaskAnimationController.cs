using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarMaskAnimationController : MonoBehaviour
{
    [SerializeField] private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void TurnOffSpecialParam()
    {
        m_animator.SetBool("Special", false);
    }
}
