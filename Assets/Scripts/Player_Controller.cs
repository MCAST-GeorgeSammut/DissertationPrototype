using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    Rigidbody2D body;
    public GameManager gameManager;

    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;

    public float runSpeed = 20.0f;

    public MazeGenerator mazeGen;


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EndPoint"))
        {
            Destroy(collision.gameObject);
            //GameObject maze = GameObject.Find("MazeObject");
            //Destroy(maze);
            transform.position = new Vector3(0, 0,-1);

            gameManager.AdjustDifficulty();

        }
        else if (collision.gameObject.CompareTag("Door"))
        {
            mazeGen.YellowDoorActive = false;
        }
        else if (collision.gameObject.CompareTag("key"))
        {
            mazeGen.YellowKeyActive = false;
        }
    }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        mazeGen = GameObject.Find("MazeObject").GetComponent<MazeGenerator>();
    }

    void Update()
    {
        // Gives a value between -1 and 1
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down
    }

    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }

}

