using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{

    [SerializeField] private AudioSource victorySfx;

    private void OnEnable()
    {
        victorySfx.Play();
    }


    private void Update()
    {
        //ONLY FOR CONTROLLER BUILD
        if (Input.GetButtonDown("ControllerNewGame") && GameController.instance.hasWon && SceneManager.GetActiveScene().name == "MesseChrisScene")
        {
            NewGame();
        }
    }


    public void MainMenuBtn(string mainMenuScene)
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Time.timeScale = 1f;
        AudioListener.pause = false;

        PlayerPrefs.DeleteKey("lastVisitedCpPosX");
        PlayerPrefs.DeleteKey("solvedEncounters");
        PlayerPrefs.DeleteKey("isDimensionshiftUnlocked");
    }
}
