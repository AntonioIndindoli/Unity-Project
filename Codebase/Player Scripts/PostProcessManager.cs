using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class PostProcessManager : MonoBehaviour
{
    public PostProcessVolume volume;
    Bloom bloom;
    ChromaticAberration chromaticAberration;
    LensDistortion lensDistortion;
    MotionBlur motionBlur;
    ColorGrading colorGrading;
    Grain grain;

    private bool playerDead;

    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out bloom);
        volume.profile.TryGetSettings(out chromaticAberration);
        volume.profile.TryGetSettings(out lensDistortion);
        volume.profile.TryGetSettings(out motionBlur);
        volume.profile.TryGetSettings(out colorGrading);
        volume.profile.TryGetSettings(out grain);
    }

    void OnEnable()
    {
        SanityScript.WhenSanityLowered += OnLowered;
        SanityScript.WhenSanityRaised += OnRaised;
        PlayerStatsController.WhenPlayerDamaged += OnDamage;
        PlayerStatsController.WhenPlayerDead += OnDeath;
        CameraController.WhenCameraOn += OnCameraOn;
    }

    void OnDisable()
    {
        SanityScript.WhenSanityLowered -= OnLowered;
        SanityScript.WhenSanityRaised -= OnRaised;
        PlayerStatsController.WhenPlayerDamaged -= OnDamage;
        PlayerStatsController.WhenPlayerDead -= OnDeath;
        CameraController.WhenCameraOn -= OnCameraOn;
    }

    void OnCameraOn(bool cameraOn)
    {
        print("cameraon!!!!");
        if (!playerDead && cameraOn)
        {
            colorGrading.colorFilter.value = new Color32(100, 100, 20, 0);
            grain.active = true;
        }
        else
        {
            colorGrading.colorFilter.value = new Color32(255, 255, 255, 0);
            grain.active = false;
        }
    }

    void OnDamage()
    {
        if (!playerDead)
        {
            colorGrading.colorFilter.value = new Color32(130, 40, 0, 0);
            StopAllCoroutines();
            StartCoroutine(ResetColor());
        }
    }

    void OnDeath()
    {
        if (!playerDead)
        {
            StopAllCoroutines();
            StartCoroutine(FadeToBlack());
        }
        playerDead = true;
    }

    IEnumerator ResetColor()
    {
        float timeElapsed = 0;
        byte red = 130;
        byte green = 40;
        byte blue = 0;

        while (timeElapsed < 5)
        {
            if(red < 255)
                red++;

            if (green < 255)
                green++;

            if (blue < 255)
                blue++;

            colorGrading.colorFilter.value = new Color32(red, green, blue, 0);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FadeToBlack()
    {
        byte red = 130;
        byte green = 40;
        byte blue = 10;

        while (red > 1 || blue > 1 || red > 1)
        {
            if (red > 1)
                red -= 2;

            if (green > 1)
                green -= 1;

            if (blue > 1)
                blue -= 1;

            colorGrading.colorFilter.value = new Color32(red, green, blue, 0);
            yield return null;
        }

        colorGrading.colorFilter.value = new Color32(0, 0, 0, 0);

        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }

    void OnLowered(int sanity)
    {
        float sanityDecimal = sanity;
        bloom.active = true;
        bloom.intensity.value = sanityDecimal - 4;
        chromaticAberration.active = true;
        chromaticAberration.intensity.value = sanityDecimal / 20;
        lensDistortion.active = true;
        if (sanity < 15)
            lensDistortion.intensity.value = sanityDecimal*4;
        lensDistortion.scale.value = 1.09f;

        motionBlur.active = true;
    }

    void OnRaised(int sanity)
    {
        float sanityDecimal = sanity;
        bloom.active = true;
        bloom.intensity.value = sanityDecimal - 4;
        chromaticAberration.active = true;
        chromaticAberration.intensity.value = sanityDecimal / 20;
        lensDistortion.active = true;
        lensDistortion.intensity.value = sanityDecimal * 4;
        lensDistortion.scale.value = 1.09f;
        motionBlur.active = false;

    }


}