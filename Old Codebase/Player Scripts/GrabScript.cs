using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GrabScript : MonoBehaviour
{
    // Reference to the character camera.
    [SerializeField]
    private Camera characterCamera;
    // Reference to the slot for holding picked item.

    // Reference to the currently held item.
    private PickableObject pickedItem;

    public LayerMask IgnoreMe;
    public Transform guide;
    public float moveForce = 250;
    private interactableInterface interactable3DUI;
    private interactableButton interactableButtonObj;
    private InventoryItem InventoryItemObj;

    void Update()
    {
        if (pickedItem)
        {

            MoveObject();
        }
    }

    void MoveObject()
    {
        if (Vector3.Distance(pickedItem.transform.position, guide.position) > 0.1f)
        {
            Vector3 moveDirection = (guide.position - pickedItem.transform.position);
            pickedItem.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }
    }

    void OnInteract(InputValue value)
    {
        // Check if player picked some item already
        if (pickedItem)
        {
            // If yes, drop picked item
            DropItem(pickedItem);
        }
        else
        {
            // If no, try to pick item in front of the player
            // Create ray from center of the screen
            var ray = characterCamera.ViewportPointToRay(Vector3.one * 0.5f);
            RaycastHit hit;
            // Shot ray to find object to pick
            if (Physics.Raycast(ray, out hit, 1.5f, ~IgnoreMe))
            {

                // Check if object is pickable or interactable

                interactable3DUI = hit.transform.GetComponent<interactableInterface>();
                interactableButtonObj = hit.transform.GetComponent<interactableButton>();
                InventoryItemObj = hit.transform.GetComponent<InventoryItem>();

                var pickable = hit.transform.GetComponent<PickableObject>();
                // If object has PickableObject class
                if (pickable && Vector3.Distance(pickable.transform.position, this.transform.position) < 3)
                {
                    // Pick it
                    PickItem(pickable);
                }
                else
                {
                    if (interactable3DUI)
                    {
                        var launchScript = interactable3DUI.GetComponentInChildren<UICursor3D>();
                        launchScript.isBeingInteracted = true;
                    }
                }

                if (interactableButtonObj)
                {
                    interactableButtonObj.isInteracted();
                }

                if (InventoryItemObj)
                {
                    InventoryItemObj.isInteracted();
                }
            }
        }
    }

    void OnTab(InputValue value)
    {
        if (interactable3DUI)
        {
            var launchScript = interactable3DUI.GetComponentInChildren<UICursor3D>();
            launchScript.isBeingInteracted = false;
        }
    }

    /// <param name="item">Item.</param>
    private void PickItem(PickableObject item)
    {
        //print("picked!");
        Rigidbody objRig = item.GetComponent<Rigidbody>();
        objRig.useGravity = false;
        objRig.drag = 10;
        objRig.transform.parent = guide;
        pickedItem = item;
    }

    /// <param name="item">Item.</param>
    private void DropItem(PickableObject item)
    {
        //print("dropped!");
        Rigidbody pickedRig = pickedItem.GetComponent<Rigidbody>();
        pickedRig.useGravity = true;
        pickedRig.drag = 1;

        pickedItem = null;
        item.transform.parent = null;

    }
}