using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSelectButton : MonoBehaviour
{
    public string levelToLoad;

    private void Start()
    {
        AudioManager.instance.PlayBattleSelectMusic();
    }

    public void SelectBattle()
    {
        SceneManager.LoadScene(levelToLoad);

        AudioManager.instance.PlaySFX(0);
    }
}
