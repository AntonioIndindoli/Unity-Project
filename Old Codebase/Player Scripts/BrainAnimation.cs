using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrainAnimation : MonoBehaviour
{
    public Image image;

    public Sprite brain1;
    public Sprite brain2;
    public Sprite brain3;
    public Sprite brain4;
    public Sprite brain5;
    private float animTimer;
    private bool forward = true;

    // Update is called once per frame
    void Update()
    {
        if(forward)
            animTimer += Time.deltaTime * 3;
        else
            animTimer -= Time.deltaTime * 3;

        if (animTimer < 1)
        {
            if (!forward)
            {
                animTimer = 0;
                forward = true;
            }
            image.sprite = brain1;
        }
        else if (animTimer < 2)
            image.sprite = brain2;
        else if(animTimer < 3)
            image.sprite = brain3;
        else if(animTimer < 4)
            image.sprite = brain4;
        else if(animTimer < 5)
            image.sprite = brain5;
        else if (animTimer < 6 && forward)
        {
            forward = false;
            animTimer = 4;
        }
    }
}
