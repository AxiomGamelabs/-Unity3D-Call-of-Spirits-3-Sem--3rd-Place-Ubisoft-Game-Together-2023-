using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavesSwitcher : MonoBehaviour
{
    [SerializeField] private Material[] leavesMaterials; //index 0 = normal world mat, index 1 = spirit world mat

    private void OnEnable()
    {
        Actions.OnWorldSwitched += ChangeMaterial;
    }

    private void OnDisable()
    {
        Actions.OnWorldSwitched -= ChangeMaterial;
    }

    private void ChangeMaterial()
    {
        if (WorldsController.instance.NormalWorldIsActive) //if we switched to normal world
        {
            GetComponent<Renderer>().material = leavesMaterials[0];
        }
        else
        {
            GetComponent<Renderer>().material = leavesMaterials[1];
        }
    }



}
