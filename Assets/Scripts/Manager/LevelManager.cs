using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject GameHud;
    [SerializeField] private GameObject DeathHud;

    private void Start()
    {
        GameHud.SetActive(true);
        DeathHud.SetActive(false);
    }

    public void Death()
    {
        GameHud.SetActive(false);
        DeathHud.SetActive(true);
    }

    public void Continue_Button()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Quit_Button()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
