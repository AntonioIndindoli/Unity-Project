using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureMoving : MonoBehaviour
{
    float scrollSpeed = .01f;
    float direction = 1;
    private int sanityLevel = 0;
    Renderer rend;
    float animTimer;
    private bool ObjectVisible;
    private bool doOnce = false;
    private Vector3 funcVector;

    public int index1;
    public int index2;
    public int index3;
    public int index4;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        funcVector = new Vector3(1.0f, 1.0f, 1.0f);
    }

    void LateUpdate()
    {
        if (sanityLevel > 10)
        {
            scrollSpeed = .01f;
        }
        if (sanityLevel > 25)
        {
            scrollSpeed = .03f;
        }
            if (sanityLevel > 5)
        {
            doOnce = false;
            animTimer -= Time.deltaTime;

            funcVector.x += scrollSpeed;
            funcVector.y = Mathf.Sin(funcVector.x);

            if (animTimer < 0 && ObjectVisible)
            {
                animTimer += .06f;

                float offset = funcVector.y * scrollSpeed;

                rend.materials[index1].SetTextureOffset("_MainTex", new Vector2(offset, 0));

                if (index2 != index1)
                    rend.materials[index2].SetTextureOffset("_MainTex", new Vector2(offset, 0));

                if (index3 != index1)
                    rend.materials[index3].SetTextureOffset("_MainTex", new Vector2(offset, 0));

                if(index4 != index1)
                    rend.materials[index4].SetTextureOffset("_MainTex", new Vector2(offset, 0));

            }
        }
        else if (!doOnce)
        {
            doOnce = true;
            rend.materials[index1].SetTextureOffset("_MainTex", new Vector2(0, 0));

            if (index2 != index1)
                rend.materials[index2].SetTextureOffset("_MainTex", new Vector2(0, 0));

            if (index3 != index1)
                rend.materials[index3].SetTextureOffset("_MainTex", new Vector2(0, 0));

            if (index4 != index1)
                rend.materials[index4].SetTextureOffset("_MainTex", new Vector2(0, 0));
        }
    }

    void OnBecameInvisible()
    {
        ObjectVisible = false;
    }

    void OnBecameVisible()
    {
        ObjectVisible = true;
    }

    void OnEnable()
    {
        SanityScript.WhenSanityLowered += OnLowered;
        SanityScript.WhenSanityRaised += OnRaised;
    }

    void OnDisable()
    {
        SanityScript.WhenSanityLowered -= OnLowered;
        SanityScript.WhenSanityRaised -= OnRaised;
    }

    void OnLowered(int sanity)
    {
        sanityLevel = sanity;
    }

    void OnRaised(int sanity)
    {
        sanityLevel = sanity;
    }
}
