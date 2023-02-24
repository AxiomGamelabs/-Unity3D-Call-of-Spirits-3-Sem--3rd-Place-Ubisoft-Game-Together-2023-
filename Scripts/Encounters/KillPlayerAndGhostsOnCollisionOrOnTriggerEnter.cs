using UnityEngine;

public class KillPlayerAndGhostsOnCollisionOrOnTriggerEnter : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Actions.OnPlayerDeath?.Invoke();
        }

        else if (collision.gameObject.CompareTag("Ghost"))
        {
            var deathGhostId = collision.gameObject.GetComponent<GhostController>().GhostId;
            Actions.OnGhostDeath?.Invoke(deathGhostId);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Actions.OnPlayerDeath?.Invoke();
        }

        else if (other.gameObject.CompareTag("Ghost"))
        {
            var deathGhostId = other.gameObject.transform.parent.GetComponent<GhostController>().GhostId;
            Actions.OnGhostDeath?.Invoke(deathGhostId);
        }
    }
}

