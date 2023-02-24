using UnityEngine;

public class ObjWithEmissiveMat : MonoBehaviour
{

    private void Awake()
    {
        GetComponent<Renderer>().material.DisableKeyword("_EMISSION");

    }


    void Start()
    {
        WorldsController.instance.AddToObjWithEmissiveMatList(gameObject);
    }
}
