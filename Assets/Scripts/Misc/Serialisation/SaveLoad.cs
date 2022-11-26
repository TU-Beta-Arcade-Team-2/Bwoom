using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization;


public class SaveLoad
{
    public enum eSaveLoadOptions
    {
        GameData,
        OptionsData
    }

    private static GameData m_gameData = new GameData(Vector3.zero, 0, 0);


#if PLATFORM_STANDALONE_WIN
    private static string BWOOM_DIRECTORY = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Bwoom";
#elif UNITY_ANDROID
    private static string BWOOM_DIRECTORY = "";
#endif

    private static string SAVE_PATH = $"{BWOOM_DIRECTORY}\\save.savegame";


    public static void SaveGame(GameData gd)
    {
        m_gameData = gd;

        Save(eSaveLoadOptions.GameData);
    }

    private static void Save(eSaveLoadOptions type)
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
        FileStream outStream = File.Create(GetSaveDataPath(type));
        StreamWriter writer = new StreamWriter(outStream);

        GetSerialisable(type).Serialise(writer);

        writer.Close();
        outStream.Close();
    }

    public static void LoadGame()
    {
        Load(eSaveLoadOptions.GameData);

        BetterDebugging.Instance.Assert(!string.IsNullOrEmpty(m_gameData.SceneName), $"SCENE NAME FROM SAVEDATA WAS NULL OR EMPTY... DID THE FILE SAVE OKAY?\nLOCATION: {SAVE_PATH}");
        LoadLevel(m_gameData.SceneName);
    }

    public static void LoadOptions()
    {

    }


    private static void Load(eSaveLoadOptions type)
    {
#if PLATFORM_STANDALONE_WIN
        // Check if the Bwoom directory exists
        string bwoomDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Bwoom";

        BetterDebugging.Instance.Assert(Directory.Exists(bwoomDirectory), "NO SAVE GAME FOUND!");

#elif UNITY_ANDROID
        // TODO: Come up with a nice system for Android too! 
        string bwoomDirectory = "";
#endif
        FileStream inStream = new FileStream(GetSaveDataPath(type), FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(inStream);

        GetSerialisable(type).Deserialise(reader);

        reader.Close();
        inStream.Close();
    }

    private static Serialisable GetSerialisable(eSaveLoadOptions type)
    {
        switch (type)
        {
            case eSaveLoadOptions.GameData:
                return m_gameData;
            case eSaveLoadOptions.OptionsData:
                return null;
            default:
                BetterDebugging.Instance.Assert(false, "UNHANDLED SERIALISED DATA ");
                return null;
        }
    }

    private static string GetSaveDataPath(eSaveLoadOptions type)
    {
        switch (type)
        {
            case eSaveLoadOptions.GameData:
                return SAVE_PATH;
            case eSaveLoadOptions.OptionsData:
                return "";
            default:
                BetterDebugging.Instance.Assert(false, "UNHANDLED SERIALISED DATA ");
                return null;
        }
    }

    public static void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static bool DoesSaveGameExist()
    {
        return File.Exists(SAVE_PATH);
    }

    public static void InitialisePlayer(PlayerStats player)
    {
        player.gameObject.transform.position = m_gameData.LastCheckpointPosition;
        player.AddPoints(m_gameData.PlayerPoints);
        player.SetHealth(m_gameData.PlayerHealth);
    }
}
