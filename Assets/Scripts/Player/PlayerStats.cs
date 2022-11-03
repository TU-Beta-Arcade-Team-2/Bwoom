using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    private LevelManager m_lvlManager;
    private PlayerInput m_playerInput;
    private Controller m_controller;
    private Animator m_cameraAnim;

    private enum eMasks
    {
        warMask,
        natureMask,
        seaMask,
        energyMask
    }

    [Header("Health Variables")]
    [SerializeField] private int m_playerHealth;
    [SerializeField] private int m_maxPlayerHealth;
    [Space(10)]

    [Header("Game HUD Variables")]
    [SerializeField] private Image m_radialHealthBar;
    public Image MaskIconImage;
    public Sprite WarMaskIcon;
    public Sprite NatureMaskIcon;
    [Space(10)]

    [Header ("Default Values")]
    public float m_DefaultMovementSpeed;
    public float m_DefaultJumpHeight;
    public float m_DefaultAttackDamage;
    public float m_CurrentMovementSpeed;
    public float m_CurrentJumpHeight;
    public float m_AttackDamage;
    public float m_DamageResistance;
    [Space(10)]

    [Header ("Frenzy Mode Values")]
    private bool m_frenzyMode;
    [SerializeField] private float m_frenzyModeDefaultTimer;
    [SerializeField] private float m_frenzyTimerIncrement;
    private float m_frenzyTimer;

    [SerializeField] private float m_frenzySpeedMultiplier;
    [SerializeField] private float m_frenzyAttackMultiplier;
    [SerializeField] private float m_frenzyJumpMultiplier;
    [SerializeField] private float m_frenzyAttackSpeedMultiplier;

    #region Main Functions
    private void Start()
    {
        m_playerHealth = Mathf.Clamp(m_playerHealth, 0, m_maxPlayerHealth);
        m_radialHealthBar.fillAmount = (float)m_playerHealth / (float)m_maxPlayerHealth;
        MaskIconImage.sprite = WarMaskIcon;
        m_DamageResistance = 1;

        m_lvlManager = FindObjectOfType<LevelManager>();
        m_cameraAnim = Camera.main.gameObject.GetComponent<Animator>();
        m_controller = GetComponent<Controller>();
        m_playerInput = GetComponent<PlayerInput>();
        m_frenzyTimer = m_frenzyModeDefaultTimer;

        DeactivateFrenzyMode();
    }
    #endregion

    #region Health Functions
    public void TakeDMG(int incomingDMG)
    {
        int actualDamage = (int)(incomingDMG / m_DamageResistance);

        Debug.Log("Actual Damage : " + actualDamage);

        m_playerHealth -= actualDamage;
        m_radialHealthBar.fillAmount = (float)m_playerHealth / (float)m_maxPlayerHealth;

        m_cameraAnim.SetTrigger("LightShake");

        if (m_playerHealth <= 0)
        {
            m_lvlManager.Death();
            Destroy(gameObject);
            return;
        }

        //Play hurt animation
    }

    public void TakeHEAL(int incomingHEAL)
    {
        Debug.Log("HEAL TEST");

        m_playerHealth = Mathf.Clamp(m_playerHealth + incomingHEAL, 0, m_maxPlayerHealth);
        m_radialHealthBar.fillAmount = (float)m_playerHealth / (float)m_maxPlayerHealth;

        //Play heal animation
    }
    #endregion

    #region Frenzy Functions
    public void ActivateFrenzyMode()
    {
        if (m_frenzyMode)
        {
            m_frenzyTimer += m_frenzyTimerIncrement;
            CancelInvoke("DeactivateFrenzyMode");
            Invoke("DeactivateFrenzyMode", m_frenzyTimer);
            Debug.Log("Frenzy Mode Replenished");

            return;
        }

        m_CurrentMovementSpeed *= m_frenzySpeedMultiplier;
        m_CurrentJumpHeight *= m_frenzyJumpMultiplier;
        m_AttackDamage *= m_frenzyAttackMultiplier;
        m_controller.doubleJumpOn = true;
        m_controller.SetDefaultValues();

        Invoke("DeactivateFrenzyMode", m_frenzyTimer);
        Debug.Log("Frenzy Mode On!");
        m_frenzyMode = true;
    }

    private void DeactivateFrenzyMode()
    {
        m_frenzyTimer = m_frenzyModeDefaultTimer;
        m_CurrentMovementSpeed = m_DefaultMovementSpeed;
        m_CurrentJumpHeight = m_DefaultJumpHeight;
        m_AttackDamage = m_DefaultAttackDamage;
        m_controller.doubleJumpOn = false;
        m_controller.SetDefaultValues();

        Debug.Log("No More Frenzy");
        m_frenzyMode = false;
    }

    #endregion
}
