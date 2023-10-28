using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 1.0f;
    public float turnSpeed = 10.0f;
    private float yBound = 70f;
    private float horizontalInput;
    private float verticalInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        ConstrainPlayerPosition();
    }

    //Moves the player based on arrow input
    void MovePlayer()
    {
        //Player automatically goes forward
        transform.Translate(Vector3.forward * playerSpeed * Time.deltaTime);
        //Check if there is a left or right input
        horizontalInput = Input.GetAxis("Horizontal");
        //Rotate the player right or left depending on input
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);

        //Check if there is a up or down input
        verticalInput = Input.GetAxis("Vertical");
        //Rotate the player up or down depending on input
        transform.Rotate(Vector3.left, Time.deltaTime * turnSpeed * verticalInput);
    }

    //Prevent the player from leaving level
    void ConstrainPlayerPosition()
    {
        if (transform.position.y > yBound)
        {
            transform.position = new Vector3(transform.position.x, yBound, transform.position.z);
        }
    }
}
