using UnityEngine;

public class OneWorldOnly : MonoBehaviour
{

    [SerializeField] private World worldOfThePrefab;

    [Header("Ignore if Skybox")]
    [SerializeField] private Material itIsPresentMat;
    [SerializeField] private Material otherWorldMat;

    [SerializeField] private GameObject[] prefabMeshes;
    [SerializeField] private bool hasToDisplayOneWorldOnlyFeedback;
    public bool HasToDisplayOneWorldOnlyFeedback => hasToDisplayOneWorldOnlyFeedback;


    public enum World
    {
        NORMAL,
        SPIRIT
    }

    private void OnEnable()
    {
        Actions.OnWorldSwitched += ChangeMaterials;
    }

    private void OnDisable()
    {
        Actions.OnWorldSwitched -= ChangeMaterials;
    }


    void Start()
    {
        if(worldOfThePrefab == World.NORMAL)
        {
            WorldsController.instance.AddToNormalOnlyList(gameObject);
        }
        else if (worldOfThePrefab == World.SPIRIT)
        {
            WorldsController.instance.AddToSpiritOnlyList(gameObject);

            if (!hasToDisplayOneWorldOnlyFeedback)
            {
                gameObject.SetActive(false);
            }
        }
    }


    private void ChangeMaterials()
    {
        if(hasToDisplayOneWorldOnlyFeedback) 
        {
            if (WorldsController.instance.NormalWorldIsActive && worldOfThePrefab == World.NORMAL) //if we switched to normal world and this is a "normal only" prefab
            {
                foreach (GameObject mesh in prefabMeshes)
                {
                    mesh.GetComponent<Renderer>().material = itIsPresentMat; //display "it is present" mat
                    if (mesh.GetComponent<ParticleSystem>() == null) //if it is not a particle system
                    {
                        mesh.GetComponent<MeshCollider>().enabled = true; //activate collider
                    }
                }

            }

            if (WorldsController.instance.NormalWorldIsActive && worldOfThePrefab == World.SPIRIT) //if we switched to normal world and this is a "spirit only" prefab
            {
                foreach (GameObject mesh in prefabMeshes)
                {
                    mesh.GetComponent<Renderer>().material = otherWorldMat; //display "other world" mat
                    if (mesh.GetComponent<ParticleSystem>() == null) //if it is not a particle system
                    {
                        mesh.GetComponent<MeshCollider>().enabled = false; //deactivate collider
                    }
                }
            }


            if (!WorldsController.instance.NormalWorldIsActive && worldOfThePrefab == World.NORMAL) //if we switched to spirit world and this is a "normal only" prefab
            {
                foreach (GameObject mesh in prefabMeshes)
                {
                    mesh.GetComponent<Renderer>().material = otherWorldMat; //display "other world" mat
                    if (mesh.GetComponent<ParticleSystem>() == null) //if it is not a particle system
                    {
                        mesh.GetComponent<MeshCollider>().enabled = false; //deactivate collider
                    }
                }
            }


            if (!WorldsController.instance.NormalWorldIsActive && worldOfThePrefab == World.SPIRIT) //if we switched to spirit world and this is a "spirit only" prefab
            {
                foreach (GameObject mesh in prefabMeshes)
                {
                    mesh.GetComponent<Renderer>().material = itIsPresentMat; //display "it is present" mat
                    if (mesh.GetComponent<ParticleSystem>() == null) //if it is not a particle system
                    {
                        mesh.GetComponent<MeshCollider>().enabled = true; //activate collider
                    }
                }
            }
        }
    }

}
