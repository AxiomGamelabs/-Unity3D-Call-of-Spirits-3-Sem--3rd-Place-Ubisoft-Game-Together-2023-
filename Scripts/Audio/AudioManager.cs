using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;


    [SerializeField] AudioMixer mixer;

    public const string MUSIC_KEY = "musicVolume"; //current value

    public const string SFX_KEY = "sfxVolume";
    public const string UISFX_KEY = "uiSfxVolume";

    private bool isVolumeLoadedTheFirstTime = false;

    //this script does the LOADING of the volume values



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadVolume();
    }

    private void Update()
    {
        if (!isVolumeLoadedTheFirstTime)
        {
            LoadVolume();
            isVolumeLoadedTheFirstTime = true;
        }
    }

    void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        float uiSfxVolume = PlayerPrefs.GetFloat(UISFX_KEY, 1f);



        mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
        mixer.SetFloat(VolumeSettings.MIXER_UISFX, Mathf.Log10(uiSfxVolume) * 20);
    }
}
