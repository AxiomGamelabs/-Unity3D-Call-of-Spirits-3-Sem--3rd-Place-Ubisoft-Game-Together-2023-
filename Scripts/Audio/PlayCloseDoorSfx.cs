using UnityEngine;

public class PlayCloseDoorSfx : MonoBehaviour
{
    [SerializeField] private AudioSource closeDoorSfx;


    public void PlayCloseDoorSFX() 
    {
        closeDoorSfx.Play();
    }
}
