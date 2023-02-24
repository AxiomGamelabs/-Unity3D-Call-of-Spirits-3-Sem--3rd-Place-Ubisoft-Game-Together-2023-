using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private CinemachineFreeLook thirdPersonCam;


    private void Awake()
    {
        thirdPersonCam = FindObjectOfType<CinemachineFreeLook>();
    }


    private void LateUpdate()
    {
        transform.LookAt(thirdPersonCam.transform.position, Vector3.up);
    }
}
