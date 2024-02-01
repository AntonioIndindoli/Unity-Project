using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTrigger : MonoBehaviour
{
    public delegate void PlayerHiding(bool isHiding);
    public static event PlayerHiding WhenHiding;

    private void OnTriggerEnter(Collider dataFromCollision)
    {
        if (dataFromCollision.gameObject.name == "PlayerCapsule")
        {
            if (WhenHiding != null)
                WhenHiding(true);
            print("entered");
        }

    }
    private void OnTriggerExit(Collider dataFromCollision)
    {
        if (dataFromCollision.gameObject.name == "PlayerCapsule")
        {
            if (WhenHiding != null)
                WhenHiding(false);
            print("exit");
        }

    }
}
