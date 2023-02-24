using Unity.VisualScripting;
using UnityEngine;

public class Follower : MonoBehaviour
{
    private GameObject targetObj;



    private void Update()
    {
        transform.position = targetObj.transform.position;
    }

    public void SetFollowerTarget(GameObject target)
    {
        targetObj = target;
    }

}
