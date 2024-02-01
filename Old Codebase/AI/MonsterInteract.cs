using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInteract : MonoBehaviour
{
    private const string interactableTag = "InteractiveObject";
    private bool animPlaying;
    private MyDoorController raycastedObj;

    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 1);

        //Transform nearest = null;
        //float nearDist = float.PositiveInfinity;
        for (int i = 0; i < hitColliders.Length; i++)
        {
            //detect doors and open them
            if (hitColliders[i].CompareTag(interactableTag) && hitColliders != null)
            {
                raycastedObj = hitColliders[i].gameObject.GetComponent<MyDoorController>();
                OpenDoor(raycastedObj);
            }

        }
      
    }

    void OpenDoor(MyDoorController raycastedObj)
    {
        if (!animPlaying && !raycastedObj.doorOpen)
        {
            print("Doornear");
            raycastedObj.PlayAnimation(animPlaying);
            animPlaying = true;
            //print("animPlaying = true");
            StartCoroutine(Delay1Sec());
        }
    }

    IEnumerator Delay1Sec()
    {
        yield return new WaitForSeconds(1);
        animPlaying = false;
        //print("animPlaying = false");
    }

}
