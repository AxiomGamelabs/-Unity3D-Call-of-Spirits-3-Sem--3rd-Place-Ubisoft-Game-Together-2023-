using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Checkpoint;


public class DoorMultiOneWay : MonoBehaviour
{
    [SerializeField] private List<ActivationBtn> activationBtns;

    [SerializeField] private GameObject doorL;
    [SerializeField] private GameObject doorR;

    [HideInInspector] public bool isRelevantForTheEncounter;
    [SerializeField] private bool isEncounterFinalDoor;
    [SerializeField] private bool hasPlayedOpenDoorSfx;
    [SerializeField] private AudioSource doorOpenSfx;
    [SerializeField] private AudioSource doorCloseSfx;

    [SerializeField] private GameObject outlineVfx;


    public bool IsEncounterFinalDoor => isEncounterFinalDoor;


    private void OnEnable()
    {
        Actions.OnPlayerDeath += Close;
        Actions.OnRestartFromLastCp += Close;
        Actions.OnCheckpointEntered += Close;
    }

    private void OnDisable()
    {
        Actions.OnPlayerDeath -= Close;
        Actions.OnRestartFromLastCp -= Close;
        Actions.OnCheckpointEntered -= Close;

    }





    private void Update()
    {
        if (HasToOpen())
        {
            outlineVfx.SetActive(true);

            if (isRelevantForTheEncounter)
            {
                Actions.OnEncounterDoorIsOpen?.Invoke();
            }
            doorL.GetComponent<Animator>().SetBool("isOpen", true);
            doorR.GetComponent<Animator>().SetBool("isOpen", true);

            if (!hasPlayedOpenDoorSfx)
            {
                doorOpenSfx.Play();
                hasPlayedOpenDoorSfx = true;
            }
        }
        else
        {
            outlineVfx.SetActive(false);
        }
    }



    private bool HasToOpen()
    {
        foreach (ActivationBtn btn in activationBtns)
        {
            if (!btn.IsActive)
            {
                return false;
            }
        }
        return true;
    }


    private void Close()
    {
        StartCoroutine(CloseCoroutine());   
    }


    private void Close(GameObject respawnPos, bool hasBeenVisited, int nrOfDoorLocksInTheEncounter, EncounterType encounterType)
    {
        if(!hasBeenVisited)
        {
            doorL.GetComponent<Animator>().SetBool("isOpen", false);
            doorR.GetComponent<Animator>().SetBool("isOpen", false);

            //if the door is close enough to the player (we don't want all the doors in the game play the closedoor sfx)
            var player = FindObjectOfType<PlayerController>().gameObject;  
            if(Vector3.Distance(gameObject.transform.position, player.transform.position) < 5)
            {
                doorCloseSfx.Play();
            }

        }
    }


    IEnumerator CloseCoroutine()
    {
        yield return new WaitForSeconds(1f); //we wait a bit, because otherwise pressing restartFromLastCpKey while standing on the ActivationBtn will not close the door
        doorL.GetComponent<Animator>().SetBool("isOpen", false);
        doorR.GetComponent<Animator>().SetBool("isOpen", false);

        hasPlayedOpenDoorSfx = false;
    }


}
