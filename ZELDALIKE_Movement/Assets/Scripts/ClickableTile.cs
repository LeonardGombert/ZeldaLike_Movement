﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour {

    public int tileX;
    public int tileY;
    public TileMap map;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void OnMouseUp()
    {
        Debug.Log("Click on " + tileX + " and " + tileY);
        map.MoveSelectedUnitTo(tileX, tileY);
    }
}