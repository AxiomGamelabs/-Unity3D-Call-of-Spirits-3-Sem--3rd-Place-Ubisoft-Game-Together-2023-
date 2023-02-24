using UnityEngine;


public class Recordings : MonoBehaviour
{
    public static Recordings instance;
    public Record[] recordings;

    [SerializeField] private KeyCode recordKey;
    public KeyCode RecordKey => recordKey;
    [SerializeField] private float waitTimeBeforeCanPlay;
    public float WaitTimeBeforeCanPlay => waitTimeBeforeCanPlay;


    [SerializeField] private int recMaxDuration;
    public int RecMaxDuration => recMaxDuration;

    [Header("Sfx")]
    [SerializeField] private AudioSource startRecSfx;
    [SerializeField] private AudioSource stopRecSfx;
    [SerializeField] private AudioSource recTimerExpiredSfx;
    [SerializeField] private AudioSource cannotRecOutsideTimefieldSfx;



    public Record GetActiveRecording()
    {
        if (recordings[0].IsRecording)
        {
            return recordings[0];
        }
        if (recordings[1].IsRecording)
        {
            return recordings[1];
        }
        return null;
    }


    private void Awake()
    {
        instance = this;
    }


    public void PlayStartRecSfx()
    {
        startRecSfx.Play();
    }
    public void PlayStopRecSfx()
    {
        stopRecSfx.Play();
    }
    public void PlayRecTimerExpiredSfx()
    {
        recTimerExpiredSfx.Play();
    }
    public void PlayCannotRecOutsideTimefieldSfx()
    {
        cannotRecOutsideTimefieldSfx.Play();
    }

}
