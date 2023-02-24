using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MagicStoneTexbox : MonoBehaviour
{
    public UnityEvent ShowPanelPressE;
    public UnityEvent ShowStoryTextBox;
    public UnityEvent ClosePanelPressE;
    public UnityEvent CloseStoryTextBox;

    private bool isOnCollider;
    private bool isTextBoxActive;
    private int buttonPressedParameter = 0;

    [SerializeField] private AudioSource openStorySfx;
    [SerializeField] private AudioSource closeStorySfx;


    [Header("InteractButton")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;


    private void OnEnable()
    {
        Actions.OnRestartFromLastCp += ResetStoryWindows;
        Actions.OnPlayerDeath += ResetStoryWindows;
    }

    private void OnDisable()
    {
        Actions.OnRestartFromLastCp -= ResetStoryWindows;
        Actions.OnPlayerDeath -= ResetStoryWindows;
    }



    private void Awake()
    {
        isOnCollider = false;
        isTextBoxActive = false;
    }


    private void ResetStoryWindows()
    {
        isOnCollider = false;
        buttonPressedParameter = 0;

        if (isTextBoxActive)
        {
            CloseStoryTextBox.Invoke();
        }
        ClosePanelPressE.Invoke();
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOnCollider = true;
            ShowPanelPressE.Invoke();
        }

        var textBoxHasBeenActivated = buttonPressedParameter == 1;
        if (textBoxHasBeenActivated)
        {
            isTextBoxActive = true;
            ShowStoryTextBox.Invoke();
        }

        var textBoxHasBeenClosed = buttonPressedParameter == 2;
        if (textBoxHasBeenClosed)
        {
            isTextBoxActive = false;
            CloseStoryTextBox.Invoke();
            buttonPressedParameter = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOnCollider = false;
            buttonPressedParameter = 0;

            if (isTextBoxActive)
            {
                CloseStoryTextBox.Invoke();
                closeStorySfx.Play();
                isTextBoxActive = false;
            }

            ClosePanelPressE.Invoke();
        }
    }

    private void Update()
    {
        if (isOnCollider)
        {
            if (Input.GetButtonDown("Interact"))
            {
                buttonPressedParameter++;

                switch(buttonPressedParameter)
                {
                    case 0:
                        break;

                    case 1:
                        openStorySfx.Play();
                        break;

                    case 2:
                        closeStorySfx.Play();
                        break;
                }
            }
        }
    }
}
