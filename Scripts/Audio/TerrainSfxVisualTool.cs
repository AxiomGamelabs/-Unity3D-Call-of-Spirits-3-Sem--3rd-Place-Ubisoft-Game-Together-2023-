using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSfxVisualTool : MonoBehaviour
{
    [SerializeField] private bool showGrass = true;
    [SerializeField] private bool showRocks = true;
    [SerializeField] private bool showWood = true;



    private void OnValidate()
    {
        var sfxLayersNr = transform.childCount;
        for (int i = 0; i < sfxLayersNr; i++)
        {
            if (transform.GetChild(i).GetComponent<ShowTerrainCollider>().Type == ShowTerrainCollider.TerrainType.GRASS)
            {
                transform.GetChild(i).GetComponent<ShowTerrainCollider>().showCollider = showGrass;
            }
            else if (transform.GetChild(i).GetComponent<ShowTerrainCollider>().Type == ShowTerrainCollider.TerrainType.ROCKS)
            {
                transform.GetChild(i).GetComponent<ShowTerrainCollider>().showCollider = showRocks;
            }
            else if (transform.GetChild(i).GetComponent<ShowTerrainCollider>().Type == ShowTerrainCollider.TerrainType.WOOD)
            {
                transform.GetChild(i).GetComponent<ShowTerrainCollider>().showCollider = showWood;
            }
        }

    }

}


