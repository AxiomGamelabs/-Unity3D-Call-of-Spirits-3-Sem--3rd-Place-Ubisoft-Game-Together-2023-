using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Checkpoint;

public class GhostController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform ghostObj;
    [SerializeField] private Transform orientation; //we have to set the rotation of this gameobject
    [SerializeField] private int ghostId;
    public int GhostId => ghostId;


    private void OnEnable()
    {
        Actions.OnGhostDeath += KillGhost;
        Actions.OnCheckpointEntered += KillGhost;


        Actions.OnGhostPlayed?.Invoke(ghostId);
        SpawnGhost.instance.SetGhostIsActive(ghostId, true);
    }


    private void OnDisable()
    {
        Actions.OnGhostDeath -= KillGhost;
        Actions.OnCheckpointEntered -= KillGhost;



        Actions.OnGhostKilled?.Invoke(ghostId);
        SpawnGhost.instance.SetGhostIsActive(ghostId, false);
    }

    void Start()
    {
        SetTransform(0); // we set the replay to the first recorded frame.
    }




    private void SetTransform(int index)
    {
        RecordedData recordData = Recordings.instance.recordings[ghostId].RecordedDatas[index];
        transform.position = recordData.position;
        orientation.rotation = recordData.modelRotation;
    }

    private void KillGhost(int ghostToKillId)
    {

        var weAreNotRecording = Recordings.instance.recordings[0].IsRecording == false && Recordings.instance.recordings[1].IsRecording == false;
        if (weAreNotRecording)
        {
            //check if it was the last living ghost active in the scene
            var allAliveGhosts = FindObjectsOfType<GhostController>();
            if (allAliveGhosts.Length == 1)
            {
                Actions.OnLastGhostDead?.Invoke();
            }
        }

        if(ghostId == ghostToKillId) //to avoid killing all ghosts at the same time, when just one dies
        {
            Destroy(gameObject);
        }
    }

    private void KillGhost(GameObject respawnPos, bool hasBeenVisited, int nrOfDoorLocksInTheEncounter, EncounterType encounterType)
    {
        if(!hasBeenVisited) 
        {
            Destroy(gameObject);
        }
    }


}
