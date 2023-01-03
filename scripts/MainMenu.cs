using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuUI;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Demoo");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
