using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInventory : MonoBehaviour
{
    bool inventoryShowing;
    Image[] images;

    void OnEnable()
    {
        EventManager.WhenTab += OnTab;
    }
    void OnDisable()
    {
        EventManager.WhenTab -= OnTab;
    }

    void Start()
    {
        inventoryShowing = false;
        images = this.gameObject.GetComponentsInChildren<Image>();

        for (int i = 0; i < images.Length; i++)
        {
            images[i].enabled = false;
        }
    }

    void OnTab()
    {
        if (inventoryShowing)
        {
            inventoryShowing = false;
            images = this.gameObject.GetComponentsInChildren<Image>();


            for (int i = 0; i < images.Length; i++)
            {
                images[i].enabled = false;
            }
        }
        else
        {
            inventoryShowing = true;
            images = this.gameObject.GetComponentsInChildren<Image>();

            for (int i = 0; i < images.Length; i++)
            {
                images[i].enabled = true;
            }
        }
    }
}
