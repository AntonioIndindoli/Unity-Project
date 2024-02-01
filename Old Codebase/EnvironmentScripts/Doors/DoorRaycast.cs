using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DoorRaycast : MonoBehaviour
{
    public delegate void DoorEvent(int eventNumber, Vector3 pos);
    public static event DoorEvent WhenDoorEvent;

    [SerializeField] private int rayLength = 2;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private string excludeLayerName = null;

    private DoorTrigger inInFrontTrigger;
    private DoorOpen DoorScript;
    private Transform trigger;
    private Transform doorObj = null;
    //private Transform doorObjInactive = null;
    private Transform knobs;
    private float doorRotation = 0;

    [SerializeField] private Image crosshair = null;
    private bool isCrosshairActive;
    private bool doOnce;
    private float speed = 350f;
    private bool side1;
    private bool side2;
    public float handleRot;
    private bool isInFront;
    private bool clamped;
    private int frame;
    private bool resetHandleOnce = false;

    private bool isInteracted = false;
    private bool animPlaying;
    private bool doorLocked;

    private const string interactableTag1 = "InteractiveObject1";
    private const string interactableTag2 = "InteractiveObject2";

    void OnLeftClick(InputValue value)
    {
        if (!isInteracted)
        {
            isInteracted = true;
            if (WhenDoorEvent != null && doorObj != null)
            {
                Vector3 pos = doorObj.transform.position;
                if (WhenDoorEvent != null)
                    WhenDoorEvent(1, pos);
            }
            if (WhenDoorEvent != null && doorObj != null && DoorScript.doorLocked == true)
            {
                Vector3 pos = doorObj.transform.position;
                if (WhenDoorEvent != null)
                    WhenDoorEvent(6, pos);
            }
        }
        else
        {
            isInteracted = false;
            doorObj = null;
            side1 = false;
            side2 = false;
            doOnce = false;
        }
    }

    void OnLockDoor(InputValue value)
    {
        if (!DoorScript.doorLocked && doorObj != null && doorObj.transform.eulerAngles.y == 0)
        {
            DoorScript.doorLocked = true;
            Vector3 pos = doorObj.transform.position;
            if (WhenDoorEvent != null)
                WhenDoorEvent(4, pos);
        }
        else if (doorObj != null && doorObj.transform.eulerAngles.y == 0)
        {
            DoorScript.doorLocked = false;
            Vector3 pos = doorObj.transform.position;
            if (WhenDoorEvent != null)
                WhenDoorEvent(5, pos);
        }
    }

    private void Update()
    {
        frame++;
        //raycast to grab door object
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;
        if (Physics.Raycast(transform.position, fwd, out hit, rayLength, mask))
        {
            if (hit.collider.CompareTag(interactableTag1))
            {
                if (!doOnce)
                {
                    doorObj = hit.collider.transform.parent;
                    DoorScript = doorObj.GetComponent<DoorOpen>();
                    CrosshairChange(true);
                    side1 = true;
                    side2 = false;
                }

                isCrosshairActive = true;
                doOnce = true;

            }
            else if (hit.collider.CompareTag(interactableTag2))
            {
                if (!doOnce)
                {
                    doorObj = hit.collider.transform.parent;
                    DoorScript = doorObj.GetComponent<DoorOpen>();
                    CrosshairChange(true);
                    side2 = true;
                    side1 = false;
                }

                isCrosshairActive = true;
                doOnce = true;

            }
        }
        else
        {
            if (isCrosshairActive)
            {
                CrosshairChange(false);
                doOnce = false;
            }
            if (!isInteracted)
            {
                doorObj = null;
            }
        }

        //if the player is interacting & raycast is colliding with a side of the door then rotate door w/ mouse

        if (isInteracted && side1 && !DoorScript.doorLocked)
        {
            resetHandleOnce = false;
            isInFront = false;
            trigger = doorObj.Find("Trigger");
            inInFrontTrigger = trigger.GetComponent<DoorTrigger>();
            isInFront = inInFrontTrigger.isInFront;
            print(isInFront);

            StopAllCoroutines();

            knobs = doorObj.Find("knobs");
            if (knobs.transform.rotation.eulerAngles.z < 70)
                knobs.transform.Rotate(0, 0, 200 * Time.deltaTime);
            handleRot = knobs.transform.rotation.eulerAngles.z;

            if (Input.GetAxis("Mouse X") < 0)
            {
                doorRotation = Input.GetAxis("Mouse X") * Time.deltaTime * speed;
                doorRotation = Mathf.Clamp(doorRotation, -10, 0);

                if (doorObj.transform.eulerAngles.y < 11 || doorObj.transform.eulerAngles.y > 79)
                {
                    doorRotation = Mathf.Clamp(doorRotation, -2, 0);
                }

                if (doorObj.transform.eulerAngles.y == 0)
                {
                    doorRotation = 0;
                    print("blocked");
                }

                if (!isInFront)
                    doorRotation = -doorRotation;

                Vector3 t = trigger.transform.eulerAngles;

                doorObj.transform.Rotate(0, doorRotation, 0, Space.World);
                trigger.transform.Rotate(0, -doorRotation, 0, Space.World);

                print("1DoorRot = " + doorRotation);

                //playsqueak
                Vector3 pos = doorObj.transform.position;
                if (WhenDoorEvent != null && (frame % 80) == 0)
                {
                    WhenDoorEvent(2, pos);
                    frame = 1;
                }

                Vector3 p = doorObj.transform.eulerAngles;
                p.y = Mathf.Clamp(doorObj.transform.eulerAngles.y, 2, 90);
                if (p.y == 2)
                {
                    p.y = 0;
                    if (WhenDoorEvent != null)
                        WhenDoorEvent(3, pos);
                }
                doorObj.transform.eulerAngles = p;
                trigger.transform.eulerAngles = t;

            }
            else if (Input.GetAxis("Mouse X") > 0)
            {
                doorRotation = Input.GetAxis("Mouse X") * Time.deltaTime * speed;
                doorRotation = Mathf.Clamp(doorRotation, 0, 10);

                if (doorObj.transform.eulerAngles.y < 11 || doorObj.transform.eulerAngles.y > 79)
                {
                    doorRotation = Mathf.Clamp(doorRotation, 0, 2);
                }

                if (!isInFront)
                    doorRotation = -doorRotation;

                if (doorObj.transform.eulerAngles.y == 0 && !isInFront)
                {
                    doorRotation = 0;
                    print("blocked");
                }

                print("2DoorRot = " + doorRotation);

                Vector3 t = trigger.transform.eulerAngles;

                doorObj.transform.Rotate(0, doorRotation, 0, Space.World);
                trigger.transform.Rotate(0, -doorRotation, 0, Space.World);

                //playsqueak
                Vector3 pos = doorObj.transform.position;
                if (WhenDoorEvent != null && (frame % 80) == 0)
                {
                    WhenDoorEvent(2, pos);
                    frame = 1;
                }


                Vector3 p = doorObj.transform.eulerAngles;
                p.y = Mathf.Clamp(doorObj.transform.eulerAngles.y, 2, 90);
                if (p.y == 2)
                {
                    p.y = 0;
                    if (WhenDoorEvent != null)
                        WhenDoorEvent(3, pos);
                }
                doorObj.transform.eulerAngles = p;
                trigger.transform.eulerAngles = t;
            }

        }
        else if (isInteracted && side2 && !DoorScript.doorLocked)
        {
            resetHandleOnce = false;
            isInFront = false;
            trigger = doorObj.Find("Trigger");
            inInFrontTrigger = trigger.GetComponent<DoorTrigger>();
            isInFront = inInFrontTrigger.isInFront;
            //print(isInFront);

            StopAllCoroutines();

            knobs = doorObj.Find("knobs");
            if (knobs.transform.rotation.eulerAngles.z < 70)
                knobs.transform.Rotate(0, 0, 200 * Time.deltaTime);
            handleRot = knobs.transform.rotation.eulerAngles.z;

            if (Input.GetAxis("Mouse X") < 0)
            {
                doorRotation = Input.GetAxis("Mouse X") * Time.deltaTime * speed;
                doorRotation = Mathf.Clamp(doorRotation, -10, 0);

                if (doorObj.transform.eulerAngles.y < 11 || doorObj.transform.eulerAngles.y > 79)
                {
                    doorRotation = Mathf.Clamp(doorRotation, -2, 0);
                }

                Vector3 t = trigger.transform.eulerAngles;

                doorObj.transform.Rotate(0, -doorRotation, 0, Space.World);
                trigger.transform.Rotate(0, doorRotation, 0, Space.World);

                print("3DoorRot = -" + doorRotation);

                //playsqueak
                Vector3 pos = doorObj.transform.position;
                if (WhenDoorEvent != null && (frame % 80) == 0)
                {
                    WhenDoorEvent(2, pos);
                    frame = 1;
                }


                Vector3 p = doorObj.transform.eulerAngles;
                p.y = Mathf.Clamp(doorObj.transform.eulerAngles.y, 2, 90);
                if (p.y == 2)
                {
                    p.y = 0;
                    if (WhenDoorEvent != null)
                        WhenDoorEvent(3, pos);
                }
                doorObj.transform.eulerAngles = p;
                trigger.transform.eulerAngles = t;
            }
            else if (Input.GetAxis("Mouse X") > 0)
            {
                doorRotation = Input.GetAxis("Mouse X") * Time.deltaTime * speed;
                doorRotation = Mathf.Clamp(doorRotation, 0, 10);

                if (doorObj.transform.eulerAngles.y < 11 || doorObj.transform.eulerAngles.y > 79)
                {
                    doorRotation = Mathf.Clamp(doorRotation, 0, 2);
                }

                Vector3 t = trigger.transform.eulerAngles;

                if (!isInFront)
                {
                    doorRotation = -doorRotation;
                }

                if (doorObj.transform.eulerAngles.y == 0 && !isInFront)
                {
                    doorRotation = 0;
                }

                doorObj.transform.Rotate(0, doorRotation, 0, Space.World);
                trigger.transform.Rotate(0, -doorRotation, 0, Space.World);

                print("4DoorRot = " + doorRotation);

                //playsqueak
                Vector3 pos = doorObj.transform.position;
                if (WhenDoorEvent != null && (frame % 80) == 0)
                {
                    WhenDoorEvent(2, pos);
                    frame = 1;
                }


                Vector3 p = doorObj.transform.eulerAngles;
                p.y = Mathf.Clamp(doorObj.transform.eulerAngles.y, 2, 90);
                if (p.y == 2)
                {
                    p.y = 0;
                    if (WhenDoorEvent != null)
                        WhenDoorEvent(3, pos);
                }
                doorObj.transform.eulerAngles = p;
                trigger.transform.eulerAngles = t;
            }
        }
        else if (knobs != null)
        {
            //if not interacting with door then reset handle position
            if (!resetHandleOnce)
            {
                resetHandleOnce = true;
                StopAllCoroutines();
                StartCoroutine(ResetHandle());
            }
            if (knobs.transform.eulerAngles.z == 0)
            {
                resetHandleOnce = false;
                knobs = null;
            }
        }
    }

    IEnumerator ResetHandle()
    {
        float lerpDuration = .2f;
        float timeElapsed = 0;
        Quaternion startRotation = knobs.transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, 0, -70);

        while (timeElapsed < lerpDuration && knobs.transform.rotation.eulerAngles.z > 0 && knobs.transform.rotation.eulerAngles.z < 200)
        {
            knobs.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        Vector3 p = knobs.transform.eulerAngles;
            p.z = 0;

        knobs.transform.eulerAngles = p;
    }

    void CrosshairChange(bool on)
    {
        if (on && !doOnce)
        {
            crosshair.color = Color.red;
        }
        else
        {
            crosshair.color = Color.white;
            isCrosshairActive = false;
        }
    }




}
