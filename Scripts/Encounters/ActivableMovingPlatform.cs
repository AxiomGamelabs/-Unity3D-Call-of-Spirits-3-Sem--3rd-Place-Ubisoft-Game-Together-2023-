using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableMovingPlatform : MonoBehaviour
{
    [SerializeField] private List<ActivationBtn> activationBtns;
    [SerializeField] float speed = 1f;

    [SerializeField] GameObject[] waypoints;
    int currentWaypointIndex = 0; //waypoint we are moving to
    [SerializeField] private GameObject mesh;

    [SerializeField] private GameObject outlineVfxPresent;
    [SerializeField] private GameObject outlineVfxNotPresent;

    [SerializeField] private ParticleSystem activationParticlesVfx;

    [SerializeField] private AudioSource movingPlatformSfx;
    private bool isPlayingSfx;
    private float movementVel;

    [SerializeField] private bool isOneWay;
    [SerializeField] private bool isDeco = false;


    [SerializeField] private bool isNormalOnly;
    [SerializeField] private bool isSpiritOnly;

    private bool isParticleEmitted = false;




    void FixedUpdate()
    {
        if (HasToBeActive())
        {

            var spiritInSpiritVar = isSpiritOnly && !WorldsController.instance.NormalWorldIsActive;
            var normalInNormalVar = isNormalOnly && WorldsController.instance.NormalWorldIsActive;
            var inBothVar = !isNormalOnly && !isSpiritOnly;



            foreach (ActivationBtn btn in activationBtns)
            {
                if (btn.memPathDetector.IsMemPathDetected)
                {
                    if (spiritInSpiritVar || normalInNormalVar || inBothVar)
                    {
                        outlineVfxNotPresent.SetActive(false);
                        outlineVfxPresent.SetActive(true);
                    }
                    else
                    {
                        outlineVfxPresent.SetActive(false);
                        outlineVfxNotPresent.SetActive(true);
                    }
                }
            }


            if (!isDeco)
            {
                if (spiritInSpiritVar || normalInNormalVar || inBothVar)
                {
                    outlineVfxNotPresent.SetActive(false);
                    outlineVfxPresent.SetActive(true);
                }
                else
                {
                    outlineVfxPresent.SetActive(false);
                    outlineVfxNotPresent.SetActive(true);
                }
            }

            if (isSpiritOnly && !WorldsController.instance.NormalWorldIsActive)
            {
                currentWaypointIndex = 1;
            }
            else if (isNormalOnly && WorldsController.instance.NormalWorldIsActive)
            {
                currentWaypointIndex = 1;
            }
            else if(!isNormalOnly && !isSpiritOnly)
            {
                currentWaypointIndex = 1;
            }
        }
        else
        {
            if(!isDeco)
            {

                var spiritInSpiritVarB = isSpiritOnly && !WorldsController.instance.NormalWorldIsActive;
                var normalInNormalVarB = isNormalOnly && WorldsController.instance.NormalWorldIsActive;
                var inBothVarB = !isNormalOnly && !isSpiritOnly;

                foreach (ActivationBtn btn in activationBtns)
                {
                    if (btn.memPathDetector.IsMemPathDetected)
                    {
                        if (spiritInSpiritVarB || normalInNormalVarB || inBothVarB)
                        {
                            outlineVfxNotPresent.SetActive(false);
                            outlineVfxPresent.SetActive(true);
                        }
                        else
                        {
                            outlineVfxPresent.SetActive(false);
                            outlineVfxNotPresent.SetActive(true);
                        }
                    }
                    else
                    {

                        outlineVfxPresent.SetActive(false);
                        outlineVfxNotPresent.SetActive(false);

                    }
                }
            }


            if (!isOneWay)
            {
                currentWaypointIndex = 0;
            }
        }

        var spiritInSpirit = isSpiritOnly && !WorldsController.instance.NormalWorldIsActive;
        var normalInNormal = isNormalOnly && WorldsController.instance.NormalWorldIsActive;
        var inBoth = !isNormalOnly && !isSpiritOnly;


        var isMoving = (spiritInSpirit || normalInNormal || inBoth);
        if (isMoving)
        {
            mesh.transform.position = Vector3.MoveTowards(mesh.transform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
        }

        if (!isDeco)
        {

            var activationParticles = activationParticlesVfx.emission;

            if (activationBtns[0].IsActive)
            {
                if (!isParticleEmitted)
                {
                    activationParticlesVfx.Play();
                    isParticleEmitted = true;
                }
            }
            else
            {
                isParticleEmitted = false;
            }
        }
    }





    private bool HasToBeActive()
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


    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(waypoints[0].transform.position, waypoints[1].transform.position);
    }


}
