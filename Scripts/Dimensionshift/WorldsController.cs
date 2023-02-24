using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldsController : MonoBehaviour
{
    public static WorldsController instance; //singleton pattern


    private bool normalWorldIsActive = true;
    public bool NormalWorldIsActive => normalWorldIsActive;



    [SerializeField] private KeyCode timeswitchKey;
    [SerializeField] private AudioSource timeswitchSfx;
    [SerializeField] Color FogColorNormal;
    [SerializeField] Color FogColorSpirit;

    public bool isDimensionshiftUnlocked;

    private List<GameObject> normalOnly = new List<GameObject>();
    private List<GameObject> spiritOnly = new List<GameObject>();
    private List<GameObject> objWithEmissiveMat = new List<GameObject>();




    private void OnEnable()
    {
        Actions.OnDimensionshiftUnlocked += EnableDimensionshift;
    }

    private void OnDisable()
    {
        Actions.OnDimensionshiftUnlocked -= EnableDimensionshift;
    }




    private void Awake()
    {
        instance = this; //singleton pattern

        if (PlayerPrefs.GetInt("isDimensionshiftUnlocked") == 1)
        {
            isDimensionshiftUnlocked = true;
        }
    }




    private void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if ((Input.GetButtonDown("Dimensionshift") | Input.GetButtonDown("ControllerQuitGame"))  && !PlayerController.instance.IsDead && isDimensionshiftUnlocked)
            {
                PlayTimeswitchSfx();

                if (normalWorldIsActive) //if we are entering spirit world
                {
                    normalWorldIsActive = false;
                    Actions.OnWorldSwitched?.Invoke(); //communicate the world, that we switched the world
                    RenderSettings.fogColor = FogColorSpirit;

                    foreach (GameObject normalOnlyObj in normalOnly)
                    {
                        if (!normalOnlyObj.GetComponent<OneWorldOnly>().HasToDisplayOneWorldOnlyFeedback)
                        {
                            normalOnlyObj.gameObject.SetActive(false);
                        }
                    }
                    foreach (GameObject spiritOnlyObj in spiritOnly)
                    {
                        if (!spiritOnlyObj.GetComponent<OneWorldOnly>().HasToDisplayOneWorldOnlyFeedback)
                        {
                            spiritOnlyObj.gameObject.SetActive(true);
                        }
                    }
                    foreach (GameObject emissiveMatHolder in objWithEmissiveMat)
                    {
                        emissiveMatHolder.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                    }
                }
                else //if we are entering normal world
                {
                    normalWorldIsActive = true;
                    Actions.OnWorldSwitched?.Invoke(); //communicate the world, that we switched the world
                    RenderSettings.fogColor = FogColorNormal;

                    foreach (GameObject normalOnlyObj in normalOnly)
                    {
                        if (!normalOnlyObj.GetComponent<OneWorldOnly>().HasToDisplayOneWorldOnlyFeedback)
                        {
                            normalOnlyObj.gameObject.SetActive(true);
                        }
                    }
                    foreach (GameObject spiritOnlyObj in spiritOnly)
                    {
                        if (!spiritOnlyObj.GetComponent<OneWorldOnly>().HasToDisplayOneWorldOnlyFeedback)
                        {
                            spiritOnlyObj.gameObject.SetActive(false);
                        }
                    }
                    foreach (GameObject emissiveMatHolder in objWithEmissiveMat)
                    {
                        emissiveMatHolder.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                    }
                }
            }
        }
    }

    public void PlayTimeswitchSfx()
    {
        timeswitchSfx.Play();
    }


    public void AddToNormalOnlyList(GameObject obj)
    {
        normalOnly.Add(obj);
    }

    public void AddToSpiritOnlyList(GameObject obj)
    {
        spiritOnly.Add(obj);
    }

    public void AddToObjWithEmissiveMatList(GameObject obj)
    {
        objWithEmissiveMat.Add(obj);
    }



    private void EnableDimensionshift()
    {
        isDimensionshiftUnlocked = true;
    }



}
