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

    [SerializeField] private Image[] m_lives;
    [SerializeField] private Sprite m_fullMaskSprite;
    [SerializeField] private Sprite m_brokenMaskSprite;
    [Space(10)]

    [Header("Mask Ability Variables")]
    [SerializeField] private eMasks m_maskSelected;
    [SerializeField] private SpriteRenderer m_playerMask;
    public MaskClass[] masks;

    #region Main Functions

    private void Start()
    {
        m_playerHealth = Mathf.Clamp(m_playerHealth, 0, m_maxPlayerHealth);
        DisplayUIMasks();

        m_lvlManager = FindObjectOfType<LevelManager>();
        m_cameraAnim = Camera.main.gameObject.GetComponent<Animator>();
        m_playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (m_playerInput.actions["OptionOne"].triggered) //will also include an if statement checking if the selected mask has been unlocked
        {
            MaskChange(0);
        }

        if (m_playerInput.actions["OptionTwo"].triggered) //will also include an if statement checking if the selected mask has been unlocked
        {
            MaskChange(1);
        }

        if (m_playerInput.actions["OptionThree"].triggered) //will also include an if statement checking if the selected mask has been unlocked
        {
            MaskChange(2);
        }

        if (m_playerInput.actions["OptionFour"].triggered) //will also include an if statement checking if the selected mask has been unlocked
        {
            MaskChange(3);
        }
    }

    #endregion

    #region Health Functions

    public void TakeDMG(int incomingDMG)
    {
        m_playerHealth -= incomingDMG;

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

    #region Mask Ability Functions

    public void MaskChange(int maskNo)
    {
        if (masks[maskNo].Unlocked)
        {
            m_playerMask.sprite = masks[maskNo].MaskSprite;
            m_maskSelected = eMasks.natureMask;
            return;
        }

        //Play an locked mask sound to signal the player that there isn't an option here
    }

    #endregion
}
