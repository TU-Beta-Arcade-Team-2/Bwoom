using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerController m_playerController;

    [Header("Health Variables")]
    [SerializeField] private int m_maxHealth;
    [SerializeField] private int m_health;

    [HideInInspector]
    public int Health
    {
        get => m_health;
        set
        {
            GameHUD.Instance.UpdateHealthBar(value, m_maxHealth);
            m_health = value;
        }
    }

    [SerializeField] private int m_points;

    [HideInInspector]
    public int Points
    {
        get => m_points;
        set
        {
            GameHUD.Instance.UpdatePoints(value);
            m_points = value;
        }
    }


    [System.Serializable]
    public class Stats
    {
        public Stats(Stats s)
        {
            MovementSpeed = s.MovementSpeed;
            JumpHeight = s.JumpHeight;
            AttackDamage = s.AttackDamage;
            DamageResistance = s.DamageResistance;
        }

        public Stats(float movementSpeed, float jumpHeight, float attackDamage, float damageResistance)
        {
            MovementSpeed = movementSpeed;
            JumpHeight = jumpHeight;
            AttackDamage = attackDamage;
            DamageResistance = damageResistance;
        }

        public float MovementSpeed;
        public float JumpHeight;
        public float AttackDamage;
        public float DamageResistance;
    }

    [SerializeField] private Stats m_defaultStats;
    [SerializeField] private Stats m_frenzyStats;
    private Stats m_currentStats;


    [Space(10)]

    [Header("Frenzy Mode Values")]
    private bool m_frenzyMode;
    [SerializeField] private float m_frenzyModeDefaultTimer;
    [SerializeField] private float m_frenzyTimerIncrement;
    private float m_frenzyTimer;

    private Vector3 m_lastCheckpointPosition;

    #region Main Functions
    private void Start()
    {
        // Set the current stats to the default ones...
        m_currentStats = m_defaultStats;


        Health = Mathf.Clamp(m_health, 0, m_maxHealth);
        Points = 0;
        

        m_frenzyTimer = m_frenzyModeDefaultTimer;

        DeactivateFrenzyMode();
    }
    #endregion

    #region Health & Point Functions
    public void TakeDamage(int incomingDamage)
    {
        int actualDamage = (int)(incomingDamage / m_currentStats.DamageResistance);

        BetterDebugging.Instance.DebugLog("Actual Damage : " + actualDamage, BetterDebugging.eDebugLevel.Message);

        Health -= actualDamage;

        // TODO: Tell Cinemachine to shake! m_cameraAnim.SetTrigger("LightShake");

        if (Health > 0)
        {
            // TODO: Play hurt animation      
        }
        else
        {
            GameManager.Instance.Death();
            Destroy(gameObject);
        }
    }

    public void HealPlayer(int healAmount)
    {
        Debug.Log("HEAL TEST");

        Health = Mathf.Clamp(m_health + healAmount, 0, m_maxHealth);

        // TODO: Play heal animation
    }
    #endregion

    public void AddPoints(int pointsToAdd)
    {
        Points += pointsToAdd;
    }

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

        m_currentStats = new(m_frenzyStats);
        m_playerController.doubleJumpOn = true;
        m_playerController.SetMovementValues(m_frenzyStats);

        Invoke("DeactivateFrenzyMode", m_frenzyTimer);
        Debug.Log("Frenzy Mode On!");
        m_frenzyMode = true;
    }

    private void DeactivateFrenzyMode()
    {
        m_frenzyTimer = m_frenzyModeDefaultTimer;

        m_currentStats = new(m_defaultStats);

        m_playerController.doubleJumpOn = false;
        m_playerController.SetMovementValues(m_defaultStats);

        Debug.Log("No More Frenzy");
        m_frenzyMode = false;
    }

    #endregion

    // TODO: This probably isn't the best place to put this but hey ho...
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(StringConstants.CHECKPOINT_STRING))
        {
            m_lastCheckpointPosition = collision.gameObject.transform.position;
            SaveLoad.SaveGame(this);
        }
    }

    public int GetPoints()
    {
        return m_points;
    }

    public int GetHealth()
    {
        return m_health;
    }

    public Vector3 GetLastCheckpointPosition()
    {
        return m_lastCheckpointPosition;
    }

    public string GetCurrentLevelName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void SetHealth(int playerHealth)
    {
        m_health = playerHealth;
    }

    public void MultiplyStats(Stats maskMultiplier)
    {
        m_currentStats = new(m_defaultStats);

        m_currentStats.MovementSpeed *= maskMultiplier.MovementSpeed;
        m_currentStats.JumpHeight *= maskMultiplier.JumpHeight;
        m_currentStats.DamageResistance *= maskMultiplier.DamageResistance;
        m_currentStats.AttackDamage *= maskMultiplier.AttackDamage;

        m_playerController.SetMovementValues(m_currentStats);
    }

    public void ResetPlayerStats()
    {
        m_currentStats = m_defaultStats;
    }
}
