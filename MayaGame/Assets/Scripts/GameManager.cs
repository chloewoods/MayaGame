using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    public bool gameOver;
    public int foodCount;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI levelCompletedText;
    public Button restartButton;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        gameOver = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void LevelCompleted()
    {
        levelCompletedText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        gameOver = true;
    }
}
