using UnityEngine;

public class MemPathDetector : MonoBehaviour
{

    private bool isMemPathDetected;
    public bool IsMemPathDetected => isMemPathDetected;



    private void Update()
    {
        if (!IsCollidingWithMemPath()) //if the ghost dies while on the button, we want the door to close
        {
            isMemPathDetected = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MemPath"))
        {
            isMemPathDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MemPath"))
        {
            Debug.Log("MemPathExited");

            isMemPathDetected = false;
        }
    }


    private bool IsCollidingWithMemPath()
    {
        Collider[] cols = Physics.OverlapBox(transform.position, new Vector3(1, 1, 1));
        foreach (Collider col in cols)
        {
            if (col.gameObject.CompareTag("MemPath"))
            {
                return true;
            }
        }
        return false;
    }
}
