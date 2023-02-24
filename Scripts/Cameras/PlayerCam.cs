using Cinemachine;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject thirdPersonCam;
    [SerializeField] private KeyCode thirdPersonCamKey;






    private void Update()
    {
        if (PauseMenu.isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            if (!GameController.instance.hasWon)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }


        if(GameController.instance.hasWon)
        {
            thirdPersonCam.GetComponent<CinemachineFreeLook>().enabled = false;
        }


    }

}
