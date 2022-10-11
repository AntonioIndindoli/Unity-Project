using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Itemspawning : MonoBehaviour
{
    public GameObject battery;
    public GameObject fireKey;
    public GameObject playerObj;
    private Transform[] arrayOfSpawns;
    private int ran1;
    private int batterySpawnRate;
    private InventoryManager playerInventory;
    private bool doOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        arrayOfSpawns = this.gameObject.transform.Cast<Transform>().Where(c => c.gameObject.tag == "itemSpawn").ToArray();

        if (playerObj == null)
            playerObj = GameObject.Find("PlayerCapsule");

        playerInventory = playerObj.GetComponent<InventoryManager>();
    }

    private void OnTriggerEnter(Collider dataFromCollision)
    {
        if (dataFromCollision.gameObject.name == "PlayerCapsule" && !doOnce)
        {
            doOnce = true;

            for (int i = 0; i < arrayOfSpawns.Length; i++)
            {
                ran1 = Random.Range(1, 10);
                if(ran1 > playerInventory.batteryCount)
                    Instantiate(battery, arrayOfSpawns[i].transform.position, Quaternion.identity);
                else if (!playerInventory.fireKey && ran1 == 5)
                {
                    Instantiate(fireKey, arrayOfSpawns[i].transform.position, Quaternion.identity);
                }
            }
        }

    }
}
