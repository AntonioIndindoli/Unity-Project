using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public delegate void CameraOn(bool cameraStatus);
    public static event CameraOn WhenCameraOn;

    private bool cameraOn = false;
    private bool clickOnce = true;
    private bool monsterVisible = false;
    private int batteryLife = 100;
    private float timeElapsed;
    private float timeElapsedFlash;
    public Image viewFinder;
    public Light cameraFlashobj;

    private Texture2D destinationTexture1;
    private Texture2D destinationTexture2;
    private Texture2D destinationTexture3;
    private Texture2D destinationTexture4;
    private Texture2D destinationTexture5;
    private Texture2D destinationTexture6;

    private bool isPerformingScreenGrab;

    private bool photo1 = true;
    private bool photo2 = false;
    private bool photo3 = false;
    private bool photo4 = false;
    private bool photo5 = false;
    private bool photo6 = false;
    private bool outOfPics = false;

    public Image rating1;
    public Image rating2;
    public Image rating3;
    public Image rating4;
    public Image rating5;
    public Image rating6;

    public Image photoDisplay1;
    public Image photoDisplay2;
    public Image photoDisplay3;
    public Image photoDisplay4;
    public Image photoDisplay5;
    public Image photoDisplay6;

    public Sprite noStar;
    public Sprite oneStar;
    public Sprite twoStar;
    public Sprite threeStar;

    private int rating = 0;

    void Start()
    {
        // Create a new Texture2D with the width and height of the screen, and cache it for reuse
        destinationTexture1 = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        destinationTexture2 = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        destinationTexture3 = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        destinationTexture4 = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        destinationTexture5 = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        destinationTexture6 = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        // Add the onPostRender callback
        Camera.onPostRender += OnPostRenderCallback;
    }

    void OnPostRenderCallback(Camera cam)
    {
        if (isPerformingScreenGrab)
        {
            // Check whether the Camera that has just finished rendering is the one you want to take a screen grab from
            // if (cam == Camera.main)
            // {

            // Define the parameters for the ReadPixels operation
            Rect regionToReadFrom = new Rect(0, 0, Screen.width, Screen.height);
            int xPosToWriteTo = 0;
            int yPosToWriteTo = 0;
            bool updateMipMapsAutomatically = false;

            // Copy the pixels from the Camera's render target to the texture
            if (photo1)
            {
                destinationTexture1.ReadPixels(regionToReadFrom, xPosToWriteTo, yPosToWriteTo, updateMipMapsAutomatically);
                destinationTexture1.Apply();

                Sprite spritePhoto1 = Sprite.Create(destinationTexture1, new Rect(0, 0, destinationTexture1.width, destinationTexture1.height), Vector2.zero);
                photoDisplay1.sprite = spritePhoto1;

                photoDisplay1.color = Color.white;

                //screenGrabRenderer1.material.color = Color.white;

                if (rating == 0)
                    rating1.sprite = noStar;
                if (rating == 3)
                    rating1.sprite = threeStar;

                photo1 = false;
                photo2 = true;
            }
            else if (photo2)
            {
                destinationTexture2.ReadPixels(regionToReadFrom, xPosToWriteTo, yPosToWriteTo, updateMipMapsAutomatically);
                destinationTexture2.Apply();

                Sprite spritePhoto2 = Sprite.Create(destinationTexture2, new Rect(0, 0, destinationTexture2.width, destinationTexture2.height), Vector2.zero);
                photoDisplay2.sprite = spritePhoto2;

                photoDisplay2.color = Color.white;

                if (rating == 0)
                    rating2.sprite = noStar;
                if (rating == 3)
                    rating2.sprite = threeStar;

                photo2 = false;
                photo3 = true;
            }
            else if (photo3)
            {
                destinationTexture3.ReadPixels(regionToReadFrom, xPosToWriteTo, yPosToWriteTo, updateMipMapsAutomatically);
                destinationTexture3.Apply();

                Sprite spritePhoto3 = Sprite.Create(destinationTexture3, new Rect(0, 0, destinationTexture3.width, destinationTexture3.height), Vector2.zero);
                photoDisplay3.sprite = spritePhoto3;

                photoDisplay3.color = Color.white;

                if (rating == 0)
                    rating3.sprite = noStar;
                if (rating == 3)
                    rating3.sprite = threeStar;

                photo3 = false;
                photo4 = true;
            }
            else if (photo4)
            {
                destinationTexture4.ReadPixels(regionToReadFrom, xPosToWriteTo, yPosToWriteTo, updateMipMapsAutomatically);
                destinationTexture4.Apply();

                Sprite spritePhoto4 = Sprite.Create(destinationTexture4, new Rect(0, 0, destinationTexture4.width, destinationTexture4.height), Vector2.zero);
                photoDisplay4.sprite = spritePhoto4;

                photoDisplay4.color = Color.white;

                if (rating == 0)
                    rating4.sprite = noStar;
                if (rating == 3)
                    rating4.sprite = threeStar;

                photo4 = false;
                photo5 = true;
            }
            else if (photo5)
            {
                destinationTexture5.ReadPixels(regionToReadFrom, xPosToWriteTo, yPosToWriteTo, updateMipMapsAutomatically);
                destinationTexture5.Apply();

                Sprite spritePhoto5 = Sprite.Create(destinationTexture5, new Rect(0, 0, destinationTexture5.width, destinationTexture5.height), Vector2.zero);
                photoDisplay5.sprite = spritePhoto5;

                photoDisplay5.color = Color.white;

                if (rating == 0)
                    rating5.sprite = noStar;
                if (rating == 3)
                    rating5.sprite = threeStar;

                photo5 = false;
                photo6 = true;
            }
            else if (photo6)
            {
                destinationTexture6.ReadPixels(regionToReadFrom, xPosToWriteTo, yPosToWriteTo, updateMipMapsAutomatically);
                destinationTexture6.Apply();

                Sprite spritePhoto6 = Sprite.Create(destinationTexture6, new Rect(0, 0, destinationTexture6.width, destinationTexture6.height), Vector2.zero);
                photoDisplay6.sprite = spritePhoto6;

                photoDisplay6.color = Color.white;

                if (rating == 0)
                    rating6.sprite = noStar;
                if (rating == 3)
                    rating6.sprite = threeStar;

                photo6 = false;
                outOfPics = true;
            }

            // Reset the isPerformingScreenGrab state
            isPerformingScreenGrab = false;
            //  }
        }
    }

    // Remove the onPostRender callback
    void OnDestroy()
    {
        Camera.onPostRender -= OnPostRenderCallback;
    }

    void OnEnable()
    {
        PlayerSensor.WhenMonsterSpotted += MonsterInView;
        PlayerSensor.WhenMonsterNotSpotted += MonsterNotInView;
    }

    void OnDisable()
    {
        PlayerSensor.WhenMonsterSpotted -= MonsterInView;
        PlayerSensor.WhenMonsterNotSpotted -= MonsterNotInView;
    }

    void OnCamera(InputValue value)
    {
        print("cameraon");
        if (!cameraOn)
        {
            cameraOn = true;
            viewFinder.enabled = true;
            StartCoroutine(BatteryDrain());
        }
        else
        {
            viewFinder.enabled = false;
            StopAllCoroutines();
            cameraOn = false;
        }
        if (WhenCameraOn != null)
            WhenCameraOn(cameraOn);
    }

    IEnumerator BatteryDrain()
    {
        while (cameraOn)
        {
            if (cameraFlashobj.intensity == 2f)
            {
                timeElapsedFlash -= Time.deltaTime;
                if (timeElapsedFlash < 0)
                {
                    cameraFlashobj.intensity = 0;
                }
            }

            timeElapsed -= Time.deltaTime;
            if (timeElapsed < 0)
            {
                timeElapsed = 1;
                batteryLife--;
            }

            //print("battery = " + batteryLife);

            if (batteryLife < 0)
            {
                viewFinder.enabled = false;
                cameraOn = false;
                if (WhenCameraOn != null)
                    WhenCameraOn(cameraOn);
            }
            yield return null;
        }

    }


    void MonsterInView()
    {
        monsterVisible = true;
    }

    void MonsterNotInView()
    {
        monsterVisible = false;
    }

    void OnLeftClick(InputValue value)
    {
        if (clickOnce)
        {
            if (cameraOn && !outOfPics)
            {
                cameraFlashobj.intensity = 2f;
                timeElapsedFlash = .5f;

                if (monsterVisible)
                    rating = 3;
                else
                {
                    rating = 0;
                }

                isPerformingScreenGrab = true;
            }
            clickOnce = false;
        }
        else
        {
            clickOnce = true;
        }

    }

}
