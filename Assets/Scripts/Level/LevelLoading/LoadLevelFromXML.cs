using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.U2D;

public class LoadLevelFromXML : MonoBehaviour
{
    public TextAsset LevelXML;

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
        Sprite[] sprites = new Sprite[m_spriteAtlas.spriteCount];

        int amount = m_spriteAtlas.GetSprites(sprites);

        BetterDebugging.Instance.Assert(amount > 0, "Texture atlas shouldn't be empty! Double check reference is assigned!");

        for (int i = 0; i < sprites.Length; i++)
        {
            GameObject newTile = new GameObject
            {
                name = sprites[i].name,
                transform =
                {
                    parent = transform
                }
            };

            newTile.AddComponent<SpriteRenderer>().sprite = sprites[i];

            m_tiles.Add(i, newTile);
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
                int layer = int.Parse(levelElem.GetAttributeNode("id").InnerXml);
                int width = int.Parse(levelElem.GetAttributeNode("width").InnerXml);
                int height = int.Parse(levelElem.GetAttributeNode("height").InnerXml);

                string[] tiles = node.InnerText.Split(",");

                for (int i = 0; i < height - 1; i++)
                {
                    for (int j =  0; j < width - 1; j++)
                    {
                        BetterDebugging.Instance.Assert(width * i + j < tiles.Length, $"{i}  {j}   {width}    {width * i + j}");

                        long ID = long.Parse(tiles[width * i + j]);

                        if (ID != 0)
                        {
                            PlaceTile(ID, j, i, height, width, layer, false);
                        }
                    }
                }
            }
        }
    }

    private void PlaceTile(long tileID, int row, int column, int width, int height, int renderOrder, bool isCollidable)
    {
        Vector2 tilePosition = new Vector2(row, height - column);

        GameObject tile = null;

        try
        {
            tile = m_tiles[(int) tileID];
        }
        catch (KeyNotFoundException)
        {
            tile = m_tiles[0];
        }

        GameObject tileGameObject = Instantiate(tile, tilePosition, Quaternion.identity, transform);

        tileGameObject.GetComponent<SpriteRenderer>().sortingOrder = renderOrder;

        if (isCollidable)
        {
            tileGameObject.AddComponent<BoxCollider2D>();
        }
    }
}
