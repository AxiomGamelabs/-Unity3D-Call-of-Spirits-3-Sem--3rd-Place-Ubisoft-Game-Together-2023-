using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Checkpoint;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    [Header("World icons")]
    [SerializeField] private GameObject normalWorldIcon;
    [SerializeField] private GameObject spiritWorldIcon;

    [Header("Memorize")]
    public GameObject cantStartMemOutsideTimefieldsTxt;
    [SerializeField] private float deactivateCantMemTimer;
    public GameObject startMemBtnTxt;
    public GameObject stopMemBtnTxt;
    [SerializeField] private GameObject playMemBtnTxt;
    public Image[] memDots; //0 = blue, 1 = orange

    [Header("Doors")]
    [SerializeField] private GameObject doorLocksHUD;
    [SerializeField] private GameObject[] doorLocks;
    [SerializeField] private GameObject[] lockBordersWhite;
    [SerializeField] private GameObject[] lockContentsWhite;
    [SerializeField] private GameObject[] lockBordersGreen;
    [SerializeField] private GameObject[] lockContentsGreen;
    [SerializeField] private GameObject[] lockContentsRed;
    [SerializeField] private GameObject doorClosedIcon;
    [SerializeField] private GameObject doorOpenIcon;
    [HideInInspector] public int nrOfFinalEncounterDoorsReached = 0;
    private int nrOfOneWayDoorsThatDONTCountAsFinalDoors;


    [SerializeField] private TextMeshProUGUI encounterCounterTxt;

    [Header("VfxSunrays")]
    [SerializeField] private ParticleSystem vfxBlueSunrays;
    [SerializeField] private ParticleSystem vfxOrangeSunrays;

    [Header("VfxSparks")]
    [SerializeField] private ParticleSystem vfxBlueSparks;
    [SerializeField] private ParticleSystem vfxOrangeSparks;


    [Header("TimeBars")]
    [SerializeField] private GameObject[] timeBars; //0 = blue, 1 = orange





    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        if (PlayerPrefs.HasKey("solvedEncounters")) //CONTINUE GAME
        {
            LoadSolvedEncountersData();
            nrOfFinalEncounterDoorsReached--;
        }
        else //NEW GAME
        {
            nrOfFinalEncounterDoorsReached--;
        }
    }



    private void OnEnable()
    {
        Actions.OnWorldSwitched += ToggleWorldIcon;

        Actions.OnMemorizeActivated += DisplayStopMem;
        Actions.OnMemorizeDeactivated += DeactivateMemTxt;

        Actions.OnCheckpointEntered += ResetMemDots;
        Actions.OnCheckpointEntered += ResetLockBorders;
        Actions.OnCheckpointEntered += ResetLockContents;
        Actions.OnCheckpointEntered += ActivateDoorLocks;
        Actions.OnCheckpointEntered += DisplayClosedDoorIcon;
        Actions.OnCheckpointEntered += UpdateEncounterCounterElement;

        Actions.OnPlayerDeath += DeactivateTimebars;

        Actions.OnRedActivationBtnEnter += DectivateLockContentRed;
        Actions.OnRedActivationBtnEnter += ActivateLockBorderGreen;
        Actions.OnRedActivationBtnEnter += ActivateLockContentGreen;

        Actions.OnRedActivationBtnExit += DectivateLockContentGreen;
        Actions.OnRedActivationBtnExit += DectivateLockBorderGreen;
        Actions.OnRedActivationBtnExit += ActivateLockContentRed;


        Actions.OnWhiteKeyBtnEnter += ActivateLockContentWhite;

        Actions.OnEncounterDoorIsOpen += DisplayOpenDoorIcon;
        Actions.OnEncounterDoorIsClosed += DisplayClosedDoorIcon;

        Actions.OnGhostPlayed += ActivateSunraysVfx;
        Actions.OnGhostPlayed += ActivateSparksVfx;
        Actions.OnGhostKilled += DeactivateSunraysVfx;

        Actions.OnRestartFromLastCp += DeactivateAllTexts;
        Actions.OnRestartFromLastCp += DeactivateLockContentsWhite;
        Actions.OnRestartFromLastCp += DisplayClosedDoorIcon;
        Actions.OnRestartFromLastCp += ReactivateHudCanvas;

    }

    private void OnDisable()
    {
        Actions.OnWorldSwitched -= ToggleWorldIcon;

        Actions.OnMemorizeActivated -= DisplayStopMem;
        Actions.OnMemorizeDeactivated -= DeactivateMemTxt;

        Actions.OnCheckpointEntered -= ResetMemDots;
        Actions.OnCheckpointEntered -= ResetLockBorders;
        Actions.OnCheckpointEntered -= ResetLockContents;
        Actions.OnCheckpointEntered -= ActivateDoorLocks;
        Actions.OnCheckpointEntered -= DisplayClosedDoorIcon;
        Actions.OnCheckpointEntered -= UpdateEncounterCounterElement;

        Actions.OnPlayerDeath -= DeactivateTimebars;

        Actions.OnRedActivationBtnEnter -= DectivateLockContentRed;
        Actions.OnRedActivationBtnEnter -= ActivateLockBorderGreen;
        Actions.OnRedActivationBtnEnter -= ActivateLockContentGreen;

        Actions.OnRedActivationBtnExit -= DectivateLockContentGreen;
        Actions.OnRedActivationBtnExit -= DectivateLockBorderGreen;
        Actions.OnRedActivationBtnExit -= ActivateLockContentRed;

        Actions.OnWhiteKeyBtnEnter -= ActivateLockContentWhite;

        Actions.OnEncounterDoorIsOpen -= DisplayOpenDoorIcon;
        Actions.OnEncounterDoorIsClosed -= DisplayClosedDoorIcon;


        Actions.OnGhostPlayed -= ActivateSunraysVfx;
        Actions.OnGhostPlayed -= ActivateSparksVfx;
        Actions.OnGhostKilled -= DeactivateSunraysVfx;

        Actions.OnRestartFromLastCp -= DeactivateAllTexts;
        Actions.OnRestartFromLastCp -= DeactivateLockContentsWhite;
        Actions.OnRestartFromLastCp -= DisplayClosedDoorIcon;
        Actions.OnRestartFromLastCp -= ReactivateHudCanvas;


    }



    private void Update()
    {
        var notStandingOnAnyTimefield = !Recordings.instance.recordings[0].canRecord && !Recordings.instance.recordings[1].canRecord;
        var weHaveBlueMemories = Recordings.instance.recordings[0].RecordedDatas.Count > 0;
        var weHaveOrangeMemories = Recordings.instance.recordings[1].RecordedDatas.Count > 0;

        var weHaveRecordedDatas = weHaveBlueMemories || weHaveOrangeMemories;
        var weAreNotMemorizing = Recordings.instance.recordings[0].IsRecording == false && Recordings.instance.recordings[1].IsRecording == false;


        if (weHaveRecordedDatas && notStandingOnAnyTimefield && weAreNotMemorizing && !PlayerController.instance.IsDead)
        {
            playMemBtnTxt.SetActive(true);
        }
        else
        {
            playMemBtnTxt.SetActive(false);
        }

        if(weAreNotMemorizing)
        {
            //we have to do this, because restarting from last cp will reset all mem dots
            if (weHaveBlueMemories)
            {
                memDots[0].enabled = true;
            }
            if(weHaveOrangeMemories)
            {
                memDots[1].enabled = true;
            }
        }
    }

    private void LoadSolvedEncountersData()
    {
        nrOfFinalEncounterDoorsReached = PlayerPrefs.GetInt("solvedEncounters");
    }


    private void UpdateEncounterCounterElement(GameObject respawnPos, bool hasBeenVisited, int nrOfDoorLocksInTheEncounter, EncounterType encounterType)
    {
        if (!hasBeenVisited && encounterType != EncounterType.NONE)
        {
            nrOfFinalEncounterDoorsReached++;
            PlayerPrefs.SetInt("solvedEncounters", nrOfFinalEncounterDoorsReached);

            encounterCounterTxt.text = nrOfFinalEncounterDoorsReached + "/" + "18";

        }
    }



    public void ToggleWorldIcon()
    {
        if (WorldsController.instance.NormalWorldIsActive) //if we are entering normal world
        {
            normalWorldIcon.SetActive(true);
            spiritWorldIcon.SetActive(false);
        }
        else //if we entered ghost world
        {
            normalWorldIcon.SetActive(false);
            spiritWorldIcon.SetActive(true);
        }
    }



    public void TriggerCantRecordFeedback()
    {
        if (!cantStartMemOutsideTimefieldsTxt.activeInHierarchy)
        {
            cantStartMemOutsideTimefieldsTxt.SetActive(true);
            Invoke(nameof(DeactivateCantRecordFeedback), deactivateCantMemTimer);
        }
    }

    public void DeactivateCantRecordFeedback()
    {
        cantStartMemOutsideTimefieldsTxt.SetActive(false);
    }


    public void DisplayStopMem(int ghostId)
    {
        stopMemBtnTxt.SetActive(true);
        startMemBtnTxt.SetActive(false);

        timeBars[ghostId].SetActive(true);
    }

    public void DeactivateMemTxt(int ghostId)
    {
        stopMemBtnTxt.SetActive(false);

        timeBars[ghostId].SetActive(false);
    }


    private void DeactivateAllTexts()
    {
        startMemBtnTxt.SetActive(false);

        timeBars[0].SetActive(false);
        timeBars[1].SetActive(false);
    }

    private void DeactivateTimebars()
    {
        timeBars[0].SetActive(false);
        timeBars[1].SetActive(false);
    }


    public void DisplayOpenDoorIcon()
    {
        doorClosedIcon.SetActive(false);
        doorOpenIcon.SetActive(true);
    }

    public void DisplayOpenDoorIcon(GameObject respawnPos, bool hasBeenVisited, int nrOfDoorLocksInTheEncounter, EncounterType encounterType)
    {
        doorClosedIcon.SetActive(false);
        doorOpenIcon.SetActive(true);
    }


    public void DisplayClosedDoorIcon()
    {
        doorClosedIcon.SetActive(true);
        doorOpenIcon.SetActive(false);
    }

    public void DisplayClosedDoorIcon(GameObject respawnPos, bool hasBeenVisited, int nrOfDoorLocksInTheEncounter, EncounterType encounterType)
    {
        if(!hasBeenVisited)
        {
            doorClosedIcon.SetActive(true);
            doorOpenIcon.SetActive(false);
        }
    }




    private void ResetLockBorders(GameObject respawnPos, bool hasBeenVisited, int nrOfDoorLocksInTheEncounter, EncounterType encounterType)
    {
        if (!hasBeenVisited)
        {
            foreach (GameObject lockBorderWhite in lockBordersWhite)
            {
                lockBorderWhite.SetActive(false);
            }
            foreach (GameObject lockBorderGreen in lockBordersGreen)
            {
                lockBorderGreen.SetActive(false);
            }
        }
    }


    private void ResetLockContents(GameObject respawnPos, bool hasBeenVisited, int nrOfDoorLocksInTheEncounter, EncounterType encounterType)
    {
        if (!hasBeenVisited)
        {
            foreach (GameObject lockContentWhite in lockContentsWhite)
            {
                lockContentWhite.SetActive(false);
            }
            foreach (GameObject lockContentGreen in lockContentsGreen)
            {
                lockContentGreen.SetActive(false);
            }
            foreach (GameObject lockContentRed in lockContentsRed)
            {
                lockContentRed.SetActive(false);
            }
        }
    }





    public void ActivateDoorLocks(GameObject respawnPos, bool hasBeenVisited, int nrOfDoorLocksInTheEncounter, EncounterType encounterType)
    {
        if (nrOfDoorLocksInTheEncounter == 0)
        {
            doorLocksHUD.SetActive(false);
        }

        if (nrOfDoorLocksInTheEncounter == 1)
        {
            doorLocksHUD.SetActive(true);
            doorLocks[0].SetActive(true);

            switch (encounterType)
            {
                case EncounterType.WHITE_KEYS:
                    lockBordersWhite[0].SetActive(true);
                    break;
                case EncounterType.RED_KEYS:
                    lockContentsRed[0].SetActive(true);
                    break;
            }


        }

        if (nrOfDoorLocksInTheEncounter == 2)
        {
            doorLocksHUD.SetActive(true);
            doorLocks[0].SetActive(true);
            doorLocks[1].SetActive(true);

            switch (encounterType)
            {
                case EncounterType.WHITE_KEYS:
                    lockBordersWhite[0].SetActive(true);
                    lockBordersWhite[1].SetActive(true);
                    break;
                case EncounterType.RED_KEYS:
                    lockContentsRed[0].SetActive(true);
                    lockContentsRed[1].SetActive(true);
                    break;
            }

        }
    }



    public void ResetMemDots(GameObject respawnPos, bool hasBeenVisited, int nrOfDoorLocksInTheEncounter, EncounterType encounterType)
    {
        memDots[0].enabled = false;
        memDots[1].enabled = false;
    }


    public void ActivateLockContentWhite(int btnIdInTheEncounter)
    {
        lockContentsWhite[btnIdInTheEncounter].SetActive(true);
    }


    public void ActivateLockContentGreen(int btnIdInTheEncounter)
    {
        lockContentsGreen[btnIdInTheEncounter].SetActive(true);
    }



    public void DeactivateLockContentsWhite()
    {
        foreach (GameObject lockContentWhite in lockContentsWhite)
        {
            lockContentWhite.SetActive(false);
        }
    }

    public void DectivateLockContentWhite(int btnIdInTheEncounter)
    {
        lockContentsWhite[btnIdInTheEncounter].SetActive(false);
    }

    public void DectivateLockContentGreen(int btnIdInTheEncounter)
    {
        lockContentsGreen[btnIdInTheEncounter].SetActive(false);
    }



    public void ActivateLockBorderGreen(int btnIdInTheEncounter)
    {
        lockBordersGreen[btnIdInTheEncounter].SetActive(true);
    }

    public void DectivateLockBorderGreen(int btnIdInTheEncounter)
    {
        lockBordersGreen[btnIdInTheEncounter].SetActive(false);
    }



    public void ActivateLockContentRed(int btnIdInTheEncounter)
    {
        lockContentsRed[btnIdInTheEncounter].SetActive(true);
    }

    public void DectivateLockContentRed(int btnIdInTheEncounter)
    {
        lockContentsRed[btnIdInTheEncounter].SetActive(false);
    }






    private void ActivateSparksVfx(int ghostId)
    {
        if (ghostId == 0)
        {
            vfxBlueSparks.Stop();
            vfxBlueSparks.Play();
        }

        if (ghostId == 1)
        {
            vfxOrangeSparks.Stop();
            vfxOrangeSparks.Play();
        }
    }


    private void ActivateSunraysVfx(int ghostId)
    {
        if(ghostId == 0)
        {
            vfxBlueSunrays.Play();
        }

        if(ghostId == 1)
        {
            vfxOrangeSunrays.Play();
        }
    }

    private void DeactivateSunraysVfx(int ghostId)
    {
        if (ghostId == 0)
        {
            vfxBlueSunrays.Stop();
        }

        if (ghostId == 1)
        {
            vfxOrangeSunrays.Stop();
        }
    }


    private void ReactivateHudCanvas()
    {
        GetComponent<Canvas>().enabled = true;
    }

}
