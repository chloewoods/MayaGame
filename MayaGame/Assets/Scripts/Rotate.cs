using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float turnSpeed = 10.0f;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Objects should only rotate when game is active
        if (!gameManager.gameOver)
        {
            transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed);
        }
        
    }
}
