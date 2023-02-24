using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FootstepsSfxRandomized : MonoBehaviour
{
    [Header("Grass")]
    [SerializeField] private AudioClip[] grassRunSteps;
    private List<AudioClip> grassRandomList;

    [Header("Rocks")]
    [SerializeField] private AudioClip[] rockRunSteps;
    private List<AudioClip> rockRandomList;

    [Header("Wood")]
    [SerializeField] private AudioClip[] woodRunSteps;
    private List<AudioClip> woodRandomList;


    [Header("Settings")]
    private AudioSource source;
    [SerializeField] private float timeBtwFootsteps = 0.5f;

    [SerializeField] AudioMixerGroup mixerOutput;

    [SerializeField] private float pitchMin = 0.95f;
    [SerializeField] private float pitchMax = 1.05f;

    [SerializeField] private float volumeMin = 0.95f;
    [SerializeField] private float volumeMax = 1.00f;


    private ActorMovement actor;


    private void Start()
    {
        actor = GetComponent<ActorMovement>();
        InvokeRepeating("CallFootsteps", 0, timeBtwFootsteps);
        source = gameObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = mixerOutput;


        grassRandomList = new List<AudioClip>(new AudioClip[grassRunSteps.Length]);
        for (int i = 0; i < grassRunSteps.Length; i++)
        {
            grassRandomList[i] = grassRunSteps[i];
        }

        rockRandomList = new List<AudioClip>(new AudioClip[rockRunSteps.Length]);
        for (int i = 0; i < rockRunSteps.Length; i++)
        {
            rockRandomList[i] = rockRunSteps[i];
        }

        woodRandomList = new List<AudioClip>(new AudioClip[woodRunSteps.Length]);
        for (int i = 0; i < woodRunSteps.Length; i++)
        {
            woodRandomList[i] = woodRunSteps[i];
        }

    }


    public void ResetLists()
    {
        for(int i = 0; i < grassRunSteps.Length; i++)
        {
            grassRandomList.Add(grassRunSteps[i]);
        }
        for (int i = 0; i < rockRunSteps.Length; i++)
        {
            rockRandomList.Add(rockRunSteps[i]);
        }
        for (int i = 0; i < woodRunSteps.Length; i++)
        {
            woodRandomList.Add(woodRunSteps[i]);
        }
    }

    private void CallFootsteps()
    {
        if(actor.IsMoving && actor.IsGrounded())
        {
            if (actor.IsOnGrass())
            {
                PlayRandomSoundFromList(grassRandomList);
            }
            else if(actor.IsOnRocks())
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
        rndList.RemoveAt(i);

        if (rndList.Count == 0)
        {
            ResetLists();
        }
    }





}
