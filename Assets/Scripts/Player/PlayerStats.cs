using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] private int m_totalPoints;
    [SerializeField] private TextMeshProUGUI m_pointText;
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
        m_playerHealth = Mathf.Clamp(m_playerHealth, 0, m_maxPlayerHealth);
        m_radialHealthBar.fillAmount = (float)m_playerHealth / (float)m_maxPlayerHealth;
        m_pointText.text = m_totalPoints.ToString();
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

    #region Health & Point Functions
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

    #region Debug

    public void Switch(Image healthBar, Image maskIcon)
    {
        m_radialHealthBar = healthBar;
        MaskIconImage = maskIcon;
        m_radialHealthBar.fillAmount = (float)m_playerHealth / (float)m_maxPlayerHealth;
        MaskIconImage.sprite = WarMaskIcon;
    }

    #endregion

    // TODO: This probably isn't the best place to put this but hey ho...
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(StringConstants.CHECKPOINT_STRING))
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
}
