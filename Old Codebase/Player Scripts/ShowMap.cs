using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMap : MonoBehaviour
{
    bool mapShowing;
    Image[] images;
    public Camera mapCamera;

    void OnEnable()
    {
        EventManager.WhenMap += OnMap;
    }
    void OnDisable()
    {
        EventManager.WhenMap -= OnMap;
    }

    void Start()
    {
        mapShowing = false;
        images = this.gameObject.GetComponentsInChildren<Image>();

        mapCamera.enabled = false;

        for (int i = 0; i < images.Length; i++)
        {
            images[i].enabled = false;
        }
    }

    void OnMap()
    {
        if (mapShowing)
        {
            mapShowing = false;
            images = this.gameObject.GetComponentsInChildren<Image>();

            mapCamera.enabled = false;

            for (int i = 0; i < images.Length; i++)
            {
                images[i].enabled = false;
            }
        }
        else
        {
            mapShowing = true;
            images = this.gameObject.GetComponentsInChildren<Image>();

            mapCamera.enabled = true;

            for (int i = 0; i < images.Length; i++)
            {
                images[i].enabled = true;
            }
        }
    }
}
