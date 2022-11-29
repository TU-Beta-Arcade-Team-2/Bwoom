using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : Serialisable
{
    private string m_sceneName;
    private Vector3 m_lastCheckpointPosition;
    private int m_playerHealth;
    private int m_playerPoints;
    public string SceneName => m_sceneName;
    public Vector3 LastCheckpointPosition => m_lastCheckpointPosition;
    public int PlayerHealth => m_playerHealth;
    public int PlayerPoints => m_playerPoints;

    public GameData(Vector3 lastCheckpointPosition, int playerHealth, int playerPoints)
    {
        m_sceneName = SceneManager.GetActiveScene().name;
        m_lastCheckpointPosition = lastCheckpointPosition;
        m_playerHealth = playerHealth;
        m_playerPoints = playerPoints;
    }

    public override void Serialise(StreamWriter writer)
    {
        // LEVEL NAME
        writer.WriteLine(m_sceneName);
        // CHECKPOINT
        writer.WriteLine(m_lastCheckpointPosition);
        // HEALTH
        writer.WriteLine(m_playerHealth);
        // POINTS
        writer.WriteLine(m_playerPoints);
    }

    public override void Deserialise(StreamReader reader)
    {
        BetterDebugging.Assert(reader != null);

        m_sceneName = reader.ReadLine();

        // vec3s get written out as (x, y, z) so will need to do some processing to read the values
        string checkpointPosition = reader.ReadLine().Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty);

        BetterDebugging.Assert(!string.IsNullOrEmpty(checkpointPosition), "PLAYER POSITION NULL FROM FILE!");

        string[] vectorValues = checkpointPosition.Split(",");

        m_lastCheckpointPosition = new Vector3(
            float.Parse(vectorValues[0]),
            float.Parse(vectorValues[1]),
            float.Parse(vectorValues[2])
        );

        m_playerHealth = int.Parse(reader.ReadLine() ?? throw new NullReferenceException());
        m_playerPoints = int.Parse(reader.ReadLine() ?? throw new InvalidOperationException());
    }
}