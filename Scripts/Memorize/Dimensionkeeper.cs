using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dimensionkeeper : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject spiritMesh;
    [SerializeField] private GameObject spiritLightHolder;
    [SerializeField] private GameObject runesLightHolder;
    [SerializeField] private GameObject runesHolder;
    [SerializeField] private GameObject runesCore;
    [SerializeField] private GameObject emirTag;



    [Header("Light Colors")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color blueColor;
    [SerializeField] private Color orangeColor;

    [Header("Light Intesities")]
    [SerializeField] private float defaultIntensity;
    [SerializeField] private float memorizeIntensity;
    [SerializeField] private float isPlayingGhostsIntensity;




    [Header("Dimensionkeeper Materials")]
    [SerializeField] private Material defaultMat;
    [SerializeField] private Material blueMat;
    [SerializeField] private Material orangeMat;

    [Header("RunesHolder Materials")]
    [SerializeField] private Material defaultRunesMat;
    [SerializeField] private Material blueRunesMat;
    [SerializeField] private Material orangeRunesMat;

    [Header("RunesCore Materials")]
    [SerializeField] private Material defaultRunesCoreMat;
    [SerializeField] private Material blueRunesCoreMat;
    [SerializeField] private Material orangeRunesCoreMat;

    [Header("EmirTag Materials")]
    [SerializeField] private Material defaultTagMat;
    [SerializeField] private Material blueTagMat;
    [SerializeField] private Material orangeTagMat;


    private void OnEnable()
    {
        Actions.OnMemorizeActivated += ActivateRecordingMaterial;
        Actions.OnMemorizeDeactivated += ActivateDefaultMaterial;
        Actions.OnLastGhostDead += DeactivateIsPlayingGhostsSettings;
    }

    private void OnDisable()
    {
        Actions.OnMemorizeActivated -= ActivateRecordingMaterial;
        Actions.OnMemorizeDeactivated -= ActivateDefaultMaterial;
        Actions.OnLastGhostDead -= DeactivateIsPlayingGhostsSettings;
    }




    private void Awake()
    {
        spiritMesh.GetComponent<SkinnedMeshRenderer>().material = defaultMat;
        runesHolder.GetComponent<MeshRenderer>().material = defaultRunesMat;

        spiritLightHolder.GetComponent<Light>().color = defaultColor;
        spiritLightHolder.GetComponent<Light>().intensity = defaultIntensity;

        runesLightHolder.GetComponent<Light>().color = defaultColor;
        runesLightHolder.GetComponent<Light>().intensity = defaultIntensity;
    }

    private void ActivateRecordingMaterial(int ghostId)
    {
        switch(ghostId)
        {
            case 0:
                spiritMesh.GetComponent<SkinnedMeshRenderer>().material = blueMat;

                spiritLightHolder.GetComponent<Light>().color = blueColor;
                spiritLightHolder.GetComponent<Light>().intensity = memorizeIntensity;

                runesLightHolder.GetComponent<Light>().color = blueColor;
                runesLightHolder.GetComponent<Light>().intensity = memorizeIntensity;

                runesHolder.GetComponent<MeshRenderer>().material = blueRunesMat;
                runesCore.GetComponent<MeshRenderer>().material = blueRunesCoreMat;

                emirTag.GetComponent<SkinnedMeshRenderer>().material = blueTagMat;
                break;

            case 1:
                spiritMesh.GetComponent<SkinnedMeshRenderer>().material = orangeMat;

                spiritLightHolder.GetComponent<Light>().color = orangeColor;
                spiritLightHolder.GetComponent<Light>().intensity = memorizeIntensity;

                runesLightHolder.GetComponent<Light>().color = orangeColor;
                runesLightHolder.GetComponent<Light>().intensity = memorizeIntensity;

                runesHolder.GetComponent<MeshRenderer>().material = orangeRunesMat;
                runesCore.GetComponent<MeshRenderer>().material = orangeRunesCoreMat;

                emirTag.GetComponent<SkinnedMeshRenderer>().material = orangeTagMat;
                break;
        }
    }

    private void ActivateDefaultMaterial(int ghostId)
    {
        spiritMesh.GetComponent<SkinnedMeshRenderer>().material = defaultMat;

        spiritLightHolder.GetComponent<Light>().color = defaultColor;
        spiritLightHolder.GetComponent<Light>().intensity = defaultIntensity;

        runesLightHolder.GetComponent<Light>().color = defaultColor;
        runesLightHolder.GetComponent<Light>().intensity = defaultIntensity;

        runesHolder.GetComponent<MeshRenderer>().material = defaultRunesMat;
        runesCore.GetComponent<MeshRenderer>().material = defaultRunesCoreMat;

        emirTag.GetComponent<SkinnedMeshRenderer>().material = defaultTagMat;
    }

    private void ActivateIsPlayingGhostsSettings()
    {
        spiritLightHolder.GetComponent<Light>().intensity = isPlayingGhostsIntensity;
    }

    private void DeactivateIsPlayingGhostsSettings()
    {
        spiritLightHolder.GetComponent<Light>().intensity = defaultIntensity;
    }
}
