using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
#if PLATFORM_STANDALONE_WIN
    private const int m_frameRate = 144;
#elif UNITY_ANDROID
    public int m_frameRate = 30;
#endif

    public static bool SHOULD_LOAD_STATS = false;

    [SerializeField] private PlayerStats m_player;

    [SerializeField] private GameObject m_gameHud;
    [SerializeField] private GameObject m_deathScreen;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = m_frameRate;

        if (SHOULD_LOAD_STATS)
        {
            InitialisePlayer();
        }

        m_gameHud.SetActive(true);
        m_deathScreen.SetActive(false);
    }

    public void InitialisePlayer()
    {
        m_player.gameObject.transform.position = SaveLoad.LAST_CHECKPOINT_POSITION;
        m_player.AddPoints(SaveLoad.PLAYER_POINTS);
        m_player.SetHealth(SaveLoad.PLAYER_HEALTH);
    }

    public void Death()
    {
        m_gameHud.SetActive(false);
        m_deathScreen.SetActive(true);
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
