using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button continueBtn;


    private void Start()
    {
        PauseMenu.isPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    private void Update()
    {
        if (PlayerPrefs.HasKey("lastVisitedCpPosX"))
        {
            continueBtn.interactable = true;
        }
        else //if you start the game for the first time
        {
            continueBtn.interactable = false;
        }
    }

    public void ContinueBtn(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void NewGameBtn(string sceneToLoad)
    {
        //PlayerPrefs.DeleteAll(); //if we do this the audio keys will not survive and the audio sliders will not be set in the game scene
        PlayerPrefs.DeleteKey("lastVisitedCpPosX"); //instead we delete only one key and use it as a condition in the gamecontroller for loading datas
        PlayerPrefs.DeleteKey("solvedEncounters");
        PlayerPrefs.DeleteKey("isDimensionshiftUnlocked");
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitBtn()
    {
        Application.Quit();
    }


}
