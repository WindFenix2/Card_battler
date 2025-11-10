using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string battleSelectScene;

    private void Start()
    {
        AudioManager.instance.PlayMenuMusic();
    }


    public void StartGame()
    {
        SceneManager.LoadScene(battleSelectScene);

        AudioManager.instance.PlaySFX(0);
    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Quitting Game");

        AudioManager.instance.PlaySFX(0);
    }
}
