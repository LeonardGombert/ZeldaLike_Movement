using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum DIRECTION { UP, DOWN, LEFT, RIGHT }

public class CharacterControllerGlobal : MonoBehaviour{    //GRID-BASED PLAYER CONTROLLER + GRID POSITION0 + ANIMATOR CONTROLLER BLEND TREE
    private bool canMove = true, moving = false;
    [SerializeField] private int speed = 5;
    private DIRECTION dir = DIRECTION.DOWN;
    private Vector3 pos;

    Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            pos = transform.position;
            move();

            float velocity = Input.GetAxisRaw("Vertical");
            float direction = Input.GetAxisRaw("Horizontal");

            anim.SetFloat("Velocity", velocity);
            anim.SetFloat("Direction", direction);
        }


        if (moving)
        {
            if (transform.position == pos)
            {
                moving = false;
                canMove = true;

                move();
            }

            transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
        }
    }

    private void move()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            dir = DIRECTION.UP;

            canMove = false;
            moving = true;
            pos += Vector3.up;
            Debug.Log("+1");
        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            dir = DIRECTION.DOWN;

            canMove = false;
            moving = true;
            pos += Vector3.down;
            Debug.Log("+1");
        }

        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir = DIRECTION.LEFT;

            canMove = false;
            moving = true;
            pos += Vector3.left;
            Debug.Log("+1");
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            dir = DIRECTION.RIGHT;

            canMove = false;
            moving = true;
            pos += Vector3.right;
            Debug.Log("+1");
        }
    }
}
