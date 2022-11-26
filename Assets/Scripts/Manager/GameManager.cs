using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static bool SHOULD_LOAD_STATS = false;

    [SerializeField] private PlayerStats m_player;

    [SerializeField] private GameObject m_gameHud;
    [SerializeField] private GameObject m_deathScreen;

    private bool m_paused = false;

    private void Start()
    {
        if (SHOULD_LOAD_STATS)
        {
            InitialisePlayer();
        }

        m_gameHud.SetActive(true);
        m_deathScreen.SetActive(false);
    }

    public void InitialisePlayer()
    {
        SaveLoad.InitialisePlayer(m_player);
    }

    public void Death()
    {
        m_gameHud.SetActive(false);
        m_deathScreen.SetActive(true);
    }

    public void PauseGame()
    {
        m_paused = !m_paused;

        Time.timeScale = m_paused ? 0f : 1f;

        UIManager.Instance.ShowPauseMenu();
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

    public void RestartGame()
    {
        SaveLoad.LoadLevel(SceneManager.GetActiveScene().name);
    }
}
