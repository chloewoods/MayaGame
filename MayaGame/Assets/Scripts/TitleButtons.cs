using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleButtons : MonoBehaviour
{
    public GameObject helpMenu;
    public GameObject levelMenu;
    public GameObject highScoreMenu;

    public TextMeshProUGUI level1Text;
    public TextMeshProUGUI level2Text;
    public TextMeshProUGUI level3Text;
    public TextMeshProUGUI playerText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadFirstLevel()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void LoadSecondLevel()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void Load2Player()
    {
        SceneManager.LoadScene("2 Player");
    }

    public void HelpMenu()
    {
        helpMenu.gameObject.SetActive(true);
    }

    public void ScoreToTime(float highScore, TextMeshProUGUI highScoreText, string level)
    {
        float minutes = Mathf.FloorToInt(highScore / 60);
        float seconds = Mathf.FloorToInt(highScore % 60);
        highScoreText.text = string.Format("{0} = {1:00}:{2:00}", level, minutes, seconds);
    }
    public void HighScoreMenu()
    {
        highScoreMenu.gameObject.SetActive(true);
        float highScore_level1 = PlayerPrefs.GetFloat("highScore_Level 1", 60);
        float highScore_level2 = PlayerPrefs.GetFloat("highScore_Level 2", 60);
        float highScore_level3 = PlayerPrefs.GetFloat("highScore_Level 3", 60);
        float highScore_2player = PlayerPrefs.GetFloat("highScore_2 Player", 60);

        ScoreToTime(highScore_level1, level1Text, "Level 1");
        ScoreToTime(highScore_level2, level2Text, "Level 2");
        ScoreToTime(highScore_level3, level3Text, "Level 3");
        ScoreToTime(highScore_2player, playerText, "2 Player");
    }

    public void LevelMenu()
    {
        levelMenu.gameObject.SetActive(true);
    }

    public void MainMenu()
    {
        helpMenu.gameObject.SetActive(false);
        levelMenu.gameObject.SetActive(false);
        highScoreMenu.gameObject.SetActive(false);
    }
}
