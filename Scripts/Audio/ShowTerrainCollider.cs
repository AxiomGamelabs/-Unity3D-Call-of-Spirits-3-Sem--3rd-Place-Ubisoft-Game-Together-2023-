using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTerrainCollider : MonoBehaviour
{
    private BoxCollider collider;
    [SerializeField] private Color colliderColor = Color.green;
    public bool showCollider = true;
    [SerializeField] private TerrainType type;
    public TerrainType Type => type;

    public enum TerrainType
    {
        GRASS,
        ROCKS,
        WOOD
    }


    //ATTENTION: when resizing the collider via inspector don't use the edit collider tool. Keep the center at (0,0,0) and change the "size" values via inspector. Or just rescale the cp parent gameobject
    private void OnDrawGizmos()
    {
        collider = GetComponent<BoxCollider>();

        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.color = colliderColor;
        //Gizmos.DrawWireCube(transform.position, new Vector3(collider.size.x, collider.size.y, collider.size.z)); //world space
        if(showCollider)
        {
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(collider.size.x, collider.size.y, collider.size.z)); //local space
        }
    }
}
