using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour
{
    public void LoadLevelOne() {
        SceneManager.LoadScene(1);
    }

    public void LoadLevelTwo() {
        SceneManager.LoadScene(2);
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }

    public void ExitGame() {
        Application.Quit();
        Debug.Log("Exit Game");
    }
}
