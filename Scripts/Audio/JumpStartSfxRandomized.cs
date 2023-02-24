using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class JumpStartSfxRandomized : MonoBehaviour
{
    [Header("Grass")]
    [SerializeField] private AudioClip[] grassJumps;
    private List<AudioClip> grassRandomList;

    [Header("Rocks")]
    [SerializeField] private AudioClip[] rockJumps;
    private List<AudioClip> rockRandomList;

    [Header("Wood")]
    [SerializeField] private AudioClip[] woodJumps;
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


        grassRandomList = new List<AudioClip>(new AudioClip[grassJumps.Length]);
        for (int i = 0; i < grassJumps.Length; i++)
        {
            grassRandomList[i] = grassJumps[i];
        }

        rockRandomList = new List<AudioClip>(new AudioClip[rockJumps.Length]);
        for (int i = 0; i < rockJumps.Length; i++)
        {
            rockRandomList[i] = rockJumps[i];
        }

        woodRandomList = new List<AudioClip>(new AudioClip[woodJumps.Length]);
        for (int i = 0; i < woodJumps.Length; i++)
        {
            woodRandomList[i] = woodJumps[i];
        }
    }

    private void Update()
    {
        if (actor.IsJumping)
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
