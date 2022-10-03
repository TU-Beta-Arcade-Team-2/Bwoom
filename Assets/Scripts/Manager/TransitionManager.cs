using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[SerializeField]
public static class TransitionManager
{
    public static Animator transAnim;

    public static void SceneSkip(string givenSceneName)
    {
        SceneManager.LoadScene(sceneName: givenSceneName);
        transAnim.SetTrigger("4");
    }

    //This is called before transitioning to the next scene or after transitioning.
    public static void Transition(int transitionType)
    {
        transAnim.SetTrigger(transitionType.ToString());
    }

}
