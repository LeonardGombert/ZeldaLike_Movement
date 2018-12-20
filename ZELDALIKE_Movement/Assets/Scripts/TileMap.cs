using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour {

    public TileType [] tileTypes;
    public GameObject selectedUnit;

    int [,] tiles;
    Node[,] graph;

    int mapSizeX = 10;
    int mapSizeY = 10;

    // Use this for initialization
    void Start()
    {
        GenerateMapData();
        GeneratePathFindingGraph();
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
                ct.map = this;
            }
        }
    }

    class Node
    {
        public List<Node> neighbors;

        public Node()
        {
            neighbors = new List<Node>();
        }
    }

    void GeneratePathFindingGraph()
    {
        graph = new Node[mapSizeX, mapSizeY];

        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                //We have a four-way movement system

                if (x > 0)
                {
                    graph[x, y].neighbors.Add(graph[x - 1, y]);
                }

                if (x < mapSizeX-1)
                {
                    graph[x, y].neighbors.Add(graph[x + 1, y]);
                }

                if (y > 0)
                {
                    graph[x, y].neighbors.Add(graph[x, y-1]);
                }

                if (y < mapSizeY-1)
                {
                    graph[x, y].neighbors.Add(graph[x , y+1]);
                }
            }
        }
    }


    public Vector3 TileCoordtoWorldWoord(int x, int y)
    {
        return new Vector3(x, y, 0);
    }

    public void MoveSelectedUnitTo (int x, int y)
    {
        selectedUnit.GetComponent<Unit>().tileX = x;
        selectedUnit.GetComponent<Unit>().tileY = y;
        selectedUnit.transform.position = TileCoordtoWorldWoord(x, y);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
