using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record : MonoBehaviour
{
    private ActorMovement actorMovement;
    [HideInInspector] public bool canRecord = false;
    public bool CanRecord => canRecord;
    private bool isRecording = false;
    public bool IsRecording => isRecording;
    public List<RecordedData> RecordedDatas = new List<RecordedData>();
    [SerializeField] int ghostId;
    private Vector3 respawnPositionAfterRecording;



    private void OnEnable()
    {
        Actions.OnTimefieldEntered += UpdateSpawnPoint;
        Actions.OnTimerReachedZero += StopRecordingByTimer;
    }

    private void OnDisable()
    {
        Actions.OnTimefieldEntered -= UpdateSpawnPoint;
        Actions.OnTimerReachedZero -= StopRecordingByTimer;
    }


    private void Start()
    {
        actorMovement = GetComponent<ActorMovement>();
    }



    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (isRecording)
            {
                SpawnGhost.instance.SetCanPlay(false); //bugfix for: stopping rec sometimes spawns ghosts at the same time, because at that point "canPlay" is true
            }



            if (Input.GetButtonDown("Memorize"))
            {
                if (canRecord) //if we are standing on the timefield to this record script
                {
                    SpawnGhost.instance.DestroyAllGhosts();
                    ActivateRecording();
                    SpawnMemPath.instance.DestroyAllMemPaths();
                    SpawnGhost.instance.SetCanPlay(false);
                    SpawnGhost.instance.SpawnAllGhosts();
                }
                else //I am not on a timefield
                {
                    if (isRecording)
                    {
                        Recordings.instance.PlayStopRecSfx();
                        DeactivateRecording();
                        RespawnPlayerAtTimefield();
                        StartCoroutine(SetupForPlayHoloCoroutine());
                    }
                    else
                    {
                        if (Recordings.instance.recordings[0].RecordedDatas.Count == 0 && Recordings.instance.recordings[1].RecordedDatas.Count == 0) //if we have no recordedDatas
                        {
                            if (!Recordings.instance.recordings[0].canRecord && !Recordings.instance.recordings[1].canRecord) //we have to be sure we are not standing on ANY timefield, before displaying feedback. The condition in the first if statement only checks the timefield that has this Record script attached to it
                            {
                                HUD.instance.TriggerCantRecordFeedback(); //display feedback: cannot activate recording outside of timefields
                                Recordings.instance.PlayCannotRecOutsideTimefieldSfx();
                            }
                        }
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        var newPosition = transform.position;
        var newRotation = actorMovement.charModel.rotation;
        var newHorInput = actorMovement.HorizontalInput;
        var newVerInput = actorMovement.VerticalInput;
        var newIsJumping = actorMovement.IsJumping;
        var newHorizontalMoveDirection = actorMovement.HorizontalMoveDirection;
    }

    public void AddRecordData(Vector3 newPosition, Quaternion newRotation, float newHorInput, float newVerInput, bool newIsJumping, Vector3 newHorizontalMoveDirection)
    {
        if (isRecording) //we want to save data only while recording
        {
            //record does not have to get the values itself, we should pass the values from ActorMovement instead
            RecordedDatas.Add(new RecordedData
            {
                position = newPosition,
                modelRotation = newRotation,
                horInput = newHorInput,
                verInput = newVerInput,
                isJumping = newIsJumping,
                horizontalMoveDirection = newHorizontalMoveDirection
            });
        }
    }

    private void ActivateRecording()
    {
        isRecording = true;
        Actions.OnMemorizeActivated?.Invoke(ghostId);
        Recordings.instance.PlayStartRecSfx();


        RecordedDatas.Clear();
    }

    private void DeactivateRecording()
    {
        Actions.OnMemorizeDeactivated?.Invoke(ghostId);


        switch (ghostId)
        {
            case 0:
                if(isRecording) 
                {
                    HUD.instance.memDots[0].enabled = true;
                }
                break;
            case 1:
                if(isRecording)
                {
                    HUD.instance.memDots[1].enabled = true;
                }
                break;
        }

        isRecording = false;

    }



    private void StopRecordingByTimer()
    {
        Recordings.instance.PlayRecTimerExpiredSfx();
        DeactivateRecording();
        RespawnPlayerAtTimefield();
        StartCoroutine(SetupForPlayHoloCoroutine());
    }

    //without it, he would spawn an hologram when we press the record key to stop a recording
    IEnumerator SetupForPlayHoloCoroutine()
    {
        yield return new WaitForSeconds(Recordings.instance.WaitTimeBeforeCanPlay);
        SpawnGhost.instance.SetCanPlay(true);
    }


    private void UpdateSpawnPoint(GameObject playerRespawnPos)
    {
        respawnPositionAfterRecording = playerRespawnPos.transform.position;
    }


    private void RespawnPlayerAtTimefield()
    {
        transform.position = respawnPositionAfterRecording;
    }

    public void SetIsRecording(bool value)
    {
        isRecording = value;
    }

}
