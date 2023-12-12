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
    private float timePassed = 0;
    public float maxTime = 60;
    private bool timerSoundPlayed = false;
    private bool canPause = true;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI levelCompletedText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI timeFinalText;
    public TextMeshProUGUI highScoreText;
    public GameObject gameOverScreen;
    public GameObject levelCompleteScreen;
    public GameObject pauseScreen;

    public AudioClip countdownSound;
    public AudioClip completeSound;
    private AudioSource gameAudio;


    // Start is called before the first frame update
    void Start()
    {
        isGameActive = true;
        gameAudio = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        //Pause game when esc pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        if (isGameActive)
        {
            //Keep track of how long player has been in level, game is over when 1 minute has passed
            TrackTime();

            //When there are no more food objects in level, the level is complete
            foodCount = GameObject.FindGameObjectsWithTag("Food").Length;
            scoreText.text = "Treats = " + foodCount;
            if (foodCount == 0)
            {
                gameAudio.PlayOneShot(completeSound, 1.0f);
                LevelCompleted();
            }

            

        }
    }

    public void TrackTime()
    {
        //Check if time passed is less than max time allowed, then advance clock
        //When max time reached, game over
        //Play a sound when time nearly over
        if (timePassed < maxTime)
        {
            timePassed += Time.deltaTime;
            DisplayTime(timePassed);
        }
        else
        {
            GameOver();
        }

        if (timePassed > maxTime - 3 && !timerSoundPlayed)
        {
            gameAudio.PlayOneShot(countdownSound, 1.0f);
            timerSoundPlayed = true;
        }
    }
    public void GameOver()
    {
        gameOverScreen.gameObject.SetActive(true);
        isGameActive = false;
        canPause = false;
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

    
    public void LevelCompleted()
    {
        //Stop game and display level complete screen
        levelCompleteScreen.gameObject.SetActive(true);
        isGameActive = false;
        canPause = false;
        DisplayFinalTime(timePassed);
        DisplayHighScore(timePassed);
    }

    
    void DisplayTime(float timeToDisplay)
    {
        //Display time left in UI
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void DisplayFinalTime(float timeToDisplay)
    {
        //Display total time taken in seconds and minutes at the end of the level
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeFinalText.text = string.Format("Time = {0:00}:{1:00}", minutes, seconds);
    }

    void DisplayHighScore(float timeTaken)
    {
        // Retreive the high score from player prefs and compare to the time taken this level
        // if time taken is less than high score, set new high score
        // then display time and high score in UI
        float highScore = PlayerPrefs.GetFloat("highScore_" + SceneManager.GetActiveScene().name, 60);
        if (timeTaken < highScore)
        {
            highScore = timeTaken;
            PlayerPrefs.SetFloat("highScore_" + SceneManager.GetActiveScene().name, highScore);

        }
        float minutes = Mathf.FloorToInt(highScore / 60);
        float seconds = Mathf.FloorToInt(highScore % 60);
        highScoreText.text = string.Format("High Score = {0:00}:{1:00}", minutes, seconds);
    }

    void PauseGame()
    {
        //Stop game and display options to restart or main menu
        //canPause bool prevents pausing when level complete or game over happens
        if (isGameActive && canPause)
        {
            isGameActive = false;
            pauseScreen.gameObject.SetActive(true);
        }
        else if (canPause)
        {
            pauseScreen.gameObject.SetActive(false);
            isGameActive=true;
        }
    }
}
