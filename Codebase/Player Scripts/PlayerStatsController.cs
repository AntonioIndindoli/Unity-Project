using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using StarterAssets;

public class PlayerStatsController : MonoBehaviour
{
    public delegate void StaminaDrained();
    public static event StaminaDrained WhenStaminaDrained;

    public delegate void StaminaFull();
    public static event StaminaFull WhenStaminaFull;

    public delegate void PlayerDamaged();
    public static event PlayerDamaged WhenPlayerDamaged;

    public delegate void PlayerDead();
    public static event PlayerDead WhenPlayerDead;

    public float playerHealth = 100f;
    public float playerStamina = 100f;
    public float staminaDecay = 1f;
    private bool isPlayerSprinting = false;
    private bool isPlayerCrouching = false;
    private bool stopDrain = false;
    private bool delayRegen = false;
    private const string monsterTag = "monster";

    private bool doOnce = false;

    public StarterAssetsInputs playerInputs;

    void OnEnable()
    {
        MonsterAnim.WhenAttack += OnAttack;
    }

    void OnDisable()
    {
        MonsterAnim.WhenAttack -= OnAttack;
    }

    void OnSprint(InputValue value)
    {
        //print(isPlayerSprinting);
        if (!isPlayerSprinting)
        {
            isPlayerSprinting = true;
        }
        else
        {
            isPlayerSprinting = false;
        }
    }

    void OnCrouch(InputValue value)
    {
        //print(isPlayerSprinting);
        if (!isPlayerCrouching)
        {
            isPlayerCrouching = true;
        }
        else
        {
            isPlayerCrouching = false;
        }
    }


    //called when monster attacks
    //checks if player is close enough to monster and then damages player
    void OnAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 1.5f);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag(monsterTag))
            {
                if (!doOnce)
                {
                    doOnce = true;

                    //sends random inputs to players camera
                    float ran1 = Random.Range(-1f, 1f);
                    float ran2 = Random.Range(-5f, 5f);
                    playerInputs.look.Set(ran1, ran2);

                    //event for damage
                    if (WhenPlayerDamaged != null)
                        WhenPlayerDamaged();
                }
                playerHealth -= .001f;
            }
            else
            {
                doOnce = false;
            }
        }

        if (playerHealth < 0)
        {
            if (WhenPlayerDead != null)
                WhenPlayerDead();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //print(playerHealth);
        if (playerStamina < 0f)
        {
            if (WhenStaminaDrained != null)
                WhenStaminaDrained();
            stopDrain = true;
        }

        if (playerStamina > 100f)
        {
            if (WhenStaminaFull != null)
                WhenStaminaFull();
            stopDrain = false;
        }

        if (isPlayerSprinting && playerStamina > 0f && !stopDrain && !isPlayerCrouching)
        {
            playerStamina -= staminaDecay * Time.deltaTime;
            delayRegen = true;
            StopAllCoroutines();
            StartCoroutine(Delay5Sec());
        }

        if (playerStamina < 100f && !delayRegen)
        {
            playerStamina += staminaDecay * Time.deltaTime;
        }


    }

    IEnumerator Delay5Sec()
    {
        yield return new WaitForSeconds(10);
        delayRegen = false;
    }


}
