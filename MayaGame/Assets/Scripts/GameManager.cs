using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public bool isGameActive;
    public int foodCount;
    public float timeRemaining = 60;
    private bool timerSoundPlayed = false;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI levelCompletedText;
    public TextMeshProUGUI timeText;
    public GameObject gameOverScreen;
    public GameObject levelCompleteScreen;

    public AudioClip countdownSound;
    public AudioClip completeSound;
    private AudioSource gameAudio;


    // Start is called before the first frame update
    void Start()
    {
        isGameActive = true;
        AddScore(score);
        gameAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive)
        {
            //Count down on clock until 0. When time runs out, the game is over
            //TODO: Make function
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                GameOver();
            }

            if (timeRemaining < 3 && !timerSoundPlayed)
            {
                gameAudio.PlayOneShot(countdownSound, 1.0f);
                timerSoundPlayed = true;
            }

            //When there are no more food objects in level, the level is complete
            foodCount = GameObject.FindGameObjectsWithTag("Food").Length;
            if (foodCount == 0)
            {
                gameAudio.PlayOneShot(completeSound, 1.0f);
                LevelCompleted();
            }
        }
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverScreen.gameObject.SetActive(true);
        isGameActive = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void OpenTitleScreen()
    {
        SceneManager.LoadScene("Title Screen");

    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Stop game and display level complete screen
    public void LevelCompleted()
    {
        levelCompleteScreen.gameObject.SetActive(true);
        isGameActive = false;
    }

    //Display time left in UI
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
