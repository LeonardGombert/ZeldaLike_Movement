using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour {

    public TileType [] tileTypes;
    public GameObject selectedUnit;

    int [,] tiles;

    int mapSizeX = 10;
    int mapSizeY = 10;

    // Use this for initialization
    void Start()
    {
        GenerateMapData();
        GenerateMapVisual();
    }

    void GenerateMapData()
    {
        //Allocate Map tiles
        tiles = new int[mapSizeX, mapSizeY];

        //Initialize map floor tiles
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                tiles[x, y] = 0;
            }
        }
        //Let's make a U shaped wall formation

        tiles[4, 4] = 0;
        tiles[5, 4] = 0;
        tiles[6, 4] = 0;
        tiles[7, 4] = 0;
        tiles[8, 4] = 0;

        tiles[4, 4] = 1;
        tiles[4, 4] = 1;
        tiles[8, 4] = 1;
        tiles[8, 4] = 1;
	}

    void GenerateMapVisual()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                TileType tt = tileTypes[ tiles[x,y] ];

                GameObject gameObject = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector2(x, y), Quaternion.identity);

                //Gets the X and Y location of whichever tile is clicked
                ClickableTile ct = gameObject.GetComponent<ClickableTile>(); 
                ct.tileX = x;
                ct.tileY = y;
            }
        }
    }

    public void MoveSelectedUnitTo (int x, int y)
    {

    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
