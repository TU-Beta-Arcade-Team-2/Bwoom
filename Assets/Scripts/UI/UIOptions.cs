using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIOptions : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_panelTitleText;

    [SerializeField] private GameObject m_accessibilityPanel;
    [SerializeField] private GameObject m_videoPanel;
    [SerializeField] private GameObject m_audioPanel;
    [SerializeField] private GameObject m_controlsPanel;

    [Header("AUDIO OPTIONS")] 
    [SerializeField] private Slider m_masterSlider;
    [SerializeField] private Slider m_musicSlider;
    [SerializeField] private Slider m_sfxSlider;

    [Header("VIDEO OPTIONS")] 
    [SerializeField] private TMP_Dropdown m_windowModeDropdown;
    [SerializeField] private TMP_Dropdown m_screenResolutionDropdown;

    [Header("ACCESSIBILITY OPTIONS")]
    [SerializeField] private TMP_Dropdown m_colourBlindnessDropdown;



    void Awake()
    {
        // Load the previous saved Options
        LoadOptions();

        ShowAudioOptions();
    }

    public void ShowAudioOptions()
    {
        HideAllPanels();
        m_audioPanel.SetActive(true);
    }

    public void BackButton()
    {
        HideAllPanels();
        SaveOptions();
    }

    public void HideAllPanels()
    {
        m_accessibilityPanel.SetActive(false);
        m_videoPanel.SetActive(false);
        m_audioPanel.SetActive(false);
        m_controlsPanel.SetActive(false);
    }

    public void ToggleVsync()
    {

    }

    public void ToggleHoldToCombo()
    {

    }

    public void ToggleControllerRumble()
    {

    }

    public void OnAudioButtonPress()
    {
        m_panelTitleText.text = StringConstants.AUDIO_SETTINGS;
        HideAllPanels();
        m_audioPanel.SetActive(true);
    }

    public void OnVideoButtonPress()
    {
        m_panelTitleText.text = StringConstants.VIDEO_SETTINGS;
        HideAllPanels();
        m_videoPanel.SetActive(true);
    }

    public void OnControlsButtonPress()
    {
        m_panelTitleText.text = StringConstants.CONTROLS_SETTINGS;
        HideAllPanels();
        m_controlsPanel.SetActive(true);
    }

    public void OnAccessibilityButtonPress()
    {
        m_panelTitleText.text = StringConstants.ACCESSIBILITY_SETTINGS;
        HideAllPanels();
        m_accessibilityPanel.SetActive(true);
    }

    public void OnBackButtonPress()
    {
        UIManager.Instance.ToggleOptions();
    }

    private void SaveOptions()
    {
    }

    private void LoadOptions()
    {
    }
}
