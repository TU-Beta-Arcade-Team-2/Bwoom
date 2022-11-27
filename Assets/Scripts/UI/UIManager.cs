using UnityEngine;


public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject m_gameHUD;
    [SerializeField] private GameObject m_pauseMenu;
    [SerializeField] private UIOptions m_optionsMenu;

    protected override void InternalInit()
    {
        // Load and apply the ingame settings
        m_optionsMenu.LoadOptions();
    }

    public void ShowPauseMenu()
    {
        m_pauseMenu.SetActive(!m_pauseMenu.activeSelf);
        m_gameHUD.SetActive(!m_pauseMenu.activeSelf);
    }

    public void ShowOptionsMenu()
    {
        m_pauseMenu.SetActive(false);
        m_optionsMenu.gameObject.SetActive(true);
    }

    public void OnOptionsBackPressed()
    {
        m_pauseMenu.SetActive(true);

        m_optionsMenu.gameObject.SetActive(false);
        m_optionsMenu.SaveOptions();

        PlayUiClick();
    }

    public void PlayUiClick()
    {
        SoundManager.Instance.PlaySfx(StringConstants.UI_CLICK_SFX);
    }

    public void ToggleOptions()
    {
        m_optionsMenu.gameObject.SetActive(!m_optionsMenu.gameObject.activeSelf);
    }

    public void QuitToTitle()
    {
        m_gameHUD.SetActive(false);
        GameManager.Instance.PauseGame();
        SaveLoad.LoadLevel(StringConstants.TITLE_SCREEN_LEVEL);
    }
}
