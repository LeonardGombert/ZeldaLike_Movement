using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure;


public class Player_Input_Topdown2D : MonoBehaviour {
    // Setting up references
    LPlayer player;

    // Xinput slot. Slot set when instantiated. state and prevState used to check controls
    public PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    // Stores Sticks and Triggers States
    Vector2 DPad;
    Vector2 leftStick = new Vector2();
    Vector2 rightStick = new Vector2();
    Vector2 moveInput;

    float leftTrigger;
    float rightTrigger;

    bool rightTriggerOnce;

    // Use this for initialization
    void Start () {
        player = GetComponent<LPlayer>();
	}


    bool firstMove;
	// Update is called once per frame
	void Update () {
        // Used to check the Gamepad state and previous State
        prevState = state;
        state = GamePad.GetState(playerIndex);

        // Stores Sticks/Triggers/DPad
        DPad.x = state.DPad.Left == ButtonState.Pressed ? -1 :
                 state.DPad.Right == ButtonState.Pressed ? 1 : 0;
        DPad.y = state.DPad.Down == ButtonState.Pressed ? -1 :
                 state.DPad.Up == ButtonState.Pressed ? 1 : 0;

        leftStick = new Vector2(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
        rightStick = new Vector2(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);

        leftTrigger = state.Triggers.Left;
        rightTrigger = state.Triggers.Right;

        // Actions
        moveInput = DPad != Vector2.zero ? DPad : leftStick;

        // Call functions inside the if statement to set what should be executed when pressed
        // A Button
        if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
        {
            Debug.Log("Xbox Controller : Button A Pressed");
        }
        // X Button
        if (prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed)
        {
            Debug.Log("Xbox Controller : Button X Pressed");
            player.SwitchTile(0);
        }
        // Y Button
        if (prevState.Buttons.Y == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed)
        {
            Debug.Log("Xbox Controller : Button Y Pressed");
            player.SwitchTile(1);
        }
        // B Button
        if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed)
        {
            Debug.Log("Xbox Controller : Button B Pressed");
            player.SwitchTile(2);
        }
        // LB Button
        if (prevState.Buttons.LeftShoulder == ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed)
        {
            Debug.Log("Xbox Controller : Button LB Pressed");
        }
        // RB Button
        if (prevState.Buttons.RightShoulder == ButtonState.Released && state.Buttons.RightShoulder == ButtonState.Pressed)
        {
            Debug.Log("Xbox Controller : Button RB Pressed");
            player.PlaceTile(false);
            player.firstTilePlaced = true;
        }
        
        if(rightTrigger == 1)
        {
            if(rightTriggerOnce == false)
            {
                Debug.Log("Right Trigger Pressed");

                if(!player.firstTilePlaced && player.aimDirection == Vector2.zero)
                {
                    for (int i = 0; i < BombParameter.directions.Length; i++)
                    {
                        Vector2 possiblePlacement = Grid.GetTilePosition((Vector2)player.transform.position + BombParameter.directions[i], Vector2.zero);
                        if(possiblePlacement != Vector2.zero)
                        {
                            player.aimDirection = possiblePlacement - Grid.GetTilePosition(player.transform.position, Vector2.zero);
                            break;
                        }
                    }
                }
                player.firstTilePlaced = true;
                player.PlaceTile(true);

                rightTriggerOnce = true;
            }
        }
        else rightTriggerOnce = false;

        // Start Button
        if (prevState.Buttons.Start == ButtonState.Released && state.Buttons.Start == ButtonState.Pressed)
        {
            Debug.Log("Xbox Controller : Button Start Pressed");
            player.Eject(3, player.aimDirection);
        }
    }

    private void FixedUpdate()
    {
        player.Move(moveInput);
        player.AimInput(rightStick);
        if (player.ejected) player.Move(player.direction, true);
    }
}
