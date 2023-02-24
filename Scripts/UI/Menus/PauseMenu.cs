using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public static bool isPaused = false;
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private GameObject controlsMenuCanvas;
    [SerializeField] private GameObject audioMenuCanvas;
    [SerializeField] private GameObject arrowContainer;
    [SerializeField] private GameObject hud;


    [SerializeField] private AudioSource pauseSfx;
    [SerializeField] private AudioSource unpauseSfx;


    [SerializeField] private CinemachineFreeLook freeLookCam;
    [HideInInspector] public GameObject player;


    private void Awake()
    {
        instance = this;
        pauseSfx.ignoreListenerPause = true;
        unpauseSfx.ignoreListenerPause = true;
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }


    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && !GameController.instance.hasWon)
        {
            if (isPaused) 
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        //ONLY FOR CONTROLLER BUILD
        if(SceneManager.GetActiveScene().name == "MesseChrisScene")
        {
            if (Input.GetButtonDown("ControllerNewGame") && !GameController.instance.hasWon)
            {
                if (isPaused)
                {
                    NewGame();
                }
            }

            if (Input.GetButtonDown("ControllerQuitGame") && !GameController.instance.hasWon)
            {
                if (isPaused)
                {
                    Application.Quit();
                }
            }

            if (Input.GetButtonDown("ControllerRespawn") && !GameController.instance.hasWon)
            {
                if (isPaused)
                {
                    RespawnFromLastCpController();
                }
            }
        }

    }



    private void PauseGame()
    {
        pauseSfx.Play();
        

        pauseMenuCanvas.SetActive(true);
        hud.gameObject.GetComponent<Canvas>().enabled = false;
        Time.timeScale = 0f;
        AudioListener.pause = true;
        LockCameraMovement();
        isPaused = true;
    }

    public void ResumeGame()
    {
        unpauseSfx.Play();


        pauseMenuCanvas.SetActive(false);
        hud.gameObject.GetComponent<Canvas>().enabled = true;
        audioMenuCanvas.SetActive(false);
        controlsMenuCanvas.SetActive(false);
        arrowContainer.SetActive(false);

        Time.timeScale = 1f;
        AudioListener.pause = false;
        UnlockCameraMovement();
        isPaused = false;
    }


    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        pauseMenuCanvas.SetActive(false);

        Time.timeScale = 1f;
        AudioListener.pause = false;
        UnlockCameraMovement();
        isPaused = false;

        PlayerPrefs.DeleteKey("lastVisitedCpPosX");
        PlayerPrefs.DeleteKey("solvedEncounters");
        PlayerPrefs.DeleteKey("isDimensionshiftUnlocked");
    }


    public void LastCpBtn()
    {
        pauseMenuCanvas.SetActive(false);
        arrowContainer.SetActive(false);


        Actions.OnRestartFromLastCp?.Invoke();
        PlayerController.instance.KillPlayer();
        player.transform.SetParent(null);
        SpawnGhost.instance.DestroyAllGhosts();
        SpawnMemPath.instance.DestroyAllMemPaths(); //so that we clean all memPaths, when reloading while standing on a timefield
        Recordings.instance.recordings[0].canRecord = false; //otherwise we would be able to start recording outside from timefields
        Recordings.instance.recordings[1].canRecord = false;

        GameController.instance.RespawnPlayer();


        Time.timeScale = 1f;
        AudioListener.pause = false;
        UnlockCameraMovement();
        isPaused = false;
    }

    private void RespawnFromLastCpController()
    {
        pauseMenuCanvas.SetActive(false);

        Actions.OnRestartFromLastCp?.Invoke();
        PlayerController.instance.KillPlayer();
        player.transform.SetParent(null);
        SpawnGhost.instance.DestroyAllGhosts();
        SpawnMemPath.instance.DestroyAllMemPaths(); //so that we clean all memPaths, when reloading while standing on a timefield
        Recordings.instance.recordings[0].canRecord = false; //otherwise we would be able to start recording outside from timefields
        Recordings.instance.recordings[1].canRecord = false;

        GameController.instance.RespawnPlayer();


        Time.timeScale = 1f;
        AudioListener.pause = false;
        UnlockCameraMovement();
        isPaused = false;
    }


    public void MainMenuBtn(string mainMenuScene)
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void AudioMenuBtn()
    {
        pauseMenuCanvas.SetActive(false);
        audioMenuCanvas.SetActive(true);
        arrowContainer.SetActive(false);
    }

    public void ControlsMenuBtn()
    {
        pauseMenuCanvas.SetActive(false);
        controlsMenuCanvas.SetActive(true);
        arrowContainer.SetActive(false);
    }

    public void BackBtn()
    {
        controlsMenuCanvas.SetActive(false);
        audioMenuCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(true);
        arrowContainer.SetActive(false);
    }


    public void SetPlayer(GameObject playerObj)
    {
        player = playerObj;
    }



    private void LockCameraMovement() => freeLookCam.enabled = false;
    private void UnlockCameraMovement() => freeLookCam.enabled = true;


}
