using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleButtons : MonoBehaviour
{
    public GameObject helpMenu;
    public GameObject levelMenu;
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

    public void LevelMenu()
    {
        levelMenu.gameObject.SetActive(true);
    }

    public void MainMenu()
    {
        helpMenu.gameObject.SetActive(false);
        levelMenu.gameObject.SetActive(false);
    }
}
