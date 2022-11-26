using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : Singleton<UIManager>
{
    [Tooltip("This only needs to be assigned ")]
    [SerializeField] private GameObject m_continueButton;

    [SerializeField] private GameObject m_gameHUD;
    [SerializeField] private GameObject m_pauseMenu;
    [SerializeField] private GameObject m_optionsMenu;

    private bool m_isMainMenu;


    private void Start()
    {
        if (SceneManager.GetActiveScene().name == StringConstants.TITLE_SCREEN_LEVEL)
        {
            // Show the Continue game button only if we've never saved
            m_continueButton.SetActive(SaveLoad.DoesSaveGameExist());

            m_isMainMenu = true;
        }
        else
        {
            m_isMainMenu = false;
        }
    }

    public void OnTitleScreenNewGamePressed()
    {
        SaveLoad.LoadLevel(StringConstants.NATURE_LEVEL);
    }

    public void OnTitleScreenContinueGamePressed()
    {
        SaveLoad.LoadGame();
        GameManager.SHOULD_LOAD_STATS = true;
    }

    public void ShowPauseMenu()
    {
        m_pauseMenu.SetActive(!m_pauseMenu.activeSelf);
        m_gameHUD.SetActive(!m_pauseMenu.activeSelf);
    }

    public void ShowOptionsMenu()
    {
        m_pauseMenu.SetActive(false);
        m_optionsMenu.SetActive(true);
    }

    public void OnOptionsBackPressed()
    {
        m_pauseMenu.SetActive(true);
        m_optionsMenu.SetActive(false);
        PlayUiClick();
    }

    public void PlayUiClick()
    {
        SoundManager.Instance.PlaySfx(StringConstants.UI_CLICK_SFX);
    }

    public void ToggleOptions()
    {
        m_optionsMenu.SetActive(!m_optionsMenu.activeSelf);
    }
}
