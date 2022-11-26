using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : Singleton<UIManager>
{
    public Animator m_Transition;

    public bool IN_GAME = false;

    [SerializeField] private GameObject m_continueButton;

    [SerializeField] private GameObject m_gameHUD;
    [SerializeField] private GameObject m_pauseMenu;
    [SerializeField] private GameObject m_optionsMenu;


    private void Start()
    {
        if (!IN_GAME)
        {
            // Show the Continue game button only if we've never saved
            m_continueButton.SetActive(SaveLoad.DoesSaveGameExist());
        }
    }

    public void StartNewGame()
    {
        LoadLevel(StringConstants.NATURE_LEVEL);
    }

    public void ContinueGame()
    {
        SaveLoad.LoadGame();
        LoadLevel(SaveLoad.SCENE_NAME);
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
        // TODO: Behave differently on the main menu vs in game
        m_pauseMenu.SetActive(true);
        m_optionsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayUiClick()
    {
        SoundManager.Instance.PlaySfx(StringConstants.UI_CLICK_SFX);
    }


    private void LoadLevel(string sceneName)
    {
        StartCoroutine(AsyncLoadLevel(sceneName));
    }

    private IEnumerator AsyncLoadLevel(string sceneName)
    {
        m_Transition.SetTrigger("Fade");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(sceneName);
    }

    public void ToggleOptions()
    {
        m_optionsMenu.SetActive(!m_optionsMenu.activeSelf);
    }
}
