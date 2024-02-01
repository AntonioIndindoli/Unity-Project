using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public InsideElevator insideElevatorTrigger;
    public Animator door1;
    public Animator door2;
    public bool elevatorCalling = false;
    public bool elevatorClosing = false;
    public bool open = false;
    public MeshRenderer button1_Renderer;
    public MeshRenderer button2_Renderer;
    public MeshRenderer indicator_Renderer;
    public Material on_material;
    public Material indon_material;
    public Material outof_material;
    public Material order_material;
    public Material off_material;
    public Collider doorCollider;
    public Light indicatorLight1;
    public Light indicatorLight2;
    public InventoryManager InventoryManagerScript;
    public bool elevatorUnlocked = false;
    public bool changeWord = false;
    public float indicatorTimer;

    private GameObject playerObj = null;

    void Awake()
    {
        if (playerObj == null)
            playerObj = GameObject.Find("PlayerCapsule");

        InventoryManagerScript = playerObj.GetComponent<InventoryManager>();
    }

    void Update()
    {
        if (!elevatorUnlocked)
        {
            indicatorTimer -= Time.deltaTime;

            if (indicatorTimer < 0)
            {
                indicatorTimer = 2;

                if (changeWord)
                {
                    indicatorLight1.intensity = .7f;
                    indicatorLight2.intensity = .7f;
                    indicator_Renderer.material = outof_material;
                    changeWord = false;
                }
                else
                {
                    indicatorLight1.intensity = .6f;
                    indicatorLight2.intensity = .6f;
                    changeWord = true;
                    indicator_Renderer.material = order_material;
                }
            }
        }
    }

    public void OpenDoors()
    {
        if (InventoryManagerScript.fireKey)
        {
            elevatorUnlocked = true;

            indicatorLight1.intensity = .5f;
            indicatorLight2.intensity = .5f;
            button2_Renderer.material = on_material;
            indicator_Renderer.material = indon_material;
            //button2_Renderer.material = on_material;
            StartCoroutine(CallElevator());
        }
    }

    public void CloseDoors()
    {
        if (insideElevatorTrigger.isInElevator)
        {
            doorCollider.enabled = true;
            door1.Play("Close", 0, 0.0f);
            door2.Play("Close", 0, 0.0f);

            StartCoroutine(CloseElevator());
        }
    }

    IEnumerator CallElevator()
    {
        float timeElapsed = 0;
        elevatorCalling = true;

        while (timeElapsed < 15)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        doorCollider.enabled = false;
        open = true;
        elevatorCalling = false;
        door1.Play("Open", 0, 0.0f);
        door2.Play("Open", 0, 0.0f);
        indicatorLight1.intensity = 0f;
        indicatorLight2.intensity = 0f;
        button2_Renderer.material = off_material;
        indicator_Renderer.material = off_material;
    }

    IEnumerator CloseElevator()
    {
        float timeElapsed = 0;
        elevatorClosing = true;

        while (timeElapsed < 2)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        elevatorClosing = false;
        open = false;
    }

}
