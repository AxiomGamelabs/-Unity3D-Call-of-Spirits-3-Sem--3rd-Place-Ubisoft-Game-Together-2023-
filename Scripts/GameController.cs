using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Checkpoint;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private GameObject player;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private float waitBeforeRespawn;
    public float WaitBeforeRespawn => waitBeforeRespawn;
    private Vector3 loadedPlayerPosition;
    [SerializeField] private bool isSaveSystemActivated;
    [SerializeField] private Transform playerSpawnPosOnNewGame;
    [SerializeField] private Follower skyBox;
    [SerializeField] private CinemachineFreeLook thirdPersonCam;
    [HideInInspector] public bool hasWon;
    public AudioSource deathSfx;


    private void OnEnable()
    {
        Actions.OnPlayerDeath += RespawnPlayer;
        Actions.OnCheckpointEntered += SaveGamePrefs;
    }

    private void OnDisable()
    {
        Actions.OnPlayerDeath -= RespawnPlayer;
        Actions.OnCheckpointEntered -= SaveGamePrefs;
    }

    private void Awake()
    {
        instance = this;

        if (isSaveSystemActivated)
        {
            if (PlayerPrefs.HasKey("lastVisitedCpPosX"))
            {
                SpawnPlayerAtLoadedPos();
            }
            else
            {
                SpawnPlayerOnNewGame();
            }
        }
        else //Developer debug mode, with player prefab placed by hand in the world
        {
            player = FindObjectOfType<PlayerController>().gameObject;
            skyBox.SetFollowerTarget(player);
            thirdPersonCam.Follow = player.transform;
            thirdPersonCam.LookAt = player.transform;
        }

        //ONLY FOR CONTROLLER BUILD
        if (SceneManager.GetActiveScene().name == "MesseChrisScene")
        {
            PlayerPrefs.DeleteKey("lastVisitedCpPosX");
            PlayerPrefs.DeleteKey("solvedEncounters");
            PlayerPrefs.DeleteKey("isDimensionshiftUnlocked");
        }
    }


    public void SaveGamePrefs(GameObject respawnPos, bool hasBeenVisited, int nrOfDoorLocksInTheEncounter, EncounterType encounterType)
    {
        PlayerPrefs.SetFloat("lastVisitedCpPosX", respawnPos.transform.position.x);
        PlayerPrefs.SetFloat("lastVisitedCpPosY", respawnPos.transform.position.y);
        PlayerPrefs.SetFloat("lastVisitedCpPosZ", respawnPos.transform.position.z);

        if(WorldsController.instance.isDimensionshiftUnlocked)
        {
            PlayerPrefs.SetInt("isDimensionshiftUnlocked", 1);
        }

    }


    public void RespawnPlayer()
    {
        Invoke(nameof(PlacePlayerAtLastCp), waitBeforeRespawn);
    }




    private void PlacePlayerAtLastCp()
    {
        player.gameObject.SetActive(true);
        player.transform.position = CheckpointController.instance.lastReachedCp.transform.position;
        PlayerController.instance.SetIsDead(false);
        SpawnGhost.instance.SetCanPlay(true); //to permit a player to play ghosts if he already recorded some
    }


    public void SpawnPlayerOnNewGame()
    {
        player = Instantiate(playerPrefab, playerSpawnPosOnNewGame.position, Quaternion.identity);
        skyBox.SetFollowerTarget(player);
        thirdPersonCam.Follow = player.transform;
        thirdPersonCam.LookAt = player.transform;
    }


    private void SpawnPlayerAtLoadedPos()
    {
        loadedPlayerPosition.x = PlayerPrefs.GetFloat("lastVisitedCpPosX");
        loadedPlayerPosition.y = PlayerPrefs.GetFloat("lastVisitedCpPosY");
        loadedPlayerPosition.z = PlayerPrefs.GetFloat("lastVisitedCpPosZ");


        player = Instantiate(playerPrefab, loadedPlayerPosition, Quaternion.identity);
        skyBox.SetFollowerTarget(player);
        thirdPersonCam.Follow = player.transform;
        thirdPersonCam.LookAt = player.transform;
    }

}
