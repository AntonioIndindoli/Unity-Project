using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDrawer : MonoBehaviour
{
    private Collider[] hitColliders;
    private Identifier roomIdentifier;

    private Transform mapObj;
    private Transform roomObj;


    // Update is called once per frame
    void Update()
    {

        hitColliders = Physics.OverlapSphere(this.transform.position, 1, 1 << 7);

        for (int i = 0; i < hitColliders.Length; i++)
        {

            if (hitColliders[i].CompareTag("RoomMesh"))
            {

                roomObj = hitColliders[i].transform.parent;

                mapObj = roomObj.Find("Map");

                mapObj.gameObject.SetActive(true);
            }


        }



    }
}