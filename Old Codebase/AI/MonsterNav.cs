using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MonsterNav : MonoBehaviour
{
    public delegate void playerBeingHunted(bool beingHunted);
    public static event playerBeingHunted WhenHunted;

    public delegate void monsterClose();
    public static event monsterClose WhenMonsterClose;

    public delegate void monsterNotClose();
    public static event monsterNotClose WhenMonsterNotClose;

    //private Transform movePositionTransform;

    private GameObject playerObj = null;
    private NavMeshAgent navMeshAgent;
    public int alertLevel = 0;
    private bool isMoving = false;
    private bool isSprinting = false;
    private bool isSpotted = false;
    private bool playerClose = false;
    private const string Player = "Player";
    public GameObject monCollider;
    private bool roaming = false;
    private bool idle;
    private GameObject[] navPoints;
    float navTimer1 = 0;
    float navTimer2 = 0;
    //bool alertLevelDecayDelay = false;
    int delayTimer = 0;
    int playerCloseDelay = 0;
    bool hiding = false;
    bool dontReset;
    private Vector3 monsterPos;
    private Vector3 playerPos;

    public Vector3 doorPos;
    private bool beingHunted;
    public bool isAttackingDoor = false;
    //private NavMeshPath monsterPath;

    void OnEnable()
    {
        EventManager.WhenMoved += OnMove;
        EventManager.WhenSprint += OnSprint;
        AiSensor.WhenSpotted += OnSpotted;
        MonsterAnim.WhenIdle += OnIdle;
        HideTrigger.WhenHiding += OnHide;
    }

    void OnDisable()
    {
        EventManager.WhenMoved -= OnMove;
        EventManager.WhenSprint -= OnSprint;
        AiSensor.WhenSpotted -= OnSpotted;
        MonsterAnim.WhenIdle -= OnIdle;
        HideTrigger.WhenHiding -= OnHide;
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (playerObj == null)
            playerObj = GameObject.Find("PlayerCapsule");

        navMeshAgent = GetComponent<NavMeshAgent>();

        monsterPos = monCollider.transform.position;
        playerPos = playerObj.transform.position;

        //monsterPath = new NavMeshPath();
    }

    void OnMove()
    {
        if (!isMoving)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    void OnSprint()
    {
        if (!isSprinting)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
    }

    void OnSpotted()
    {
        if (!isSpotted)
        {
            isSpotted = true;
        }
    }

    void OnIdle()
    {
        idle = true;
        roaming = false;
    }

    void OnHide(bool isHiding)
    {
        hiding = isHiding;
    }

    // Update is called once per frame
    void Update()
    {
        //print(playerClose);
        navTimer2 -= Time.deltaTime;
        if (navTimer2 < 0)
        {
            navTimer2 = 1;

            float distance = Vector3.Distance(monCollider.transform.position, playerObj.transform.position);
            //print("distance = "+ distance);
            //detect player within distance
            if (distance < 10)
            {
                //print("playerclose");

                playerClose = true;
                playerCloseDelay = 3;
                if (WhenMonsterClose != null)
                    WhenMonsterClose();
            }
            else
            {

                playerClose = false;
                if (WhenMonsterNotClose != null)
                    WhenMonsterNotClose();

                if (playerCloseDelay > -1)
                {
                    playerCloseDelay--;
                }
            }

        }

        if (isMoving)
        {
            alertLevel += 5;
        }

        if (isSprinting)
        {
            alertLevel += 5;
        }

        if (isSpotted && !hiding)
        {
            alertLevel += 31;
        }

        //print("isHiding = " + hiding);

        if (playerClose || playerCloseDelay > 0)
        {
            if (!hiding)
                alertLevel += 21;
        }

        //if the player was recently being hunted, then keep alert level elevated temporarily as long as player is not hiding
        if (alertLevel < 21 && delayTimer > 1 && !hiding)
        {
            alertLevel = 21;
            dontReset = true;
            if (delayTimer > 0)
                delayTimer--;
        }
        else
        {
            dontReset = false;
            if (delayTimer > 0)
                delayTimer--;
        }

        //once hunting threshold met do this stuff
        if (alertLevel > 20)
        {
            navTimer1 -= Time.deltaTime;
            if (navTimer1 < 0)
            {
                navTimer1 = .1f;

                if (!isAttackingDoor && playerClose)
                {
                    //monsterPath.ClearCorners();
                    //navMeshAgent.CalculatePath(playerObj.transform.position, monsterPath);
                    //NavMesh.CalculatePath(monsterPos, playerObj.transform.position, NavMesh.AllAreas, monsterPath);

                    navMeshAgent.SetDestination(playerObj.transform.position);
                    //print("playerSetAsDestination");
                }
                else if (!isAttackingDoor && !playerClose)
                {

                    int random1 = Random.Range(1, 5);
                    if (random1 == 2)
                    {
                        navMeshAgent.SetDestination(playerObj.transform.position);
                        //print("playerDESTFAR");
                    }

                }
                else
                {
                    navMeshAgent.SetDestination(doorPos);
                }
            }

            roaming = false;

            if (!beingHunted)
            {
                //unity event for being hunted
                beingHunted = true;
                if (WhenHunted != null)
                    WhenHunted(beingHunted);
            }

            //how many frames until hunting expires upon monster losing sight of player
            if (!dontReset && !hiding)
                delayTimer = 400;

        }
        else
        {
            beingHunted = false;
            if (WhenHunted != null)
                WhenHunted(beingHunted);

            //if not hunting player then roam to random navPoint
            if (!roaming && idle)
            {
                print("!roaming && idle");
                roaming = true;
                idle = false;
                navPoints = GameObject.FindGameObjectsWithTag("navPoint");
                int randomNum = Random.Range(0, navPoints.Length);
                GameObject destinationPoint = navPoints[randomNum];
                navMeshAgent.destination = destinationPoint.transform.position;
            }
        }
        //print("alertLevel = " + alertLevel);
        alertLevel = 0;
        isSpotted = false;
    }
}

