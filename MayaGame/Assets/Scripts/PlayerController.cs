using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 1.0f;
    public float turnSpeed = 10.0f;
    private float horizontalInput;
    private float verticalInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check if there is a left or right input
        horizontalInput = Input.GetAxis("Horizontal");
        //Rotate the player right or left depending on input
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);

        //Check if there is a up or down input
        verticalInput = Input.GetAxis("Vertical");
        //Rotate the player up or down depending on input
        transform.Rotate(Vector3.left, Time.deltaTime * turnSpeed * verticalInput);

        //Player automatically goes forward
        transform.Translate(Vector3.forward * playerSpeed * Time.deltaTime);
    }
}
