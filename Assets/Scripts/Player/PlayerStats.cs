using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    private LevelManager m_lvlManager;
    private PlayerInput m_playerInput;
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
    public float m_DefaultAttackDamage;
    public float m_AttackDamage;
    public float m_DamageResistance;

    [SerializeField] private Image[] m_lives;
    [SerializeField] private Sprite m_fullMaskSprite;
    [SerializeField] private Sprite m_brokenMaskSprite;

    private bool m_frenzyMode;
    [SerializeField] private float m_frenzyModeTimer;

    #region Main Functions

    private void Start()
    {
        m_playerHealth = Mathf.Clamp(m_playerHealth, 0, m_maxPlayerHealth);
        DisplayUIMasks();
        m_DamageResistance = 1;

        m_lvlManager = FindObjectOfType<LevelManager>();
        m_cameraAnim = Camera.main.gameObject.GetComponent<Animator>();
        m_playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        // TEMPORARY UPDATE FUNCTION JUST TO TEST IF FRENZY MODE WORKS WITHOUT NEEDING TO KILL AT THE MOMENT
        // FUNCTION IS ALSO CALLED AT ENEMY DEATH
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
        for (int i = 0; i < m_lives.Length; i++)
        {
            if (i < m_playerHealth)
            {
                m_lives[i].sprite = m_fullMaskSprite;
            }

            else
            {
                m_lives[i].sprite = m_brokenMaskSprite;
            }

            m_lives[i].enabled = (i < m_maxPlayerHealth);
        }
    }

    #endregion

    #region Frenzy Functions
    public void ActivateFrenzyMode()
    {
        if (m_frenzyMode)
        {
            CancelInvoke("DeactivateFrenzyMode");
            Invoke("DeactivateFrenzyMode", 3);
            Debug.Log("Frenzy Mode Replenished");
            return;
        }

        Invoke("DeactivateFrenzyMode", 3);
        Debug.Log("Frenzy Mode On!");
        m_frenzyMode = true;
    }

    private void DeactivateFrenzyMode()
    {
        Debug.Log("No More Frenzy");
        m_frenzyMode = false;
    }

    #endregion
}
