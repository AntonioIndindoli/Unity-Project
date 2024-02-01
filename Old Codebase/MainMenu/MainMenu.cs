using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button optionsButton;
    public Button quitButton;

    public TMP_Text startText;
    public TMP_Text optionsText;
    public TMP_Text quitText;

    public GameObject optionsMenu;
    public GameObject mainMenu;

    public void OnStart()
    {
        print("start!");
        Application.LoadLevel(1);
    }
    public void OnOptions()
    {
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);

    }
    public void OnExitOptions()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);

    }
    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnQuitToMenu()
    {
        Application.LoadLevel(0);
    }
}
