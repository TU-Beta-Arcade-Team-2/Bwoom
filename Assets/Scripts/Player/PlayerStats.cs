using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerController m_playerController;

    [Header("Health Variables")]
    [SerializeField] private int m_playerHealth;
    [SerializeField] private int m_maxPlayerHealth;
    [Space(10)]

    [Header("Game HUD Variables")]
    [SerializeField] private int m_totalPoints;
    [SerializeField] private TextMeshProUGUI m_pointText;
    [SerializeField] private Image m_radialHealthBar;
    public Image MaskIconImage;
    public Sprite WarMaskIcon;
    public Sprite NatureMaskIcon;

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

    [Header("Combo Attack 1")]
    public float m_ComboAttackDamage1;
    [Tooltip("Launches enemy")]
    public Vector3 m_ComboAttackEnemyLaunchVector1;
    public Vector3 m_ComboAttackPlayerLaunchVector1;
    [Header("Combo Attack 2")]
    public float m_ComboAttackDamage2;
    [Tooltip("Launches player")]
    public Vector3 m_ComboAttackEnemyLaunchVector2;
    public Vector3 m_ComboAttackPlayerLaunchVector2;
    [Header("Combo Attack 3")]
    public float m_ComboAttackDamage3;
    [Tooltip("Launches enemy")]
    public Vector3 m_ComboAttackEnemyLaunchVector3;
    public Vector3 m_ComboAttackPlayerLaunchVector3;
    private Vector3 m_lastCheckpointPosition;

    #region Main Functions
    private void Start()
    {
        // Set the current stats to the default ones...
        m_currentStats = m_defaultStats;


        m_playerHealth = Mathf.Clamp(m_playerHealth, 0, m_maxPlayerHealth);
        m_radialHealthBar.fillAmount = m_playerHealth / (float)m_maxPlayerHealth;
        m_pointText.text = m_totalPoints.ToString();
        MaskIconImage.sprite = WarMaskIcon;

        m_frenzyTimer = m_frenzyModeDefaultTimer;

        DeactivateFrenzyMode();
    }
    #endregion

    #region Health & Point Functions
    public void TakeDamage(int incomingDamage)
    {
        int actualDamage = (int)(incomingDamage / m_currentStats.DamageResistance);

        BetterDebugging.Instance.DebugLog("Actual Damage : " + actualDamage, BetterDebugging.eDebugLevel.Message);

        m_playerHealth -= actualDamage;
        m_radialHealthBar.fillAmount = m_playerHealth / (float)m_maxPlayerHealth;

        // TODO: Tell Cinemachine to shake! m_cameraAnim.SetTrigger("LightShake");

        if (m_playerHealth > 0)
        {
            // TODO: Play hurt animation      
        }
        else
        {
            GameManager.Instance.PlayerDied();
            Destroy(gameObject);
        }
    }

    public void HealPlayer(int healAmount)
    {
        Debug.Log("HEAL TEST");

        m_playerHealth = Mathf.Clamp(m_playerHealth + healAmount, 0, m_maxPlayerHealth);
        m_radialHealthBar.fillAmount = m_playerHealth / (float)m_maxPlayerHealth;

        // TODO: Play heal animation
    }
    #endregion

    public void AddPoints(int pointsToAdd)
    {
        m_totalPoints += pointsToAdd;
        m_pointText.text = m_totalPoints.ToString();
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

    #region Debug

    public void Switch(Image healthBar, Image maskIcon)
    {
        m_radialHealthBar = healthBar;
        MaskIconImage = maskIcon;
        m_radialHealthBar.fillAmount = m_playerHealth / (float)m_maxPlayerHealth;
        MaskIconImage.sprite = WarMaskIcon;
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
        return m_totalPoints;
    }

    public int GetHealth()
    {
        return m_playerHealth;
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
        m_playerHealth = playerHealth;
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
