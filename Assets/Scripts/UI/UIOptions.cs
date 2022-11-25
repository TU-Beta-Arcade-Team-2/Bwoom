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
        Options.VSYNC = !Options.VSYNC;
    }

    public void ToggleHoldToCombo()
    {
        Options.HOLD_TO_COMBO = !Options.HOLD_TO_COMBO;
    }

    public void ToggleControllerRumble()
    {
        Options.CONTROLLER_RUMBLE = !Options.CONTROLLER_RUMBLE;
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

    public void OnMasterVolumeChanged()
    {
        Options.MASTER_VOLUME = m_masterSlider.value;
        // TODO: NOTIFY THE SOUND MANAGER
    }

    public void OnMusicVolumeChanged()
    {
        Options.MUSIC_VOLUME = m_masterSlider.value;
        // TODO: NOTIFY THE SOUND MANAGER
    }

    public void OnSfxVolumeChanged()
    {
        Options.SFX_VOLUME = m_masterSlider.value;
        // TODO: NOTIFY THE SOUND MANAGER
    }

    public void OnScreenResolutionChanged()
    {
        BetterDebugging.Instance.Assert(m_screenResolutionDropdown.value < (int)Options.eScreenResolution.Count, "MAKE SURE TO ADJUST THE SCREEN RESOLUTION ENUM WHEN ADDING NEW RESOLUTIONS");

        Options.SCREEN_RESOLUTION = (Options.eScreenResolution) m_screenResolutionDropdown.value;
    }

    public void OnWindowModeChanged()
    {
        BetterDebugging.Instance.Assert(m_windowModeDropdown.value < (int)Options.eWindowMode.Count, "MAKE SURE TO ADJUST THE WINDOW MODE ENUM WHEN ADDING NEW OPTIONS");

        Options.SCREEN_MODE = (Options.eWindowMode)m_windowModeDropdown.value;
    }

    public void OnColourBlindnessChanged()
    {
        BetterDebugging.Instance.Assert(m_colourBlindnessDropdown.value < (int)Options.eColourBlindness.Count, "MAKE SURE TO ADJUST THE COLOUR BLINDNESS ENUM WHEN ADDING NEW OPTIONS");

        Options.COLOUR_BLINDNESS = (Options.eColourBlindness)m_colourBlindnessDropdown.value;
    }

    private void SaveOptions()
    {
        // TODO: SERIALISE TO A .SETTINGS FILE
    }

    private void LoadOptions()
    {
        // TODO: DESERIALISE FROM THE .SETTINGS FILE
    }
}
