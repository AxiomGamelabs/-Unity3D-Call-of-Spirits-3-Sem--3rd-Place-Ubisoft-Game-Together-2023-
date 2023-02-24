using Cinemachine;
using UnityEngine;
using UnityEngine.Android;

public class ActivationBtn : MonoBehaviour
{
    private bool isActive = false;
    public bool IsActive => isActive;

    [SerializeField] private ParticleSystem dustLineVfx;
    [SerializeField] private ParticleSystem dustCircularVfx;
    [SerializeField] private GameObject dustLight;

    private bool isPlayingActivationSfx;
    [SerializeField] private AudioSource activationSfx;


    public MemPathDetector memPathDetector;

    [SerializeField] private bool isRedBtn;
    [SerializeField] private bool isWhiteBtn;
    private bool whiteBtnUsed;
    [SerializeField] private bool isWhiteKeyBtn; //whiteKeyBtns are also whiteBtns
    [SerializeField] private bool isWhiteBossCageBtn;
    [SerializeField] private bool isBridgeBtn;



    [SerializeField] private bool isVictoryMenuWhiteBtn;
    [SerializeField] private GameObject victoryScreen;



    [Header("### Ignore if not a red Button ###")]

    [HideInInspector] public bool isRelevantForTheEncounter = false; //always false if we are not inside its encounter. Every cp knows the buttons that are relevant for the encounter
    [SerializeField] private int btnIdInTheEncounter;

    [SerializeField] private Material redMat; 
    [SerializeField] private Material greenMat;

    [SerializeField] private Material redDustLineMat;
    [SerializeField] private Material redDustCircularMat;

    [SerializeField] private Material greenDustLineMat;
    [SerializeField] private Material greenDustCircularMat;

    [SerializeField] private bool isNormalOnly;
    [SerializeField] private bool isSpiritOnly;

    [Header("### Ignore if present in both worlds ###")]
    [SerializeField] private Material otherWorldMaterial;




    private void OnEnable()
    {
        Actions.OnRestartFromLastCp += ReactivateParticles;
        Actions.OnRestartFromLastCp += ResetSfx;
        Actions.OnRestartFromLastCp += ResetWhiteBtnUsed;


        Actions.OnPlayerDeath += ReactivateParticles;
        Actions.OnPlayerDeath += ResetSfx;
        Actions.OnPlayerDeath += ResetWhiteBtnUsed;

        Actions.OnWorldSwitched += RunesColorActivationCheck;
    }


    private void OnDisable()
    {
        isActive = false;

        Actions.OnRestartFromLastCp -= ReactivateParticles;
        Actions.OnRestartFromLastCp -= ResetSfx;
        Actions.OnRestartFromLastCp += ResetWhiteBtnUsed;


        Actions.OnPlayerDeath -= ReactivateParticles;
        Actions.OnPlayerDeath -= ResetSfx;
        Actions.OnPlayerDeath -= ResetWhiteBtnUsed;


        Actions.OnWorldSwitched -= RunesColorActivationCheck;
    }



    private void Update()
    {
        if (!IsCollidingWithPlayerOrGhost()) //if the ghost dies while on the button, we want the door to close
        {
            if (isRedBtn && isRelevantForTheEncounter) //we dont want buttons from other encounters to forbid the activation of the hud element
            {
                Actions.OnRedActivationBtnExit?.Invoke(btnIdInTheEncounter);
            }
            isActive = false;

        }

        if (GetComponent<MeshCollider>().enabled == false) //if we are on the button and dimensionshift makes the btn disappear, we want to deactivate the btn
        {
            if (isRedBtn && isRelevantForTheEncounter)
            {
                Actions.OnRedActivationBtnExit?.Invoke(btnIdInTheEncounter);
            }
            isActive = false;

        }

        if (GetComponent<MeshCollider>().enabled == true && IsCollidingWithPlayerOrGhost()) //if we are standing on the btn and dimensionshift makes the btn appear, we want to activate it 
        {
            if (isRedBtn && isRelevantForTheEncounter)
            {
                Actions.OnRedActivationBtnEnter?.Invoke(btnIdInTheEncounter);
            }
            isActive = true;

        }


        if (isActive)
        {
            if(!isPlayingActivationSfx)
            {
                activationSfx.Stop();
                activationSfx.Play();
                isPlayingActivationSfx = true;
            }



            var lineParticles = dustLineVfx.emission;
            lineParticles.enabled = false;

            var circleParticles = dustCircularVfx.emission;
            circleParticles.enabled = false;

            if (isWhiteBtn)
            {
                DeactivateWhiteRunes();
            }

            if (isVictoryMenuWhiteBtn)
            {
                GameController.instance.hasWon = true;
                HUD.instance.gameObject.GetComponent<Canvas>().enabled = false;
                Invoke(nameof(DisplayVictoryMenu), 2);
            }

            

        }
        else //NOTHING IS ON THE BUTTON (THE BTN IS NOT ACTIVE)
        {
            if (isWhiteBtn)
            {
                if (whiteBtnUsed) //we dont want to reactivate emissions or sfx on used white btns
                {
                    Material mymat = GetComponent<Renderer>().material;
                    mymat.SetColor("_EmissionColor", Color.black); //deactivates white rune
                }

                if (isWhiteBossCageBtn)
                {
                    var lineParticles = dustLineVfx.emission;
                    lineParticles.enabled = true;

                    var circleParticles = dustCircularVfx.emission;
                    circleParticles.enabled = true;
                }
            }
            else
            {
                isPlayingActivationSfx = false;

                var lineParticles = dustLineVfx.emission;
                lineParticles.enabled = true;

                var circleParticles = dustCircularVfx.emission;
                circleParticles.enabled = true;

            }
        }



        if (isRedBtn)
        {
            ToggleColor();
        }
    }


    private void DisplayVictoryMenu()
    {
        victoryScreen.SetActive(true);

        //PlayerCam script also manipulates cursor on update
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void DeactivateWhiteRunes()
    {
        Material mymat = GetComponent<Renderer>().material;
        mymat.SetColor("_EmissionColor", Color.black); //deactivates white rune
        dustLight.SetActive(false);
    }

    private void RunesColorActivationCheck()
    {
        if(isWhiteBtn)
        {
            if(whiteBtnUsed)
            {
                DeactivateWhiteRunes();
            }
        }
    }


    private void ToggleColor()
    {
        if(!isNormalOnly && !isSpiritOnly)
        {
            if (isActive)
            {
                GetComponent<Renderer>().material = greenMat;
                dustLineVfx.GetComponent<Renderer>().material = greenDustLineMat;
                dustCircularVfx.GetComponent<Renderer>().material = greenDustCircularMat;
            }
            else
            {
                GetComponent<Renderer>().material = redMat;
                dustLineVfx.GetComponent<Renderer>().material = redDustLineMat;
                dustCircularVfx.GetComponent<Renderer>().material = redDustCircularMat;
            }
        }
        else
        {
            if (isNormalOnly && WorldsController.instance.NormalWorldIsActive || isSpiritOnly && !WorldsController.instance.NormalWorldIsActive)
            {
                if (isActive)
                {
                    GetComponent<Renderer>().material = greenMat;
                    dustLineVfx.GetComponent<Renderer>().material = greenDustLineMat;
                    dustCircularVfx.GetComponent<Renderer>().material = greenDustCircularMat;
                }
                else
                {
                    GetComponent<Renderer>().material = redMat;
                    dustLineVfx.GetComponent<Renderer>().material = redDustLineMat;
                    dustCircularVfx.GetComponent<Renderer>().material = redDustCircularMat;
                }
            }
            else
            {
                GetComponent<Renderer>().material = otherWorldMaterial;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") | other.CompareTag("Ghost"))
        {
            if (isRedBtn)
            {
                Actions.OnRedActivationBtnEnter?.Invoke(btnIdInTheEncounter);
            }

            if(isWhiteBtn)
            {
                whiteBtnUsed = true;
            }

            if (isWhiteKeyBtn)
            {
                Actions.OnWhiteKeyBtnEnter?.Invoke(btnIdInTheEncounter);
            }
            isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") | other.CompareTag("Ghost"))
        {
            if (isRedBtn)
            {
                Actions.OnRedActivationBtnExit?.Invoke(btnIdInTheEncounter);
            }
            isActive = false;

        }
    }



    private bool IsCollidingWithPlayerOrGhost()
    {
        Collider[] cols = Physics.OverlapBox(transform.position, new Vector3(1, 1, 1));
        foreach (Collider col in cols)
        {
            if (col.gameObject.CompareTag("Player") | col.gameObject.CompareTag("Ghost"))
            {
                return true;
            }
        }
        return false;
    }

    private void ResetSfx()
    {
        if (!isBridgeBtn)
        {
            isPlayingActivationSfx = false;
        }
    }


    private void ResetWhiteBtnUsed()
    {
        if(!isBridgeBtn)
        {
            whiteBtnUsed = false;
        }
    }


    private void ReactivateParticles()
    {
        if (!isBridgeBtn)
        {
            var lineParticles = dustLineVfx.emission;
            lineParticles.enabled = true;

            var circleParticles = dustCircularVfx.emission;
            circleParticles.enabled = true;

            if (isWhiteBtn)
            {
                Material mymat = GetComponent<Renderer>().material;
                mymat.SetColor("_EmissionColor", Color.white);

                dustLight.SetActive(true);
            }
        }
    }
}
