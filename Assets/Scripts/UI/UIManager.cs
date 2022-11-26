using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject m_gameHUD;
    [SerializeField] private GameObject m_pauseMenu;
    [SerializeField] private GameObject m_optionsMenu;

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
