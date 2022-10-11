using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using StarterAssets;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuScreen;
    public bool inMenu = false;
    public bool doOnce = false;
    public StarterAssetsInputs userInputs;
    public PlayerInput playerInput;
    public Camera mainCamera;
    public Camera menuCamera;
    public Image crosshairImage;

    // Start is called before the first frame update
    void Start()
    {
        inMenu = false;
        menuScreen.SetActive(false);
        userInputs.inPauseMenu = false;
        playerInput.camera = mainCamera;
        playerInput.SwitchCurrentActionMap("Player");
        userInputs.cursorInputForLook = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnPauseMenu(InputValue value)
    {
        
        if (!doOnce)
        {
            doOnce = true;
            //print("called");

            if (!inMenu)
            {
                print("called");
                menuScreen.SetActive(true);
                inMenu = true;
                userInputs.inPauseMenu = true;
                playerInput.camera = menuCamera;
                playerInput.SwitchCurrentActionMap("UI");
                Cursor.lockState = CursorLockMode.None;
                crosshairImage.enabled = false;
            }

        }
        else
        {
            doOnce = false;
        }
        

    }

    void OnExit(InputValue value)
    {
        if (inMenu)
        {
            inMenu = false;
            menuScreen.SetActive(false);
            userInputs.inPauseMenu = false;
            playerInput.camera = mainCamera;
            playerInput.SwitchCurrentActionMap("Player");
            userInputs.cursorInputForLook = true;
            Cursor.lockState = CursorLockMode.Locked;
            crosshairImage.enabled = true;
            Cursor.visible = false;
        }
    }

}
