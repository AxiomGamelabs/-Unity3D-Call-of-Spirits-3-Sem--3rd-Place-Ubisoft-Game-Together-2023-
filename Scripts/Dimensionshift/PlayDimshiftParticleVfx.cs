using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDimshiftParticleVfx : MonoBehaviour
{
    [SerializeField] private ParticleSystem dimshiftVfx;


    private void OnEnable()
    {
        Actions.OnWorldSwitched += PlayParticleVfx;
    }

    private void OnDisable()
    {
        Actions.OnWorldSwitched -= PlayParticleVfx;
    }


    private void PlayParticleVfx()
    {
        dimshiftVfx.Play();
    }
}
