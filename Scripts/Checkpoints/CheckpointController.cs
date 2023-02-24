using System.Collections.Generic;
using UnityEngine;
using static Checkpoint;

public class CheckpointController : MonoBehaviour
{
    public static CheckpointController instance;
    public GameObject lastReachedCp;



    private void OnEnable()
    {
        Actions.OnCheckpointEntered += UpdateCp;
    }

    private void OnDisable()
    {
        Actions.OnCheckpointEntered -= UpdateCp;
    }




    private void Awake()
    {
        instance = this;
    }


    private void UpdateCp(GameObject respawnPos, bool hasBeenVisited, int nrOfDoorLocksInTheEncounter, EncounterType encounterType)
    {
        if (!hasBeenVisited) //only update and clear old recs if we are entering it the first time
        {
            lastReachedCp = respawnPos;
            foreach (Record rec in Recordings.instance.recordings)
            {
                rec.RecordedDatas.Clear(); //delete old recordings when reaching new checkpoint to avoid beeing able to go back to previous encounters
                //this makes it unnecessary to manually close the doors behind us when entering a new encounter. The holograms that were keeping that door open get instantly deleted.
            }
        }
    }


}
