using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject ground;
    private GameManager gameManager;
    private Rigidbody playerRb;

    public bool gameOver;
    public bool stunned;

    public float playerSpeed = 1.0f;
    public float turnSpeed = 10.0f;
    private float yBound = 60f;
    private float horizontalInput;
    private float verticalInput;
    public float stunLength = 0.5f;
    public float stunForce = 1.0f;
    public float stableSpeed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        //reference to player rigidbody
        playerRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver && !stunned)
        {
            MovePlayer();
        }

        if (!Input.anyKey && !gameOver && !stunned)
        {
            Vector3 currentRotation = transform.eulerAngles;

            float rotationAmount = transform.localEulerAngles.z;

            currentRotation.z -= rotationAmount;

            if (Vector3.Distance(transform.eulerAngles, currentRotation) > 0.01f)
            {
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(transform.rotation.eulerAngles), Quaternion.Euler(currentRotation), Time.deltaTime * stableSpeed);
            }
            else
            {
                transform.eulerAngles = currentRotation;
            }
            
        }

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
        if (transform.position.y < ground.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, ground.transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            int scoreToAdd = other.gameObject.GetComponent<scoreHolder>().scoreValue;
            gameManager.AddScore(scoreToAdd);
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Game Over");
            gameOver = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            stunned = true;
            Debug.Log("Collided with obstacle");
            playerRb.AddForce(Vector3.down * stunForce, ForceMode.Impulse);
            playerRb.AddForce(Vector3.back * stunForce, ForceMode.Impulse);
            StartCoroutine(StunCountdownRoutine());
        }
    }

    IEnumerator StunCountdownRoutine()
    {
        //wait 0.5 seconds before unstun
        yield return new WaitForSeconds(stunLength);
        playerRb.velocity = Vector3.zero;
        stunned = false;
    }

}
