using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    
    [SerializeField]
    private int gridWidth;
    [SerializeField]
    private int gridLength;
    [SerializeField]
    private GameObject solidBlockPrefab;
    [SerializeField]
    private GameObject softBlockPrefab;
    [Range(0,100)]
    [SerializeField]
    private int softBlockNumber;

    private float AssetPPU;
    private float tileSize;

    private const int extents = 4;

    public static List<Bounds> extentTiles = new List<Bounds>();
    public static List<Bounds> tiles = new List<Bounds>();

    public static List<Bounds> softBlocksPowerups = new List<Bounds>();
    public static List<Bounds> solidBlocks = new List<Bounds>();

    private int indexResetAmount = 0;

    public static List<System.Tuple<Bounds, GameObject>> playerTiles = new List<System.Tuple<Bounds, GameObject>>();

    void Awake()
    {
        AssetPPU = Camera.main.GetComponent<PixelPerfectCamera>().assetsPixelsPerUnit;
        tileSize = 1 / AssetPPU;

        CreateGrid();
        PlaceSolidBlock();
        PlaceSoftBlock();
    }

    /*private void Update()
    {
        foreach(Bounds tile in tiles)
        {
            Vector2 topRight = new Vector2(tile.max.x, tile.max.y);
            Vector2 bottomLeft = new Vector2(tile.min.x, tile.min.y);
            Debug.DrawLine(topRight, bottomLeft, Color.red);
        }

        foreach (Bounds tile in extentTiles)
        {
            Vector2 topRight = new Vector2(tile.max.x, tile.max.y);
            Vector2 bottomLeft = new Vector2(tile.min.x, tile.min.y);
            Debug.DrawLine(topRight, bottomLeft, Color.blue);
        }
    }*/

    private void CreateGrid()
    {
        for (int y = -extents; y < gridWidth + extents; y++)
        {
            for (int x = -extents; x < gridLength + extents; x++)
            {
                Vector2 tilePosition = (Vector2)transform.position + new Vector2(x + (0.5f), -y - (0.5f));
                Bounds tile = new Bounds(tilePosition, Vector2.one);

                if (y < 0 || x < 0 || y > gridWidth-1 || x > gridLength-1) extentTiles.Add(tile);
                else tiles.Add(tile);
            }
        }
    }

    private void PlaceSolidBlock()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            int Y = Mathf.CeilToInt(i / 15);
            int X = i + 1;

            if(Y % 2 == 1 && X % 2 == 1)
            {
                Instantiate(solidBlockPrefab, tiles[i].center, Quaternion.identity, this.transform);
                solidBlocks.Add(tiles[i]);
            }
        }
    }

    private void PlaceSoftBlock()
    {
        solidBlocks.Add(tiles[0]);
        solidBlocks.Add(tiles[14]);
        solidBlocks.Add(tiles[120]);
        solidBlocks.Add(tiles[134]);

        PlaceOnRandomPos(softBlockNumber, softBlockPrefab, this.transform);
    }

    public static void PlaceOnRandomPos(int objNumber, GameObject obj, Transform parent)
    {
        int resetAmount = 0;
        int randomIndex = 0;

        for (int i = 0; i < objNumber + resetAmount; i++)
        {
            randomIndex = Random.Range(0, tiles.Count);

            if (!softBlocksPowerups.Contains(tiles[randomIndex]) && !solidBlocks.Contains(tiles[randomIndex]))
            {
                Instantiate(obj, tiles[randomIndex].center, Quaternion.identity, parent);
                softBlocksPowerups.Add(tiles[randomIndex]);
            }
            else resetAmount++; 
        }
    }

    public static System.Tuple<bool, GameObject> isPosOnPlayerTile(Vector2 position)
    {
        bool isOnTile = false;
        GameObject tile = null;

        for (int i = 0; i < playerTiles.Count; i++)
        {
            if (playerTiles[i].Item1.Contains(position))
            {
                isOnTile = true;
                tile = playerTiles[i].Item2;
                break;
            }
        }

        return System.Tuple.Create(isOnTile, tile);
    }

    public static Vector2 GetTilePosition(Vector2 position, Vector2 direction)
    {
        Vector2 tilePosition = Vector2.zero;

        foreach (Bounds tile in tiles)
        {
            if (tile.Contains(position)) tilePosition = (Vector2)tile.center + direction;
        }

        return tilePosition;
    }

    public static Vector2 GetExtentTilePosition(Vector2 position, Vector2 direction)
    {
        Vector2 tilePosition = Vector2.zero;

        foreach (Bounds tile in extentTiles)
        {
            if (tile.Contains(position)) tilePosition = (Vector2)tile.center + direction;
        }

        return tilePosition;
    }
}
