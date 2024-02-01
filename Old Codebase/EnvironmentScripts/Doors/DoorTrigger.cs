using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public bool isInFront = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //print(isInFront);
    }

    private void OnTriggerEnter(Collider dataFromCollision)
    {
        if (dataFromCollision.gameObject.name == "PlayerCapsule")
        {
            isInFront = true;
            //print("entered trigger");
        }

    }
    private void OnTriggerExit(Collider dataFromCollision)
    {
        if (dataFromCollision.gameObject.name == "PlayerCapsule")
        {
            isInFront = false;
            //print("exit trigger");
        }

    }
}
