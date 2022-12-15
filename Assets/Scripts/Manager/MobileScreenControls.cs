using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MobileButtonSprites
{
    [SerializeField] public Sprite JoystickBackground;
    [SerializeField] public Sprite Joystick;
    [SerializeField] public Sprite ButtonSwitch;
    [SerializeField] public Sprite ButtonJump;
    [SerializeField] public Sprite ButtonSpecial;
    [SerializeField] public Sprite ButtonAttack;
    [SerializeField] public Sprite ButtonPause;
}

public class MobileScreenControls : MonoBehaviour
{
#if UNITY_ANDROID || UNITY_IOS
    [Header("Screen Reference")]
    [SerializeField] private GameObject m_mobileScreen;
    [Space(10)]

    [Header("Button Prefabs")]
    [SerializeField] private Image m_joystickBackgroundImage;
    [SerializeField] private Image m_joystickImage;
    [SerializeField] private Image m_buttonSwitchImage;
    [SerializeField] private Image m_buttonJumpImage;
    [SerializeField] private Image m_buttonSpecialImage;
    [SerializeField] private Image m_buttonAttackImage;
    [SerializeField] private Image m_buttonPauseImage;
    [Space(10)]

    [Header("WarButtonSprites")]
    [SerializeField] private MobileButtonSprites m_warButtons;
    [SerializeField] private MobileButtonSprites m_natureButtons;

    private void Awake()
    {
        m_mobileScreen.SetActive(true);
        Switch(true);
    }

    public void Switch(bool war)
    {
        if (war)
        {
            m_joystickBackgroundImage.sprite = m_warButtons.JoystickBackground;
            m_joystickImage.sprite = m_warButtons.Joystick;
            m_buttonSwitchImage.sprite = m_warButtons.ButtonSwitch;
            m_buttonJumpImage.sprite = m_warButtons.ButtonJump;
            m_buttonSpecialImage.sprite = m_warButtons.ButtonSpecial;
            m_buttonAttackImage.sprite = m_warButtons.ButtonAttack;
            m_buttonPauseImage.sprite = m_warButtons.ButtonPause;

            return;
        }

        m_joystickBackgroundImage.sprite = m_natureButtons.JoystickBackground;
        m_joystickImage.sprite = m_natureButtons.Joystick;
        m_buttonSwitchImage.sprite = m_natureButtons.ButtonSwitch;
        m_buttonJumpImage.sprite = m_natureButtons.ButtonJump;
        m_buttonSpecialImage.sprite = m_natureButtons.ButtonSpecial;
        m_buttonAttackImage.sprite = m_natureButtons.ButtonAttack;
        m_buttonPauseImage.sprite = m_natureButtons.ButtonPause;
    }

#endif
}
