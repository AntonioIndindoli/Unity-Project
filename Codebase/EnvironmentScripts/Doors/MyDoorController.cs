using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDoorController : MonoBehaviour
{
    private Animator doorAnim;
    public bool doorOpen = false;
    private bool animPlaying;

    private void Awake()
    {
        doorAnim = gameObject.GetComponent<Animator>();
    }

    public void PlayAnimation(bool animPlaying)
    {
        if (!doorOpen && !animPlaying)
        {
            doorAnim.Play("DoorOpen", 0, 0.0f);
            doorOpen = true;
            //animPlaying = true;
            //StartCoroutine(Delay1Sec());
        }
        else if (!animPlaying)
        {
            doorAnim.Play("DoorClose", 0, 0.0f);
            doorOpen = false;
            //animPlaying = true;
            //StartCoroutine(Delay1Sec());
        }
    }
    /*
    IEnumerator Delay1Sec()
    {
        yield return new WaitForSeconds(1);
        animPlaying = false;
    }
    */
}
