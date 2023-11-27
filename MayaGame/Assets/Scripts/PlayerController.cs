using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string inputID;
    
    public GameObject ground;
    private GameManager gameManager;
    private Rigidbody playerRb;

    public float playerSpeed = 12.0f;
    public float turnSpeed = 10.0f;
    public float stableSpeed = 3.0f;

    private float horizontalInput;
    private float verticalInput;
    private float yBound = 40f;
    public float xzBound = 80f;

    private bool stunned;
    public float stunLength = 0.5f;
    public float stunForce = 1.0f;

    public AudioClip eatSound;
    public AudioClip bumpSound;
    public AudioClip dieSound;
    public AudioClip speedSound;
    private AudioSource playerAudio;

    public ParticleSystem stunEffect;
    public ParticleSystem wingSpeed;
    
    

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Speed Boost Controller
        // We change the player speed when input is held down
        // The input required changes for 2 player

        //In single player or for player 1. When space bar is pressed, speed is increased
        //TODO: can we make this a function - will require a float return
        if (inputID == "1" || inputID == "0") 
        {
            if (Input.GetKey(KeyCode.Space))
            {
                playerSpeed = 24;

            }
            else
            {
                playerSpeed = 12;
                wingSpeed.Stop();
            }

            //SFX and VFX play
            if (Input.GetKeyDown(KeyCode.Space))
            {
                wingSpeed.Play();
                playerAudio.PlayOneShot(speedSound, 1.0f);
            }
        }

        // If using player 2, Right Control activates speed increase
        if (inputID == "2")
        {
            if (Input.GetKey(KeyCode.RightControl))
            {
                playerSpeed = 24;

            }
            else
            {
                playerSpeed = 12;
                wingSpeed.Stop();
            }

            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                wingSpeed.Play();
                playerAudio.PlayOneShot(speedSound, 1.0f);
            }
        }

        //While game is active, player is moving forward

        if (gameManager.isGameActive && !stunned)
        {
            MovePlayer();
        }


        // When player stops turning, bring player back to neutral position
        if (Input.GetAxis("Horizontal") == 0 && gameManager.isGameActive && !stunned)
        {
            SteadyPlayer();
        }

        //Prevent player from leaving the level
        ConstrainPlayerPosition();
    }

    //Moves the player based on arrow input
    void MovePlayer()
    {
        //Player automatically goes forward
        transform.Translate(Vector3.forward * playerSpeed * Time.deltaTime);
        //Check if there is a left or right input
        horizontalInput = Input.GetAxis("Horizontal"+ inputID);
        //Rotate the player right or left depending on input
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);

        //Check if there is a up or down input
        verticalInput = Input.GetAxis("Vertical" + inputID);
        //Rotate the player up or down depending on input
        transform.Rotate(Vector3.left, Time.deltaTime * turnSpeed * verticalInput);
    }

    //Prevent the player from leaving level
    void ConstrainPlayerPosition()
    {
        // Prevent player from leaving through sky
        if (transform.position.y > yBound)
        {
            transform.position = new Vector3(transform.position.x, yBound, transform.position.z);
        }

        //Prevent player from going through ground
        if (transform.position.y < ground.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, ground.transform.position.y, transform.position.z);
        }

        //Prevent player from leaving x and z bounds

        if (transform.position.z > xzBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, xzBound);
        }

        if (transform.position.z < -xzBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -xzBound);
        }

        if (transform.position.x > xzBound)
        {
            transform.position = new Vector3(xzBound, transform.position.y, transform.position.z);
        }

        if (transform.position.x < -xzBound)
        {
            transform.position = new Vector3(-xzBound, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If object is food, eat and add score
        if (other.gameObject.CompareTag("Food"))
        {
            playerAudio.PlayOneShot(eatSound, 1.0f);
            Destroy(other.gameObject);
            int scoreToAdd = other.gameObject.GetComponent<scoreHolder>().scoreValue;
            gameManager.AddScore(scoreToAdd);
        }

        // If player hits ground, game over
        if (other.gameObject.CompareTag("Ground"))
        {
            playerAudio.PlayOneShot(dieSound, 1.0f);
            gameManager.GameOver();
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If player collides with obstacle, push player back and down
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            playerAudio.PlayOneShot(bumpSound, 1.0f);
            stunEffect.Play();
            stunned = true;
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

    //Return player back to a neutral position
    void SteadyPlayer()
    {
        //Find current z component rotation
        Vector3 currentRotation = transform.eulerAngles;
        float rotationAmount = transform.localEulerAngles.z;
        
        //Target rotation is z=0
        currentRotation.z -= rotationAmount;

        if (Vector3.Distance(transform.eulerAngles, currentRotation) > 0.01f)
        {
            //Return player's z rotation component to zero over time
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(transform.rotation.eulerAngles), Quaternion.Euler(currentRotation), Time.deltaTime * stableSpeed);
        }
        else
        {
            transform.eulerAngles = currentRotation;
        }
    }

}
