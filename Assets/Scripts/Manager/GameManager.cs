using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static bool SHOULD_LOAD_SAVE = false;

    [SerializeField] private PlayerStats m_player;

    [SerializeField] private GameObject m_gameHud;
    [SerializeField] private GameObject m_deathScreen;

    private bool m_paused = false;

    protected override void InternalInit()
    {
        if (SHOULD_LOAD_SAVE)
        {
            InitialisePlayer();
        }

        SoundManager.Instance.PlayMusic(StringConstants.NATURE_LEVEL_SOUNDTRACK, true);

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

    public void OnPauseButtonPressed()
    {
        UIManager.Instance.OnPauseButtonPressed();

        m_paused = UIManager.Instance.IsStillPaused();
        Time.timeScale = m_paused ? 0f : 1f;
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
        OnPauseButtonPressed();
        SHOULD_LOAD_SAVE = false;
        SaveLoad.LoadLevel(SceneManager.GetActiveScene().name);
    }
}
