using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class rotatetest : MonoBehaviour
{
    private GameObject itemToRotate = null;

    private void Start()
    {
        itemToRotate = GameObject.Find("testdoor");
    }


    void OnInteract(InputValue value)
    {
        print("b ket press");
        StartCoroutine(RotateObject());
    }

    IEnumerator RotateObject()
    {
        float moveSpeed = 50f;
        Quaternion endingAngle = Quaternion.Euler(new Vector3(45, 45, 0));

        while (Vector3.Distance(itemToRotate.transform.rotation.eulerAngles, endingAngle.eulerAngles) > 0.01f)
        {
            itemToRotate.transform.rotation = Quaternion.RotateTowards(itemToRotate.transform.rotation, endingAngle, moveSpeed * Time.deltaTime);
            yield return null;
        }

        itemToRotate.transform.rotation = endingAngle;
    }

}
