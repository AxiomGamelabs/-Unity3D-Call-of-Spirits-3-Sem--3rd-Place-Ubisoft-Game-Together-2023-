using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject playerRespawnPos;
    private bool hasBeenVisited = false;
    [SerializeField] private EncounterType encounterType;
    [SerializeField] private DoorMulti door;
    [SerializeField] private DoorMultiOneWay doorOneWay;



    [SerializeField] private int nrOfLocksInTheEncounter;
    [SerializeField] private List<ActivationBtn> redBtnsInTheEncounter;
    [SerializeField] private ActivationBtn whiteKeyBtnInTheEncounter;



    [SerializeField] private AudioSource checkpointReachedSfx;




    public enum EncounterType
    {
        WHITE_KEYS,
        RED_KEYS,
        NONE
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasBeenVisited)
            {
                checkpointReachedSfx.Play();
                EncounterKeyBtnsSetup();
                EncounterDoorsSetup();
            }

            Actions.OnCheckpointEntered?.Invoke(playerRespawnPos, hasBeenVisited, nrOfLocksInTheEncounter, encounterType); //we communicate, where the player should be respawned when finishing recording, if the cp has been visited already, and how many locks the door in the encounter has
            hasBeenVisited = true;

        }
    }



    private void EncounterKeyBtnsSetup()
    {
        var allBtnsInTheGame = FindObjectsOfType<ActivationBtn>(); //does this detect btns that are in the spirit world?
        foreach (ActivationBtn btn in allBtnsInTheGame)
        {
            btn.isRelevantForTheEncounter = false;
        }

        foreach (ActivationBtn redBtn in redBtnsInTheEncounter)
        {
            redBtn.isRelevantForTheEncounter = true;
        }

        if (whiteKeyBtnInTheEncounter != null)
        {
            whiteKeyBtnInTheEncounter.isRelevantForTheEncounter = true;
        }
    }

    private void EncounterDoorsSetup()
    {
        var allDoorsInTheGame = FindObjectsOfType<DoorMulti>();
        var allDoorsOneWayInTheGame = FindObjectsOfType<DoorMultiOneWay>();
        foreach (DoorMultiOneWay doorOneWay in allDoorsOneWayInTheGame)
        {
            doorOneWay.isRelevantForTheEncounter = false;
        }

        foreach (DoorMulti doorMulti in allDoorsInTheGame)
        {
            doorMulti.isRelevantForTheEncounter = false;
        }

        if (door != null)
        {
            door.isRelevantForTheEncounter = true;
        }
        if (doorOneWay != null)
        {
            doorOneWay.isRelevantForTheEncounter = true;
        }
    }
}
