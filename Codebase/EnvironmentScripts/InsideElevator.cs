using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideElevator : MonoBehaviour
{
    public bool isInElevator = false;

    private void OnTriggerEnter(Collider dataFromCollision)
    {
        if (dataFromCollision.gameObject.name == "PlayerCapsule")
        {
            isInElevator = true;
        }

    }
    private void OnTriggerExit(Collider dataFromCollision)
    {
        if (dataFromCollision.gameObject.name == "PlayerCapsule")
        {
            isInElevator = false;
        }

    }
}
