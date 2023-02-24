using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpawnGhost : MonoBehaviour
{
    public static SpawnGhost instance;

    [SerializeField] private GameObject[] ghostPrefabs;
    private bool[] ghostIsActive = new bool[] { false, false };
    private bool canPlay = false;

    [SerializeField] private AudioSource playGhostsSfx;




    private void Awake()
    {
        instance = this;
    }



    private void Update()
    {
        if(!PauseMenu.isPaused)
        {
            if (Input.GetButtonDown("Memorize") && !IsStandingOnATimefield())
            {
                if (canPlay)
                {
                    DestroyAllGhosts();
                    PlaySpawnGhostSfx();
                    SpawnAllGhosts();
                    Actions.OnGhostsPlayed?.Invoke();
                }
            }
        }
    }

    public void SpawnAllGhosts()
    {
        for (int i = 0; i < Recordings.instance.recordings.Length; i++)
        {
            if (Recordings.instance.recordings[i].RecordedDatas.Count > 0)
            {
                Instantiate(ghostPrefabs[i]); //the positions of the ghost (including the start position) gets set in the PlayGhost script
            }
        }
    }

    private bool IsStandingOnATimefield()
    {
        foreach(Record rec in Recordings.instance.recordings)
        {
            if (rec.CanRecord)
            {
                return true;
            }
        }

        return false;
    }

    public void SetGhostIsActive(int ghostId, bool isActive)
    {
        ghostIsActive[ghostId] = isActive;
    }

    public void SetCanPlay(bool value)
    {
        canPlay = value;
    }

    public void DestroyAllGhosts()
    {
        var ghosts = FindObjectsOfType<GhostController>();
        if (ghosts.Length > 0) //if any ghost is already active
        {
            foreach (GhostController ghost in ghosts)
            {
                Destroy(ghost.gameObject); //destroy it
            }
        }
    }

    public void PlaySpawnGhostSfx()
    {
        playGhostsSfx.Play();
    }
}
