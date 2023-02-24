using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISfxController : MonoBehaviour
{
    public static UISfxController instance;


    [SerializeField] private AudioSource hoverSfx;
    [SerializeField] private AudioSource clickSfx;



    private void Start()
    {
        hoverSfx.ignoreListenerPause = true;
        clickSfx.ignoreListenerPause = true;
    }

    public void PlayHoverSound()
    {
        hoverSfx.Stop();
        hoverSfx.Play();
    }

    public void PlayClickSound()
    {
        clickSfx.Stop();
        clickSfx.Play();
    }
}
