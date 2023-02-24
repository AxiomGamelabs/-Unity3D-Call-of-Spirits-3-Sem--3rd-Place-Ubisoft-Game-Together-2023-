using UnityEngine;

public class Trans : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float animationSpeed;



    private void OnEnable()
    {
        Actions.OnWorldSwitched += PlayTimeswitchVfx;
    }

    private void OnDisable()
    {
        Actions.OnWorldSwitched -= PlayTimeswitchVfx;
    }



    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnValidate()
    {
        anim.speed = animationSpeed;    
    }



    public void PlayTimeswitchVfx()
    {
        if (WorldsController.instance.NormalWorldIsActive)
        {
            anim.SetTrigger("toNormal"); 
        }
        else
        {
            anim.SetTrigger("toGhost");
        }
    }


}


    




