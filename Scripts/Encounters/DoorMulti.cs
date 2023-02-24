using System.Collections.Generic;
using UnityEngine;

public class DoorMulti : MonoBehaviour
{
    [SerializeField] private ActivationBtn[] activationBtns;
    [SerializeField] private GameObject[] activationLights;
    [SerializeField] private Material[] lightMats; //index 0 = red color, index 1 = green color
    [SerializeField] private GameObject doorL;
    [SerializeField] private GameObject doorR;
    [HideInInspector] public bool isRelevantForTheEncounter;

    private bool hasPlayedOpenDoorSfx;
    private bool hasPlayedCloseDoorSfx;

    [SerializeField] private bool isBossWorkaroundDoor;


    [SerializeField] private GameObject outlineVfx;


    private void Start()
    {
        hasPlayedCloseDoorSfx = true;
    }



    private void Update()
    {
        if (HasToOpen())
        {
            if (isRelevantForTheEncounter)
            {
                Actions.OnEncounterDoorIsOpen?.Invoke();
            }
            doorL.GetComponent<Animator>().SetBool("isOpen", true);
            doorR.GetComponent<Animator>().SetBool("isOpen", true);

            outlineVfx.SetActive(true);

            if (!hasPlayedOpenDoorSfx && !isBossWorkaroundDoor)
            {
                hasPlayedOpenDoorSfx = true;
                hasPlayedCloseDoorSfx = false;
            }

        }
        else
        {
            if (isRelevantForTheEncounter)
            {
                Actions.OnEncounterDoorIsClosed?.Invoke();
            }
            doorL.GetComponent<Animator>().SetBool("isOpen", false);
            doorR.GetComponent<Animator>().SetBool("isOpen", false);

            outlineVfx.SetActive(false);


            if (!hasPlayedCloseDoorSfx)
            {
                if (!hasPlayedOpenDoorSfx && !isBossWorkaroundDoor)
                {
                    hasPlayedCloseDoorSfx = true;
                    hasPlayedOpenDoorSfx = false;
                }
            }
        }

        for (int i = 0; i < activationLights.Length; i++)
        {
            if (activationBtns[i].IsActive)
            {
                activationLights[i].GetComponent<Renderer>().material = lightMats[1];
            }
            else
            {
                activationLights[i].GetComponent<Renderer>().material = lightMats[0];
            }
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
}
