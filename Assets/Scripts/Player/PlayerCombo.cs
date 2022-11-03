using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombo : MonoBehaviour
{
    /// <summary> Player References </summary>
    private PlayerInput m_playerInput;
    private PlayerStats m_playerStats;

    [SerializeField] private float m_attackDelay;
    [SerializeField] private float m_attackTimer;

    private enum eComboState
    {
        none,
        attack1,
        attack2,
        attack3
    }

    private eComboState m_comboState;

    // Start is called before the first frame update
    void Start()
    {
        m_playerInput = GetComponent<PlayerInput>();
        m_playerStats = GetComponent<PlayerStats>();
        m_comboState = eComboState.none;
        m_attackTimer = m_attackDelay;
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }
    private void Attack()
    {
        switch(m_comboState)
        {
            case eComboState.none:
                if (m_playerInput.actions["Attack"].triggered)
                {
                    m_comboState = eComboState.attack1;
                    m_attackTimer = m_attackDelay;
                }
                break;
            case eComboState.attack1:
                if (m_playerInput.actions["Attack"].triggered && m_attackTimer > 0)
                {
                    m_comboState = eComboState.attack2;
                    m_attackTimer = m_attackDelay;
                }
                break;
            case eComboState.attack2:
                if (m_playerInput.actions["Attack"].triggered && m_attackTimer > 0)
                {
                    m_comboState = eComboState.attack3;
                    m_attackTimer = m_attackDelay;
                }
                break;
        }

        m_attackTimer -= Time.deltaTime;

        if (m_attackTimer <= 0)
        {
            m_comboState = eComboState.none;
        }
    }

}
