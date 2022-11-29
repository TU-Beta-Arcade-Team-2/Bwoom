using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : Singleton<GameHUD>
{
    [SerializeField] Image m_uiMaskImage;
    [SerializeField] private Sprite m_warMaskIcon;
    [SerializeField] private Sprite m_natureMaskIcon;

    [SerializeField] private TextMeshProUGUI m_pointText;
    [SerializeField] private Image m_radialHealthBar;
    [SerializeField] private Image m_radialFrenzyBar;

    protected override void InternalInit()
    {
        BetterDebugging.Assert(m_uiMaskImage != null, "MASK IMAGE SHOULDN'T BE NULL!");
        BetterDebugging.Assert(m_warMaskIcon != null, "WAR MASK IMAGE SHOULDN'T BE NULL!");

        BetterDebugging.Assert(m_natureMaskIcon != null, "NATURE MASK IMAGE SHOULDN'T BE NULL!");

        BetterDebugging.Assert(m_pointText != null, "POINT TEXT SHOULDN'T BE NULL!");

        BetterDebugging.Assert(m_radialHealthBar != null, "HEALTH BAR SHOULDN'T BE NULL!");
    }

    public void UpdateHealthBar(int healthValue, int maxHealthValue)
    {
        m_radialHealthBar.fillAmount = healthValue / (float)maxHealthValue;
    }

    public void UpdateFrenzyBar(float frenzyTime, float maxFrenzyTime)
    {
        float fillAmount = frenzyTime / maxFrenzyTime;

        m_radialFrenzyBar.fillAmount = fillAmount;
    }

    public void UpdatePoints(int value)
    {
        m_pointText.text = value.ToString();
    }

    public void UpdateMaskIcon(PlayerController.eMasks mask)
    {
        switch (mask)
        {
            case PlayerController.eMasks.War:
                m_uiMaskImage.sprite = m_warMaskIcon;
                break;
            case PlayerController.eMasks.Nature:
                m_uiMaskImage.sprite = m_natureMaskIcon;
                break;
            default:
                BetterDebugging.Assert(false, $"UNHANDLED MASK IN THE GAMEHUD {mask}");
                break;
        }
    }
}
