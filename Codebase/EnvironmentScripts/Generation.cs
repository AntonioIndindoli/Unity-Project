using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Generation : MonoBehaviour
{
    public GameObject roomSegment1;
    public GameObject roomSegment2;
    public GameObject roomSegment3;
    public GameObject roomSegment4;

    public GameObject monster;

    private GameObject objToSpawn;
    private GameObject[] spawnPoints;
    private bool doOnce = false;

    private NavMeshSurface surface;

    //NavMeshSurface[] surface = new NavMeshSurface[25];

    private GameObject genNavMeshObj;
    private bool doOnce2 = false;
    private int ranVal;
    private int ranValOld;


    [SerializeField]
    public int mapSize = 5;
    private float spawnTimer = 2;


    // Start is called before the first frame update
    void Start()
    {
        //generate array of map segments at runtime
        for (int h = 0; h < mapSize; h++)
        {

            for (int i = 0; i < mapSize; i++)
            {
                ranVal = Random.Range(1, 4);
                if (ranVal == ranValOld)
                {
                    ranVal = Random.Range(1, 4);
                }
                ranValOld = ranVal;
                switch (ranVal)
                {
                    case 1:
                        objToSpawn = roomSegment1;
                        break;
                    case 2:
                        objToSpawn = roomSegment2;
                        break;
                    case 3:
                        objToSpawn = roomSegment3;
                        break;
                    case 4:
                        objToSpawn = roomSegment4;
                        break;
                    default:
                        print("Default case");
                        break;
                }

                if( i == 2 && h == 2)
                objToSpawn = roomSegment2;

                genNavMeshObj = Instantiate(objToSpawn, new Vector3(50 * i, 0, 50 * h), Quaternion.identity);

                surface = genNavMeshObj.GetComponent<NavMeshSurface>();
                //surface.BuildNavMesh();
            }
        }

        surface.BuildNavMesh();

    }

    // Update is called once per frame
    void Update()
    {
        //spawn the monster randomly on the map after spawnTimer amount of seconds
        if (spawnTimer < 0 && !doOnce)
        {
            GameObject monsterSpawnPoint = GenerateSpawnPos();
            Instantiate(monster, monsterSpawnPoint.transform.position, Quaternion.identity);
            print("MonsterSpawned");
            doOnce = true;
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }

    GameObject GenerateSpawnPos()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        int randomNum = Random.Range(0, spawnPoints.Length);
        GameObject finalSpawnPos = spawnPoints[randomNum];

        float distance = Vector3.Distance(this.transform.position, finalSpawnPos.transform.position);

        //if the spawn chosen is too close, choose again
        if (distance < 100)
        {
            //print("regeneration");
            finalSpawnPos = GenerateSpawnPos();
        }

        //print("distance = " + distance);
        return finalSpawnPos;
    }
}
