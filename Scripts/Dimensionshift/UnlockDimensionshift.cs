using UnityEngine;

public class UnlockDimensionshift : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Actions.OnDimensionshiftUnlocked?.Invoke();
        }
    }
}
