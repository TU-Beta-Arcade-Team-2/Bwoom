using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class LoadLevelFromXML : MonoBehaviour
{
    public TextAsset LevelXML;
    public TextAsset TileSetXML;

    private Dictionary<int, GameObject> m_level;


    // Start is called before the first frame update
    void Start()
    {
        // TODO: ANYTHING 30 make 19
        ParseLevelXML();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ParseLevelXML()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(LevelXML.text);

        foreach(XmlNode node in xmlDoc.DocumentElement.ChildNodes)
        {
            if (node.Name.Equals("layer"))
            {
                XmlElement levelElem = (XmlElement)node;
                int width = Int32.Parse(levelElem.GetAttributeNode("width").InnerXml);
                int height = Int32.Parse(levelElem.GetAttributeNode("height").InnerXml);

                String[] tiles = node.InnerText.Split(",");

                for(int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        int ID = Int32.Parse(tiles[width * i + j]);
                        if (ID != 0)
                        {
                            BetterDebugging.Instance.DebugLog(ID.ToString(), BetterDebugging.eDebugLevel.Message);
                        }
                    }
                }
            }
        }
    }
}
