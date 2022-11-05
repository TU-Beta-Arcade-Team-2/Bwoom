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
    [Range(0, 8)]
    [SerializeField] private int m_playerHealth;
    [Range(1, 8)]
    [SerializeField] private int m_maxPlayerHealth;

    [Header ("Default Values")]
    public float m_DefaultMovementSpeed;
    public float m_DefaultJumpHeight;
    public float m_DefaultAttackDamage;
    public float m_CurrentMovementSpeed;
    public float m_CurrentJumpHeight;
    public float m_AttackDamage;
    public float m_DamageResistance;

    [SerializeField] private Image[] m_lives;
    [SerializeField] private Sprite m_fullMaskSprite;
    [SerializeField] private Sprite m_brokenMaskSprite;

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

    [Header("Combo Attack 1")]
    public float m_ComboAttackDamage1;
    public float m_ComboAttackLaunchDistance1;
    [Header("Combo Attack 2")]
    public float m_ComboAttackDamage2;
    public float m_ComboAttackLaunchDistance2;
    [Header("Combo Attack 3")]
    public float m_ComboAttackDamage3;
    public float m_ComboAttackLaunchDistance3;

    #region Main Functions

    private void Start()
    {
        m_playerHealth = Mathf.Clamp(m_playerHealth, 0, m_maxPlayerHealth);
        DisplayUIMasks();
        m_DamageResistance = 1;

        m_lvlManager = FindObjectOfType<LevelManager>();
        m_cameraAnim = Camera.main.gameObject.GetComponent<Animator>();
        m_controller = GetComponent<Controller>();
        m_playerInput = GetComponent<PlayerInput>();
        m_frenzyTimer = m_frenzyModeDefaultTimer;

        DeactivateFrenzyMode();
    }
    
      //TEMPORARY UPDATE FUNCTION JUST TO TEST IF FRENZY MODE WORKS WITHOUT NEEDING TO KILL AT THE MOMENT
        // FUNCTION IS ALSO CALLED AT ENEMY DEATH
    private void Update()
    {
        
        if (m_playerInput.actions["Attack"].triggered)
        {
            ActivateFrenzyMode();
        }
    }

    #endregion

    #region Health Functions

    public void TakeDMG(int incomingDMG)
    {
        int actualDamage = (int)(incomingDMG / m_DamageResistance);

        Debug.Log("Actual Damage : " + actualDamage);

        m_playerHealth -= actualDamage;

        DisplayUIMasks();

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

        DisplayUIMasks();

        //Play heal animation
    }

    private void DisplayUIMasks()
    {
        //for (int i = 0; i < m_lives.Length; i++)
        //{
        //    if (i < m_playerHealth)
        //    {
        //        m_lives[i].sprite = m_fullMaskSprite;
        //    }

        //    else
        //    {
        //        m_lives[i].sprite = m_brokenMaskSprite;
        //    }

        //    m_lives[i].enabled = (i < m_maxPlayerHealth);
        //}
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
