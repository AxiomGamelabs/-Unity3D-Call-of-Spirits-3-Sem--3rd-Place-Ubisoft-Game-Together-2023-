using System.Collections;
using UnityEngine;

public class Timefield : MonoBehaviour
{
    [SerializeField] private ColorType color = ColorType.BLUE;
    [SerializeField] private GameObject playerRespawnPos;




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var weAreNotRecording = Recordings.instance.recordings[0].IsRecording == false && Recordings.instance.recordings[1].IsRecording == false;
            if (weAreNotRecording) //if we are not recording. (we don't want to be able to record if we are already recording)
            {
                switch (color)
                {
                    case ColorType.BLUE: 
                        Recordings.instance.recordings[0].canRecord = true;
                        break;
                    case ColorType.ORANGE:
                        Recordings.instance.recordings[1].canRecord = true;
                        break;
                }

                Actions.OnTimefieldEntered?.Invoke(playerRespawnPos); //we communicate, where the player should be respawned when finishing recording.

                HUD.instance.startMemBtnTxt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (color)
            {
                case ColorType.BLUE:
                    Recordings.instance.recordings[0].canRecord = false;
                    break;
                case ColorType.ORANGE:
                    Recordings.instance.recordings[1].canRecord = false;
                    break;
            }

            HUD.instance.startMemBtnTxt.SetActive(false);
        }

    }


    public enum ColorType
    {
        BLUE,
        ORANGE
    }
}
