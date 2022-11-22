using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : Singleton<UIManager>
{
    public Animator m_Transition;

    public bool IN_GAME;
    [SerializeField] private GameObject m_pauseMenu;

    [SerializeField] private GameObject m_continueButton;

    private void Start()
    {
        // Show the Continue game button only if we've never saved
        if(SaveLoad.DoesSaveGameExist())
        {
            m_continueButton.SetActive(true);
        }else
        {
            m_continueButton.SetActive(false);
        }
    }

    private void Update()
    {
        if (IN_GAME)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) //TODO: Change to input maager
            {
                if (m_pauseMenu.activeSelf)
                {
                    m_pauseMenu.SetActive(false);
                    Time.timeScale = 1f;
                }
                else
                {
                    m_pauseMenu.SetActive(true);
                    Time.timeScale = 0f;
                }
            }
        }
    }

    public void StartNewGame()
    {
        StartCoroutine(LoadLevel(StringConstants.NATURE_LEVEL));
    }

    public void ContinueGame()
    {
        SaveLoad.LoadGame();
    }

    public void ShowOptionsMenu()
    {

    }

    public void PauseGame()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadLevel(string sceneName)
    {
        m_Transition.SetTrigger("Fade");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(sceneName);
    }
}
