using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickableObject : MonoBehaviour
{
    // Reference to the rigidbody
    private Rigidbody rb;
    public Rigidbody Rb => rb;

    private GameObject thisObject;
    public AudioClip col_Clip;

    private void Start()
    {
        // Get reference to the rigidbody
        rb = GetComponent<Rigidbody>();
        thisObject = this.gameObject;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (col_Clip != null && thisObject != null && thisObject.transform.position != null)
        AudioSource.PlayClipAtPoint(col_Clip, thisObject.transform.position, .05f);

    }
}