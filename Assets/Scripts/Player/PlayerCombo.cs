using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombo : MonoBehaviour
{
    /// <summary> Player References </summary>
    private PlayerInput m_playerInput;
    private PlayerStats m_playerStats;
    private Rigidbody2D m_playerRigidbody;

    [SerializeField] private float m_attackDelay;
    [SerializeField] private Animator m_playerAnimator;
    [SerializeField] private PlayerAttackHitbox m_playerAttackHitbox;
    private float m_attackTimer;

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

        Debug.Log("Combo State : " + m_comboState);
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
                    ComboAttack1();
                }
                break;
            case eComboState.attack1:
                if (m_playerInput.actions["Attack"].triggered && m_attackTimer > 0)
                {
                    m_comboState = eComboState.attack2;
                    m_attackTimer = m_attackDelay;
                    ComboAttack2();
                }
                break;
            case eComboState.attack2:
                if (m_playerInput.actions["Attack"].triggered && m_attackTimer > 0)
                {
                    m_comboState = eComboState.attack3;
                    m_attackTimer = m_attackDelay;
                    ComboAttack3();
                }
                break;
        }

        m_attackTimer -= Time.deltaTime;

        if (m_attackTimer <= 0)
        {
            m_comboState = eComboState.none;
        }
    }

    private void ComboAttack1()
    {
        m_playerAttackHitbox.GetDamage(m_playerStats.m_ComboAttackDamage1);
        m_playerAnimator.SetTrigger(StringConstants.COMBO_ATTACK_ONE);
        m_playerAttackHitbox.SetLaunchForce(m_playerStats.m_ComboAttackLaunchDistance1);
    }

    private void ComboAttack2()
    {
        m_playerAttackHitbox.GetDamage(m_playerStats.m_ComboAttackDamage2);
        m_playerAnimator.SetTrigger(StringConstants.COMBO_ATTACK_TWO);
        m_playerRigidbody.AddForce(m_playerStats.m_ComboAttackLaunchDistance2);
    }

    private void ComboAttack3()
    {
        m_playerAttackHitbox.GetDamage(m_playerStats.m_ComboAttackDamage3);
        m_playerAnimator.SetTrigger(StringConstants.COMBO_ATTACK_THREE);
        m_playerAttackHitbox.SetLaunchForce(m_playerStats.m_ComboAttackLaunchDistance3);
    }

}
