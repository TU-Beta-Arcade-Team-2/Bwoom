using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveLoad
{
    private static string m_sceneName;
    private static Vector3 m_lastCheckpointPosition;
    private static int m_playerHealth;
    private static int m_playerPoints;

    public static string SCENE_NAME => m_sceneName;

    public static Vector3 LAST_CHECKPOINT_POSITION => m_lastCheckpointPosition;

    public static int PLAYER_HEALTH => m_playerHealth;

    public static int PLAYER_POINTS => m_playerPoints;


#if PLATFORM_STANDALONE_WIN
    private static string BWOOM_DIRECTORY = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Bwoom";
#elif UNITY_ANDROID
    private static string BWOOM_DIRECTORY = "";
#endif

    private static string SAVE_PATH = $"{BWOOM_DIRECTORY}\\save.savegame";

    public static void SaveGame(PlayerStats playerStats)
    {
#if PLATFORM_STANDALONE_WIN
        if (!Directory.Exists(BWOOM_DIRECTORY))
        {
            Directory.CreateDirectory(BWOOM_DIRECTORY);
        }
#elif UNITY_ANDROID
        // TODO: Come up with a nice system for Android too! 
        string bwoomDirectory = "";
#endif
        FileStream outStream = File.Create($"{BWOOM_DIRECTORY}\\save.savegame");
        StreamWriter writer = new StreamWriter(outStream);

        // LEVEL NAME
        writer.WriteLine(playerStats.GetCurrentLevelName());
        // CHECKPOINT
        writer.WriteLine(playerStats.GetLastCheckpointPosition());
        // HEALTH
        writer.WriteLine(playerStats.GetHealth());
        // POINTS
        writer.WriteLine(playerStats.GetPoints());

        writer.Close();
        outStream.Close();
    }


    public static void LoadGame()
    {
#if PLATFORM_STANDALONE_WIN
        // Check if the Bwoom directory exists
        string bwoomDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Bwoom";

        BetterDebugging.Instance.Assert(Directory.Exists(bwoomDirectory), "NO SAVE GAME FOUND!");

#elif UNITY_ANDROID
        // TODO: Come up with a nice system for Android too! 
        string bwoomDirectory = "";
#endif

        FileStream inStream = new FileStream(SAVE_PATH, FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(inStream);

        m_sceneName = reader.ReadLine();

        // vec3s get written out as (x, y, z) so will need to do some processing to read the values
        string checkpointPosition = reader.ReadLine().Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty);
        string[] vectorValues = checkpointPosition.Split(",");

        m_lastCheckpointPosition = new Vector3(
            float.Parse(vectorValues[0]),
            float.Parse(vectorValues[1]),
            float.Parse(vectorValues[2])
            );



        m_playerHealth = int.Parse(reader.ReadLine());
        m_playerPoints = int.Parse(reader.ReadLine());


        reader.Close();
        inStream.Close();


        BetterDebugging.Instance.Assert(!string.IsNullOrEmpty(SCENE_NAME), $"SCENE NAME FROM SAVEDATA WAS NULL OR EMPTY... DID THE FILE SAVE OKAY?\nLOCATION: {SAVE_PATH}");
        LoadLevel(SCENE_NAME);
    }

    public static void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static bool DoesSaveGameExist()
    {
        return File.Exists(SAVE_PATH);
    }
}
