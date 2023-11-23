using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    public bool isGameActive;
    public int foodCount;
    private float timeRemaining = 60;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI levelCompletedText;
    public TextMeshProUGUI timeText;
    public GameObject gameOverScreen;
    public GameObject levelCompleteScreen;


    // Start is called before the first frame update
    void Start()
    {
        isGameActive = true;
        AddScore(score);
    }

    // Update is called once per frame
    void Update()
    {
        foodCount = GameObject.FindGameObjectsWithTag("Food").Length;
        if (foodCount == 0)
        {
            LevelCompleted();
        }

        if (isGameActive)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                GameOver();
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

    public void LevelCompleted()
    {
        levelCompleteScreen.gameObject.SetActive(true);
        isGameActive = false;
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
