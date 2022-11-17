using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    public Animator m_Transition;

    public bool IN_GAME;
    [SerializeField] private GameObject m_pauseMenu;

    public void QuitGame()
    {
        
        Application.Quit();
  
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


    //Level Loader Code from a brackeys tutorial - https://www.youtube.com/watch?v=CE9VOZivb3I

    public void LoadNextLevel(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    

    IEnumerator LoadLevel(string sceneName)
    {
        m_Transition.SetTrigger("Fade");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(sceneName);
    }


}
