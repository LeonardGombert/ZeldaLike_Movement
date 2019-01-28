using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Game_Controller_Topdown2D : MonoBehaviour {
    //Used in SetSlots
    List<int> takenSlots = new List<int>();
    //The player prefab that will be used when spawning a player
    [SerializeField]
    private GameObject[] playerPrefabs;
    [SerializeField]
    private Transform[] spawnPoints;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        SetSlots();
	}

    //Assigns a player to controllers pressing A
    void SetSlots() {
        for (int i = 0; i < 4; i++) {
            // Gets the state of the tested slot (from slot 0 to 3)
            PlayerIndex slotTested = (PlayerIndex)i;
            GamePadState slotState = GamePad.GetState(slotTested);
            // if checked is not taken and has a controller pressing A
            if (slotState.Buttons.A == ButtonState.Pressed && takenSlots.Contains(i) == false)
            {
                // Instantiate a player and adds the slot to the takenSlots list
                takenSlots.Add(i);
                GameObject playerInstance = (GameObject)Instantiate(playerPrefabs[i], spawnPoints[i].position, transform.rotation);
                // Sets the controller as the instantiated player's controller
                playerInstance.GetComponent<Player_Input_Topdown2D>().playerIndex = (PlayerIndex)i;
                Debug.Log("Player Nbr(" + i + ") connected");
            }
        }
    }
}
