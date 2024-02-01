using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFigureController : MonoBehaviour
{
    private ParticleSystem figureParticleSystem;
    float distance;
    private GameObject playerObj = null;

    void Awake()
    {
        figureParticleSystem = GetComponent<ParticleSystem>();
        if (playerObj == null)
            playerObj = GameObject.Find("PlayerCapsule");
    }

    void OnEnable()
    {
        SanityScript.WhenGenerateFigures += GenerateFigure;
    }

    void OnDisable()
    {
        SanityScript.WhenGenerateFigures -= GenerateFigure;
    }

    void GenerateFigure()
    {
        distance = Vector3.Distance(this.transform.position, playerObj.transform.position);
        if (distance < 50 && distance > 15)
        figureParticleSystem.Play();
    }
}
