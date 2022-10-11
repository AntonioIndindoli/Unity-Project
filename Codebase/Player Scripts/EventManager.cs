using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public delegate void playerMoving();
    public static event playerMoving WhenMoved;

    public delegate void playerSprinting();
    public static event playerSprinting WhenSprint;

    public delegate void playerleftClick();
    public static event playerleftClick WhenleftClick;

    public delegate void playerTab();
    public static event playerTab WhenTab;

    public delegate void playerMap();
    public static event playerMap WhenMap;

    void OnMove(InputValue value)
    {
        if (WhenMoved != null)
            WhenMoved();
    }

    void OnSprint(InputValue value)
    {
        if (WhenSprint != null)
            WhenSprint();
    }

    void OnLeftClick(InputValue value)
    {
        if (WhenleftClick != null)
            WhenleftClick();
    }

    void OnTab(InputValue value)
    {
        if (WhenTab != null)
            WhenTab();
    }

    void OnMap(InputValue value)
    {
        if (WhenMap != null)
            WhenMap();
    }

}
