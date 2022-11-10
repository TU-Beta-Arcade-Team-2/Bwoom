using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.U2D;

public class LoadLevelFromXML
{
    private string m_levelXML;
    private string m_tileSetXML;

    private Dictionary<int, string> m_tileSetIds;

    private SpriteAtlas m_spriteAtlas;

    private Dictionary<int, GameObject> m_tiles;

    public LoadLevelFromXML(string levelXml, string tileSetXml, SpriteAtlas spriteAtlas)
    {
        m_levelXML = levelXml;
        m_tileSetXML = tileSetXml;
        m_spriteAtlas = spriteAtlas;

        m_tileSetIds = new();
        m_tiles = new();
    }

    public void BuildLevel()
    {
        BetterDebugging.Instance.Assert(m_levelXML != null, "LevelXML needs to be set! Check the references!");
        BetterDebugging.Instance.Assert(m_tileSetXML != null, "LevelXML needs to be set! Check the references!");
        BetterDebugging.Instance.Assert(m_spriteAtlas != null, "spriteAtlas needs to be set! Check the references!");


        BuildTileSet();

        // TODO: ANYTHING 30 make 19
        ParseLevelXML();
    }

    private void BuildTileSet()
    {
        ParseTileSetXML();

        BetterDebugging.Instance.Assert(m_spriteAtlas.spriteCount > 0, "Texture atlas shouldn't be empty! Double check reference is assigned!");

        for (int i = 0; i < m_spriteAtlas.spriteCount; i++)
        {
            GameObject newTile = new GameObject(m_tileSetIds[i]);

            newTile.AddComponent<SpriteRenderer>().sprite = m_spriteAtlas.GetSprite(m_tileSetIds[i]);

            m_tiles.Add(i, newTile);
        }
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

                BetterDebugging.Instance.Assert(tileElem != null);

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

                BetterDebugging.Instance.Assert(levelElem != null);

                string layerName = levelElem.GetAttributeNode("name").InnerXml;
                int layer = int.Parse(levelElem.GetAttributeNode("id").InnerXml);
                int width = int.Parse(levelElem.GetAttributeNode("width").InnerXml);
                int height = int.Parse(levelElem.GetAttributeNode("height").InnerXml);

                string[] tiles = node.InnerText.Split(",");

                // Create a gameObject to parent each layer to...
                GameObject parent = Object.Instantiate(new GameObject(layerName), Vector3.zero, Quaternion.identity);

                for (int i = 0; i < height - 1; i++)
                {
                    for (int j = 0; j < width - 1; j++)
                    {
                        BetterDebugging.Instance.Assert(width * i + j < tiles.Length, $"{i}  {j}   {width}    {width * i + j}");

                        if (layerName.Equals("debug_tile_notations"))
                        {
                            BetterDebugging.Instance.DebugLog("TILE NOTATION LAYER ISN'T DONE YET...", BetterDebugging.eDebugLevel.Warning);
                        }
                        else
                        {
                            long subAmount = 0;
                            bool canCollide = false;

                            switch (layerName)
                            {
                                // Because we have more than 1 tileset, we want to subtract the "gid" field from the appropriate one...
                                // Hardcoding for now because I really CBA at the moment and just want this working. Future Tom can 
                                // shout at me but present Tom is very very tired
                                case "debug_ground":
                                    subAmount = 1L;
                                    canCollide = true;
                                    break;
                                case "nature_ground":
                                    // Skipping nature_ground for now
                                    subAmount = 34L;
                                    //TODO: Remove skip
                                    continue;
                            }

                            // IDs are stored as longs because TILED is stupid and when a tile gets flipped it becomes a MASSIVE number...
                            // Again, it will have to be a case of 
                            long ID = long.Parse(tiles[width * i + j]) - subAmount;

                            if (ID != -1) // -1 is air!
                            {
                                PlaceTile(parent.transform, ID, j, i, height, width, layer, canCollide);
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

        GameObject tile = null;

        try
        {
            tile = m_tiles[(int)tileID];
        }
        catch (KeyNotFoundException)
        {
            tile = m_tiles[0];
        }

        GameObject tileGameObject = Object.Instantiate(tile, tilePosition, Quaternion.identity, parent);
        
        tileGameObject.GetComponent<SpriteRenderer>().sortingOrder = renderOrder;
        
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
            else
            {
                tileGameObject.AddComponent<BoxCollider2D>();
            }

            tileGameObject.layer = LayerMask.NameToLayer(StringConstants.SURFACE_LAYER);
            tileGameObject.tag = StringConstants.GROUND_TAG;
        }
    }
}
