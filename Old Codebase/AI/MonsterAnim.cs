using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAnim : MonoBehaviour
{
    private const string Player = "Player";
    Animator monsterAnim;
    NavMeshAgent monsterAgent;
    Vector3 worldDeltaPosition;
    Vector2 groundDeltaPosition;
    Vector2 velocity = Vector2.zero;
    Vector3 monPos1;
    Vector3 monPos2;
    bool attack1 = false;
    bool attack2 = false;
    bool endAttackLoop;
    float scanTimer;
    //private int firstTrigger = -1;
    int endAttackDelay;

    private GameObject playerObj = null;

    private const string interactableTag = "InteractiveObject";
    private DoorOpen raycastedObj;

    private MonsterNav navScript;

    public float notMovingCountdown;

    public delegate void monsterIdle();
    public static event monsterIdle WhenIdle;

    public delegate void monsterAttack();
    public static event monsterAttack WhenAttack;

    void Awake()
    {
        monsterAnim = GetComponent<Animator>();
        monsterAgent = GetComponentInParent<NavMeshAgent>();
        navScript = GetComponentInParent<MonsterNav>();
        monsterAgent.updatePosition = false;

        if (playerObj == null)
            playerObj = GameObject.Find("PlayerCapsule");
    }

    void Update()
    {
        worldDeltaPosition = monsterAgent.nextPosition - transform.position;
        groundDeltaPosition.x = Vector3.Dot(transform.right, worldDeltaPosition);
        groundDeltaPosition.y = Vector3.Dot(transform.forward, worldDeltaPosition);
        velocity = (Time.deltaTime > 1e-5f) ? groundDeltaPosition / Time.deltaTime : velocity = Vector2.zero;
        monsterAnim.SetFloat("velx", velocity.x);
        monsterAnim.SetFloat("vely", velocity.y);

        //monPos1 = this.transform.position + new Vector3(0, 0, 1);
        //monPos2 = this.transform.position + new Vector3(0, 0, -1);
        //Collider[] arr1 = Physics.OverlapSphere(monPos1, 1);
        //Collider[] arr2 = Physics.OverlapSphere(monPos2, 1);
        //Collider[] hitColliders = new Collider[arr1.Length + arr2.Length];
        //Array.Copy(arr1, hitColliders, arr1.Length);
        //Array.Copy(arr2, 0, hitColliders, arr1.Length, arr2.Length);

        float distance = Vector3.Distance(this.transform.position, playerObj.transform.position);

        //detect player and attack
        if (distance < 2)
        {
            monsterAnim.SetBool("isAttacking", true);
            Attack();
            endAttackDelay = 20;
        }
        else
        {
            endAttackDelay--;
            if (endAttackDelay < 0)
            {
                monsterAnim.SetBool("isAttacking", false);
                endAttackLoop = true;
            }
        }


        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 1);
        raycastedObj = null;

        for (int i = 0; i < hitColliders.Length; i++)
        {
            //detect doors and open them
            if (hitColliders[i].CompareTag(interactableTag))
            {
                raycastedObj = hitColliders[i].gameObject.GetComponent<DoorOpen>();
                if (raycastedObj.doorLocked)
                {
                    navScript.isAttackingDoor = true;
                    navScript.doorPos = raycastedObj.GetComponentInChildren<Renderer>().bounds.center;
                    print("attmepted door open");
                    monsterAnim.SetBool("isAttacking", true);
                    Attack();
                    endAttackDelay = 20;
                }
                else
                {
                    navScript.isAttackingDoor = false;
                    OpenDoor(raycastedObj);
                }
            }
        }

        //print("x = " + velocity.x);
        //print("y = " + velocity.y);

        if (velocity.x > 0.01 || velocity.y > 0.01)
        {
            monsterAnim.SetBool("isMoving", true);
            notMovingCountdown = 0;
        }
        else
        {
            notMovingCountdown += Time.deltaTime;
            if (notMovingCountdown > 2)
            {
                print("idle");
                monsterAnim.SetBool("isMoving", false);
                notMovingCountdown = 0;
                if (WhenIdle != null)
                {
                    WhenIdle();
                }

            }

        }


        if (!endAttackLoop)
        {
            scanTimer -= Time.deltaTime;
            if (scanTimer < 0)
            {
                scanTimer += 1;
                AttackSwitch();
            }
        }

        if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("attack1") || monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("attack2"))
        {
            if (monsterAnim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f > .5f)
            {
                if (WhenAttack != null)
                {
                    WhenAttack();
                }

                if (raycastedObj.doorLocked && raycastedObj != null)
                {
                    raycastedObj.doorHealth -= 1;
                    raycastedObj.UpdateHealth();
                    if (raycastedObj.doorDestroyed)
                    {
                        Destroy(raycastedObj.gameObject);
                        navScript.isAttackingDoor = false;
                    }
                }
            }
        }

    }

    void Attack()
    {
        endAttackLoop = false;
    }

    void AttackSwitch()
    {
        if (!attack1)
        {
            if (!endAttackLoop)
            {
                //print("attack1");
                monsterAnim.SetBool("Attack2", false);
                monsterAnim.SetBool("Attack1", true);
                attack1 = true;
                attack2 = false;
            }
        }
        else
        {
            if (!endAttackLoop)
            {
                //print("attack2");
                monsterAnim.SetBool("Attack2", true);
                monsterAnim.SetBool("Attack1", false);
                attack2 = true;
                attack1 = false;
            }
        }
    }

    void OpenDoor(DoorOpen raycastedObj)
    {
        if (!raycastedObj.doorOpen)
        {
            //print("Doornear");
            raycastedObj.PlayAnimation();
            //animPlaying = true;
            //print("animPlaying = true");
            //StartCoroutine(Delay1Sec());
        }
    }


    void OnAnimatorMove()
    {
        transform.position = monsterAgent.nextPosition;
    }
}
