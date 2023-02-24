using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsVisualizationTool : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float sphereRadius;


    void OnDrawGizmos()
    {
        foreach (GameObject waypoint in waypoints)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(waypoint.transform.position, sphereRadius);
        }

    }
}
