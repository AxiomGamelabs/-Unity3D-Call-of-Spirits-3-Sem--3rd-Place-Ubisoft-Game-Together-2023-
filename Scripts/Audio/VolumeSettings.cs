using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    [Header("Sliders")]
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider uiSfxSlider;


    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";
    public const string MIXER_UISFX = "UIVolume";


    //this script does the SAVING of the volume values


    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        uiSfxSlider.onValueChanged.AddListener(SetUiSfxVolume);
    }

    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);  
        uiSfxSlider.value = PlayerPrefs.GetFloat(AudioManager.UISFX_KEY, 1f);
    }


    private void OnDisable()
    {
        //here happens the saving
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfxSlider.value);
        PlayerPrefs.SetFloat(AudioManager.UISFX_KEY, uiSfxSlider.value);
    }


    private void SetMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);

        //here happens the saving
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
    }

    private void SetSfxVolume(float value)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);

        //here happens the saving
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfxSlider.value);
    }

    private void SetUiSfxVolume(float value)
    {
        mixer.SetFloat(MIXER_UISFX, Mathf.Log10(value) * 20);

        //here happens the saving
        PlayerPrefs.SetFloat(AudioManager.UISFX_KEY, uiSfxSlider.value);
    }

    //public void ToggleMuteMusic()
    //{
    //    isMusicMuted = !isMusicMuted;

    //    if (isMusicMuted) //we have to mute music
    //    {
    //        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY_LAST, musicSlider.value);
    //        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(0) * 20);
    //        musicSlider.value = 0;
    //        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
    //    }
    //    else //we have to unmute music
    //    {
    //        float musicVolume = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY_LAST, 1f); //temp save
    //        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
    //        musicSlider.value = musicVolume;
    //        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
    //    }
    //}

}
