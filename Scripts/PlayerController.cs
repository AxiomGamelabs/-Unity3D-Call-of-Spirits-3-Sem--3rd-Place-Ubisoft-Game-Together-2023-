using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private bool isDead = false;
    public bool IsDead => isDead;




    private void OnEnable()
    {
        Actions.OnPlayerDeath += KillPlayer;
    }

    private void OnDisable()
    {
        Actions.OnPlayerDeath -= KillPlayer;
    }



    private void Awake()
    {
        instance = this;
    }

    public void KillPlayer()
    {
        GameController.instance.deathSfx.Play();
        gameObject.SetActive(false);
        transform.SetParent(null); //unparent player from moving platforms when dying on them
        isDead = true;

        SpawnGhost.instance.DestroyAllGhosts();


        HUD.instance.stopMemBtnTxt.SetActive(false);


        foreach(Record rec in Recordings.instance.recordings)
        {
            rec.SetIsRecording(false);
        }

    }

    public void SetIsDead(bool value)
    {
        isDead = value;
    }

}
