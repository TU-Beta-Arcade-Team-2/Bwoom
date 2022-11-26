using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class UIOptions : MonoBehaviour
{
    private OptionsData m_optionsData = OptionsData.Defaults;

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
    [SerializeField] private GameObject m_videoSettingsButton;
    [SerializeField] private TMP_Dropdown m_windowModeDropdown;
    [SerializeField] private TMP_Dropdown m_screenResolutionDropdown;
    [SerializeField] private Toggle m_vsyncToggle;

    [Header("ACCESSIBILITY OPTIONS")]
    [SerializeField] private TMP_Dropdown m_colourBlindnessDropdown;
    [SerializeField] private Toggle m_holdToComboToggle;
    [SerializeField] private Toggle m_controllerRumbleToggle;


    [SerializeField] private PostProcessVolume m_normalVolume;
    [SerializeField] private PostProcessVolume m_achromaVolume;
    [SerializeField] private PostProcessVolume m_protoVolume;
    [SerializeField] private PostProcessVolume m_deuteroVolume;
    [SerializeField] private PostProcessVolume m_tritoVolume;

    void Awake()
    {
        // Load the previous saved Options
        LoadOptions();

        OnAudioButtonPress();

        // Only show the video button on windows
#if UNITY_STANDALONE_WIN
        m_videoSettingsButton.SetActive(true);
#else
        m_videoSettingsButton.SetActive(false);
#endif
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
        m_optionsData.VSync = !m_optionsData.VSync;

        QualitySettings.vSyncCount = m_optionsData.VSync ? 1 : 0;
    }

    private void SetVSync(bool vsync)
    {
        m_optionsData.VSync = vsync;
        QualitySettings.vSyncCount = vsync ? 1 : 0;
        m_vsyncToggle.isOn = vsync;
    }

    public void ToggleHoldToCombo()
    {
        m_optionsData.HoldToCombo = !m_optionsData.HoldToCombo;
    }

    private void SetHoldToCombo(bool holdToCombo)
    {
        m_optionsData.HoldToCombo = holdToCombo;
        m_holdToComboToggle.isOn = holdToCombo;
    }

    public void ToggleControllerRumble()
    {
        m_optionsData.ControllerRumble = !m_optionsData.ControllerRumble;
    }

    private void SetControllerRumble(bool controllerRumble)
    {
        m_optionsData.ControllerRumble = controllerRumble;
        m_controllerRumbleToggle.isOn = controllerRumble;
    }

    public void OnAudioButtonPress()
    {
        ShowOptionsPanel(StringConstants.AUDIO_SETTINGS, m_audioPanel);
        UIManager.Instance.PlayUiClick();
    }

#if UNITY_STANDALONE_WIN
    public void OnVideoButtonPress()
    {
        ShowOptionsPanel(StringConstants.VIDEO_SETTINGS, m_videoPanel);
        UIManager.Instance.PlayUiClick();
    }
#endif

    public void OnControlsButtonPress()
    {
        ShowOptionsPanel(StringConstants.CONTROLS_SETTINGS, m_controlsPanel);
        UIManager.Instance.PlayUiClick();
    }

    public void OnAccessibilityButtonPress()
    {
        ShowOptionsPanel(StringConstants.ACCESSIBILITY_SETTINGS, m_accessibilityPanel);
        UIManager.Instance.PlayUiClick();
    }

    public void OnBackButtonPress()
    {
        SaveLoad.SaveOptions(m_optionsData);
        UIManager.Instance.ToggleOptions();
        UIManager.Instance.PlayUiClick();
    }

    public void OnMasterVolumeChanged()
    {
        m_optionsData.MasterVolume = m_masterSlider.value;
        SoundManager.Instance.OnMasterVolumeChanged(m_optionsData.MasterVolume);
    }

    private void SetMasterVolume(float value)
    {
        m_optionsData.MasterVolume = value;
        m_masterSlider.value = value;
        SoundManager.Instance.OnMasterVolumeChanged(value);
    }

    public void OnMusicVolumeChanged()
    {
        m_optionsData.MusicVolume = m_musicSlider.value;
        SoundManager.Instance.OnMusicVolumeChanged(m_optionsData.MusicVolume);
    }

    private void SetMusicVolume(float value)
    {
        m_optionsData.MusicVolume = value;
        m_musicSlider.value = value;
        SoundManager.Instance.OnMusicVolumeChanged(value);
    }

    public void OnSfxVolumeChanged()
    {
        m_optionsData.SfxVolume = m_sfxSlider.value;
        SoundManager.Instance.OnSfxVolumeChanged(m_optionsData.SfxVolume);
    }

    private void SetSfxVolume(float value)
    {
        m_optionsData.SfxVolume = value;
        m_sfxSlider.value = value;
        SoundManager.Instance.OnSfxVolumeChanged(value);
    }

    private void ShowOptionsPanel(string panelTitle, GameObject panel)
    {
        m_panelTitleText.text = panelTitle;
        HideAllPanels();
        panel.SetActive(true);
        UIManager.Instance.PlayUiClick();
    }

    public void OnScreenResolutionChanged()
    {
        BetterDebugging.Instance.Assert(m_screenResolutionDropdown.value < (int)OptionsData.eScreenResolution.Count, "MAKE SURE TO ADJUST THE SCREEN RESOLUTION ENUM WHEN ADDING NEW RESOLUTIONS");

        SetScreenResolution((OptionsData.eScreenResolution)m_screenResolutionDropdown.value);
    }

    private void SetScreenResolution(OptionsData.eScreenResolution res)
    {
        m_optionsData.ScreenResolution = res;

        FullScreenMode fsMode = Screen.fullScreenMode;

        switch (m_optionsData.ScreenResolution)
        {
            case OptionsData.eScreenResolution.r1920x1080:
                Screen.SetResolution(1920, 1080, fsMode);
                break;
            case OptionsData.eScreenResolution.r1280x960:
                Screen.SetResolution(1280, 960, fsMode);
                break;
            case OptionsData.eScreenResolution.r1024x768:
                Screen.SetResolution(1024, 768, fsMode);
                break;
            case OptionsData.eScreenResolution.r960x540:
                Screen.SetResolution(960, 540, fsMode);
                break;
            case OptionsData.eScreenResolution.r640x360:
                Screen.SetResolution(640, 360, fsMode);
                break;
            default:
                BetterDebugging.Instance.Assert(false, $"UNHANDLED CASE {m_optionsData.ScreenResolution}");
                break;
        }
    }

    public void OnWindowModeChanged()
    {
        BetterDebugging.Instance.Assert(m_windowModeDropdown.value < (int)OptionsData.eWindowMode.Count, "MAKE SURE TO ADJUST THE WINDOW MODE ENUM WHEN ADDING NEW OPTIONS");

        SetWindowMode((OptionsData.eWindowMode)m_windowModeDropdown.value);

        UIManager.Instance.PlayUiClick();
    }

    private void SetWindowMode(OptionsData.eWindowMode windowMode)
    {
        m_optionsData.WindowMode = windowMode;

        switch (m_optionsData.WindowMode)
        {
            case OptionsData.eWindowMode.Windowed:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case OptionsData.eWindowMode.Borderless:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
            case OptionsData.eWindowMode.FullScreen:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            default:
                BetterDebugging.Instance.Assert(false, $"UNHANDLED CASE {m_optionsData.WindowMode}");
                break;
        }
    }

    public void OnColourBlindnessChanged()
    {
        BetterDebugging.Instance.Assert(m_colourBlindnessDropdown.value < (int)OptionsData.eColourBlindness.Count, "MAKE SURE TO ADJUST THE COLOUR BLINDNESS ENUM WHEN ADDING NEW OPTIONS");

        SetColourBlindness((OptionsData.eColourBlindness)m_colourBlindnessDropdown.value);

        UIManager.Instance.PlayUiClick();
    }

    private void SetColourBlindness(OptionsData.eColourBlindness newColourBlindnessSetting)
    {
        m_optionsData.ColourBlindness = newColourBlindnessSetting;

        m_colourBlindnessDropdown.value = (int)newColourBlindnessSetting;

        m_normalVolume.gameObject.SetActive(false);
        m_achromaVolume.gameObject.SetActive(false);
        m_protoVolume.gameObject.SetActive(false);
        m_deuteroVolume.gameObject.SetActive(false);
        m_tritoVolume.gameObject.SetActive(false);

        switch (m_optionsData.ColourBlindness)
        {
            case OptionsData.eColourBlindness.None:
                m_normalVolume.gameObject.SetActive(true);
                break;
            case OptionsData.eColourBlindness.Achroma:
                m_achromaVolume.gameObject.SetActive(true);
                break;
            case OptionsData.eColourBlindness.Proto:
                m_protoVolume.gameObject.SetActive(true);
                break;
            case OptionsData.eColourBlindness.Deutero:
                m_deuteroVolume.gameObject.SetActive(true);
                break;
            case OptionsData.eColourBlindness.Trito:
                m_tritoVolume.gameObject.SetActive(true);
                break;
            default:
                BetterDebugging.Instance.Assert(false, $"UNHANDLED CASE {m_optionsData.ColourBlindness}");
                break;
        }
    }

    public void SaveOptions()
    {
        SaveLoad.SaveOptions(m_optionsData);
    }

    public void LoadOptions()
    {
        m_optionsData = SaveLoad.LoadOptions();

        // Set the values in game from the OptionsData object
        SetMasterVolume(m_optionsData.MasterVolume);
        SetMusicVolume(m_optionsData.MusicVolume);
        SetSfxVolume(m_optionsData.SfxVolume);

        SetScreenResolution(m_optionsData.ScreenResolution);
        SetWindowMode(m_optionsData.WindowMode);
        SetVSync(m_optionsData.VSync);
        SetHoldToCombo(m_optionsData.HoldToCombo);
        SetControllerRumble(m_optionsData.ControllerRumble);

        SetColourBlindness(m_optionsData.ColourBlindness);
    }
}
