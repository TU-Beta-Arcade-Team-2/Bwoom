using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.U2D;

public class LoadLevelFromXML
{
    private string m_levelXML;
    private string m_tileSetXML;

    private Dictionary<int, string> m_tileSetIds;

    public LoadLevelFromXML(string levelXml, string tileSetXml, SpriteAtlas spriteAtlas, GameObject tilePrefab)
    {
        m_levelXML = levelXml;
        m_tileSetXML = tileSetXml;
        m_tileSetIds = new();
    }

    public void BuildLevel()
    {
        BetterDebugging.Assert(m_levelXML != null, "LevelXML needs to be set! Check the references!");
        BetterDebugging.Assert(m_tileSetXML != null, "LevelXML needs to be set! Check the references!");

        BuildTileSet();

        ParseLevelXML();
    }

    private void BuildTileSet()
    {
        ParseTileSetXML();

        BetterDebugging.Assert(m_tileSetIds.Count != 0, "TILE COUNT SHOULDN'T BE EMPTY!");
    }

    private void ParseTileSetXML()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(m_tileSetXML);

        foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
        {
            if (node.Name.Equals("tile"))
            {
                XmlElement tileElem = (XmlElement)node;

                BetterDebugging.Assert(tileElem != null);

                int id = int.Parse(tileElem.GetAttributeNode("id").InnerXml);

                string @class = tileElem.GetAttributeNode("class").InnerXml;

                m_tileSetIds.Add(id, @class);
            }
        }
    }

    private void ParseLevelXML()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(m_levelXML);

        foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
        {
            if (node.Name.Equals("layer"))
            {
                XmlElement levelElem = (XmlElement)node;

                BetterDebugging.Assert(levelElem != null);

                string layerName = levelElem.GetAttributeNode("name").InnerXml;
                int layer = int.Parse(levelElem.GetAttributeNode("id").InnerXml);
                int width = int.Parse(levelElem.GetAttributeNode("width").InnerXml);
                int height = int.Parse(levelElem.GetAttributeNode("height").InnerXml);

                if (layerName == "debug_ground")
                {
                    string[] tiles = node.InnerText.Split(",");

                    // Create a gameObject to parent each layer to...
                    GameObject parent = Object.Instantiate(new GameObject(layerName), Vector3.zero, Quaternion.identity);

                    for (int i = 0; i < height; i++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            BetterDebugging.Assert(width * i + j < tiles.Length, $"{i}  {j}   {width}    {width * i + j}");

                            if (layerName.Equals("debug_tile_notations"))
                            {
                                BetterDebugging.Log("TILE NOTATION LAYER ISN'T DONE YET...", BetterDebugging.eDebugLevel.Warning);
                            }
                            else
                            {
                                // IDs are stored as longs because TILED is stupid and when a tile gets flipped it becomes a MASSIVE number...
                                // Again, it will have to be a case of 
                                long ID = long.Parse(tiles[width * i + j]) - 1;

                                if (ID != -1) // -1 is air!
                                {
                                    PlaceTile(parent.transform, ID, j, i, height, width, layer, true);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void PlaceTile(Transform parent, long tileID, int row, int column, int width, int height, int renderOrder, bool isCollidable)
    {
        Vector2 tilePosition = new Vector2(row, height - column);

        GameObject tileGameObject = Object.Instantiate(new GameObject($"Collider:{tileID}"), tilePosition, Quaternion.identity, parent);

        if (isCollidable)
        {
            // If it's a one way platform (for now, 18, 19 and 20) apply the correct components...
            if (tileID is 18 or 19 or 20)
            {
                BoxCollider2D collider = tileGameObject.AddComponent<BoxCollider2D>();

                // Make the collider half height
                collider.offset = new Vector2(0f, -0.32f);
                collider.size = new Vector2(1f, 0.3f);

                collider.usedByEffector = true;

                // Add the platform effector component
                PlatformEffector2D effector = tileGameObject.AddComponent<PlatformEffector2D>();

                effector.surfaceArc = 170;
            }
            else if (tileID <= 17 && tileID >= 9)
            {
                // These are invisible walls!
                // spriteRenderer.sprite = null;
                tileGameObject.AddComponent<BoxCollider2D>();
            }
            else
            {
                tileGameObject.AddComponent<BoxCollider2D>();
            }

            tileGameObject.layer = LayerMask.NameToLayer(StringConstants.SURFACE_LAYER);
            tileGameObject.tag = StringConstants.GROUND_TAG;
        }
    }
}
