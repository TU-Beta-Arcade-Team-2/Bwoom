using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
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
    [SerializeField] private GameObject m_videoSettingsButton;
    [SerializeField] private TMP_Dropdown m_windowModeDropdown;
    [SerializeField] private TMP_Dropdown m_screenResolutionDropdown;

    [Header("ACCESSIBILITY OPTIONS")]
    [SerializeField] private TMP_Dropdown m_colourBlindnessDropdown;

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

        // REMOVE AFTER TESTING
        SoundManager.Instance.PlayMusic(StringConstants.NATURE_LEVEL_SOUNDTRACK, true);
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
        OptionsData.VSYNC = !OptionsData.VSYNC;

        QualitySettings.vSyncCount = OptionsData.VSYNC ? 1 : 0;
    }

    public void ToggleHoldToCombo()
    {
        OptionsData.HOLD_TO_COMBO = !OptionsData.HOLD_TO_COMBO;
    }

    public void ToggleControllerRumble()
    {
        OptionsData.CONTROLLER_RUMBLE = !OptionsData.CONTROLLER_RUMBLE;
    }

    public void OnAudioButtonPress()
    {
        ShowOptionsPanel(StringConstants.AUDIO_SETTINGS, m_audioPanel);
    }

#if UNITY_STANDALONE_WIN
    public void OnVideoButtonPress()
    {
        ShowOptionsPanel(StringConstants.VIDEO_SETTINGS, m_videoPanel);
    }
#endif

    public void OnControlsButtonPress()
    {
        ShowOptionsPanel(StringConstants.CONTROLS_SETTINGS, m_controlsPanel);
    }

    public void OnAccessibilityButtonPress()
    {
        ShowOptionsPanel(StringConstants.ACCESSIBILITY_SETTINGS, m_accessibilityPanel);
    }

    public void OnBackButtonPress()
    {
        UIManager.Instance.ToggleOptions();
    }

    public void OnMasterVolumeChanged()
    {
        OptionsData.MASTER_VOLUME = m_masterSlider.value;
        SoundManager.Instance.OnMasterVolumeChanged(OptionsData.MASTER_VOLUME);
    }

    public void OnMusicVolumeChanged()
    {
        OptionsData.MUSIC_VOLUME = m_musicSlider.value;
        SoundManager.Instance.OnMusicVolumeChanged(OptionsData.MUSIC_VOLUME);
    }

    public void OnSfxVolumeChanged()
    {
        OptionsData.SFX_VOLUME = m_sfxSlider.value;
        SoundManager.Instance.OnSfxVolumeChanged(OptionsData.SFX_VOLUME);
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

        OptionsData.SCREEN_RESOLUTION = (OptionsData.eScreenResolution)m_screenResolutionDropdown.value;

        FullScreenMode fsMode = Screen.fullScreenMode;

        switch (OptionsData.SCREEN_RESOLUTION)
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
                BetterDebugging.Instance.Assert(false, $"UNHANDLED CASE {OptionsData.SCREEN_RESOLUTION}");
                break;
        }
    }

    public void OnWindowModeChanged()
    {
        BetterDebugging.Instance.Assert(m_windowModeDropdown.value < (int)OptionsData.eWindowMode.Count, "MAKE SURE TO ADJUST THE WINDOW MODE ENUM WHEN ADDING NEW OPTIONS");

        OptionsData.SCREEN_MODE = (OptionsData.eWindowMode)m_windowModeDropdown.value;


        switch (OptionsData.SCREEN_MODE)
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
                BetterDebugging.Instance.Assert(false, $"UNHANDLED CASE {OptionsData.SCREEN_MODE}");
                break;
        }

    }

    public void OnColourBlindnessChanged()
    {
        m_normalVolume.gameObject.SetActive(false);
        m_achromaVolume.gameObject.SetActive(false);
        m_protoVolume.gameObject.SetActive(false);
        m_deuteroVolume.gameObject.SetActive(false);
        m_tritoVolume.gameObject.SetActive(false);


        BetterDebugging.Instance.Assert(m_colourBlindnessDropdown.value < (int)OptionsData.eColourBlindness.Count, "MAKE SURE TO ADJUST THE COLOUR BLINDNESS ENUM WHEN ADDING NEW OPTIONS");

        OptionsData.COLOUR_BLINDNESS = (OptionsData.eColourBlindness)m_colourBlindnessDropdown.value;

        switch (OptionsData.COLOUR_BLINDNESS)
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
                BetterDebugging.Instance.Assert(false, $"UNHANDLED CASE {OptionsData.COLOUR_BLINDNESS}");
                break;
        }
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
