using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveLoad
{
    public enum eSaveLoadOptions
    {
        GameData,
        OptionsData
    }

    private static GameData m_gameData = new GameData(Vector3.zero, 0, 0);

    private static OptionsData m_optionsData = new OptionsData(OptionsData.Defaults);


#if PLATFORM_STANDALONE_WIN
    private static string BWOOM_DIRECTORY = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Bwoom";
#elif UNITY_ANDROID
    private static string BWOOM_DIRECTORY = Application.persistentDataPath + "\\Bwoom";
#endif

    private static readonly string m_savePath = $"{BWOOM_DIRECTORY}\\save.savegame";
    private static readonly string m_optionsPath = $"{BWOOM_DIRECTORY}\\options.properties";


    public static void SaveGame(GameData gameData)
    {
        m_gameData = gameData;

        Save(eSaveLoadOptions.GameData);
    }

    public static void SaveOptions(OptionsData optionsData)
    {
        m_optionsData = optionsData;

        Save(eSaveLoadOptions.OptionsData);
    }

    private static void Save(eSaveLoadOptions type)
    {
        if (!Directory.Exists(BWOOM_DIRECTORY))
        {
            Directory.CreateDirectory(BWOOM_DIRECTORY);
        }

        FileStream outStream = File.Create(GetSaveDataPath(type));
        StreamWriter writer = new StreamWriter(outStream);

        GetSerialisable(type).Serialise(writer);

        writer.Close();
        outStream.Close();
    }

    public static void LoadGame()
    {
        Load(eSaveLoadOptions.GameData);

        BetterDebugging.Assert(!string.IsNullOrEmpty(m_gameData.SceneName), $"SCENE NAME FROM SAVEDATA WAS NULL OR EMPTY... DID THE FILE SAVE OKAY?\nLOCATION: {m_savePath}");
        LoadLevel(m_gameData.SceneName);
    }

    public static OptionsData LoadOptions()
    {
        Load(eSaveLoadOptions.OptionsData);

        return m_optionsData;
    }


    private static void Load(eSaveLoadOptions type)
    {
        string saveDataPath = GetSaveDataPath(type);

        BetterDebugging.Assert(File.Exists(saveDataPath), $"NO SAVE DATA FOUND AT: {saveDataPath}");

        if (!File.Exists(saveDataPath) && type == eSaveLoadOptions.OptionsData)
        {
            SaveOptions(OptionsData.Defaults);
        }

        FileStream inStream = new FileStream(saveDataPath, FileMode.Open, FileAccess.Read);
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
                return m_optionsData;
            default:
                BetterDebugging.Assert(false, "UNHANDLED SERIALISED DATA ");
                return null;
        }
    }

    private static string GetSaveDataPath(eSaveLoadOptions type)
    {
        switch (type)
        {
            case eSaveLoadOptions.GameData:
                return m_savePath;
            case eSaveLoadOptions.OptionsData:
                return m_optionsPath;
            default:
                BetterDebugging.Assert(false, "UNHANDLED SERIALISED DATA ");
                return null;
        }
    }

    public static void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static bool DoesSaveGameExist()
    {
        return File.Exists(m_savePath);
    }

    public static void InitialisePlayer(PlayerStats player)
    {
        player.gameObject.transform.position = m_gameData.LastCheckpointPosition;
        player.AddPoints(m_gameData.PlayerPoints);
        player.SetHealth(m_gameData.PlayerHealth);
    }
}
