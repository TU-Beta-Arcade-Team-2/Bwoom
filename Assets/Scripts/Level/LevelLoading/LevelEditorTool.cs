using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;


public class LevelEditorTool : MonoBehaviour
{
    public TextAsset LevelXML;
    public TextAsset TileSetXML;
    public SpriteAtlas SpriteAtlas;
}


[CustomEditor(typeof(LevelEditorTool))]
[CanEditMultipleObjects]
public class LevelEditorGUI : Editor
{
    private LevelEditorTool m_target;

    void OnEnable()
    {
        m_target = (LevelEditorTool)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Build Layout!"))
        {
            LoadLevelFromXML levelLoader = new LoadLevelFromXML(m_target.LevelXML.text, m_target.TileSetXML.text, m_target.SpriteAtlas);
            levelLoader.BuildLevel();

            BetterDebugging.Instance.DebugLog("Clicked!", BetterDebugging.eDebugLevel.Message);
        }

        DrawDefaultInspector();
    }
}
