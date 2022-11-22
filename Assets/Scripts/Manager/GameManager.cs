using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : Singleton<GameManager>
{
#if PLATFORM_STANDALONE_WIN
    private const int m_frameRate = 144;
#elif UNITY_ANDROID
    public int frameRate = 30;
#endif

    public static bool SHOULD_LOAD_STATS = false;

    [SerializeField] private PlayerStats m_player;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = m_frameRate;
    }

    void Start()
    {
        if (SHOULD_LOAD_STATS)
        {
            InitialiseGame();
        }
    }

    public void InitialiseGame()
    {
        m_player.gameObject.transform.position = SaveLoad.LAST_CHECKPOINT_POSITION;
        m_player.AddPoints(SaveLoad.PLAYER_POINTS);
        m_player.SetHealth(SaveLoad.PLAYER_HEALTH);
    }
}
