using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class JumpLandSfxRandomized : MonoBehaviour
{
    [Header("Grass")]
    [SerializeField] private AudioClip[] grassLandings;
    private List<AudioClip> grassRandomList;

    [Header("Rocks")]
    [SerializeField] private AudioClip[] rockLandings;
    private List<AudioClip> rockRandomList;

    [Header("Wood")]
    [SerializeField] private AudioClip[] woodLandings;
    private List<AudioClip> woodRandomList;



    [Header("Settings")]
    private AudioSource source;
    [SerializeField] AudioMixerGroup mixerOutput;

    [SerializeField] private float pitchMin = 0.95f;
    [SerializeField] private float pitchMax = 1.05f;

    [SerializeField] private float volumeMin = 0.95f;
    [SerializeField] private float volumeMax = 1.00f;


    private ActorMovement actor;


    private void Start()
    {
        actor = GetComponent<ActorMovement>();
        source = gameObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = mixerOutput;


        grassRandomList = new List<AudioClip>(new AudioClip[grassLandings.Length]);
        for (int i = 0; i < grassLandings.Length; i++)
        {
            grassRandomList[i] = grassLandings[i];
        }

        rockRandomList = new List<AudioClip>(new AudioClip[rockLandings.Length]);
        for (int i = 0; i < rockLandings.Length; i++)
        {
            rockRandomList[i] = rockLandings[i];
        }

        woodRandomList = new List<AudioClip>(new AudioClip[woodLandings.Length]);
        for (int i = 0; i < woodLandings.Length; i++)
        {
            woodRandomList[i] = woodLandings[i];
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 10) //if we collided with the "Ground" Layer
        {
            if (actor.IsOnGrass())
            {
                PlayRandomSoundFromList(grassRandomList);
            }
            else if (actor.IsOnRocks())
            {
                PlayRandomSoundFromList(rockRandomList);
            }
            else if (actor.IsOnWood())
            {
                PlayRandomSoundFromList(woodRandomList);
            }
        }
    }


    public void PlayRandomSoundFromList(List<AudioClip> rndList)
    {
        int i = Random.Range(0, rndList.Count);
        source.pitch = Random.Range(pitchMin, pitchMax);
        source.volume = Random.Range(volumeMin, volumeMax);
        source.PlayOneShot(rndList[i]);
    }
}
