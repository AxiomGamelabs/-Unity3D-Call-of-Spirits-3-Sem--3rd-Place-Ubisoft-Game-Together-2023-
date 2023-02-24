using UnityEngine;

public class PlayOpenDoorSfx : MonoBehaviour
{
    [SerializeField] private AudioSource openDoorSfx;


    public void PlayOpenDoorSFX()
    {
        openDoorSfx.Play();
    }
}
