using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    public Animator m_Transition;

    public void QuitGame()
    {
        
        Application.Quit();
  
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
