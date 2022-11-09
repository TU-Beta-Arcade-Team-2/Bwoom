using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.U2D;

public class LoadLevelFromXML : MonoBehaviour
{
    public TextAsset LevelXML;
    public TextAsset TileSetXML;

    private Dictionary<int, string> m_tileSetIds = new();


    [SerializeField] private SpriteAtlas m_spriteAtlas;

    private Dictionary<int, GameObject> m_tiles = new();
    [SerializeField] private GameObject m_tilePrefab;


    // Start is called before the first frame update
    void Start()
    {
        BuildTileSet();

        // TODO: ANYTHING 30 make 19
        ParseLevelXML();
    }

    // Update is called once per frame
    void Update()
    {

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
        xmlDoc.LoadXml(TileSetXML.text);

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
        xmlDoc.LoadXml(LevelXML.text);

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
                GameObject parent = Instantiate(new GameObject(layerName), Vector3.zero, Quaternion.identity, transform);

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

        GameObject tileGameObject = Instantiate(tile, tilePosition, Quaternion.identity, parent);

        tileGameObject.GetComponent<SpriteRenderer>().sortingOrder = renderOrder;

        if (isCollidable)
        {
            tileGameObject.AddComponent<BoxCollider2D>();
        }
    }
}
