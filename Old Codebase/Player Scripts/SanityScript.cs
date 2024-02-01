using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanityScript : MonoBehaviour
{
    //sanitylevel, higher is worse
    int sanityLevel;

    public delegate void SanityLowered(int sanityLevel);
    public static event SanityLowered WhenSanityLowered;

    public delegate void SanityRaised(int sanityLevel);
    public static event SanityRaised WhenSanityRaised;

    public delegate void GenerateFigures();
    public static event GenerateFigures WhenGenerateFigures;

    private const string lightTag = "light";
    private Light lightObj;
    int frame;
    int ran1;
    int ran2;
    public AudioClip lightOn_Clip;
    public AudioSource audioSourceObj;
    bool MonsterClose;
    private Collider[] hitColliders;
    private float scanforLightTimer;
    private float figureGenTimer;
    private bool doOnce = false;
    private float distance;
    private bool playerInLight;
    private int playerInLightTimer = 500;

    private bool onCooldown = false;

    public Image sanityStatus;
    public Sprite calm;
    public Sprite unsettled;
    public Sprite panicked;
    public Sprite insane;

    // Start is called before the first frame update
    void Start()
    {
        sanityLevel = 0;
        ran1 = Random.Range(1, 10) * 10;
    }

    void WhenMonsterCloseEvent()
    {
        MonsterClose = true;
    }

    void WhenMonsterNotClose()
    {
        MonsterClose = false;
    }

    // Update is called once per frame
    void Update()
    {
        frame++;

        if (!playerInLight)
        {
            //playerInLightTimer--;
        }
        else
        {
            playerInLightTimer++;
        }
        if (playerInLightTimer < 0)
        {
            sanityLevel++;

            if (WhenSanityLowered != null)
                WhenSanityLowered(sanityLevel);

            playerInLightTimer = 500;
            print("sanityLowered from darkness sanity = " + sanityLevel);
        }
        else if (playerInLightTimer > 1500)
        {
            sanityLevel--;

            if (WhenSanityRaised != null)
                WhenSanityRaised(sanityLevel);

            playerInLightTimer = 500;
            print("sanity raised from light sanity = " + sanityLevel);
        }

        scanforLightTimer -= Time.deltaTime;
        if (scanforLightTimer < 0)
        {
            if(sanityLevel < 5)
            {
                sanityStatus.sprite = calm;
            }
            else if (sanityLevel < 10)
            {
                sanityStatus.sprite = unsettled;
            }
            else if (sanityLevel < 19)
            {
                sanityStatus.sprite = panicked;
            }
            else
            {
                sanityStatus.sprite = insane;
            }


            scanforLightTimer += 5;
            hitColliders = Physics.OverlapSphere(this.transform.position, 10, 1 << 16);
        }

        if (scanforLightTimer < 1)
        {
            playerInLight = false;
        }

        for (int i = 0; i < hitColliders.Length; i++)
        {
            ran1 = Random.Range(1, 10) * 10;
            ran2 = Random.Range(1, 10) * 10;
            //detect lights and flicker them on and off
            if (hitColliders[i].CompareTag(lightTag))
            {
                lightObj = hitColliders[i].gameObject.GetComponentInChildren<Light>();
                audioSourceObj = hitColliders[i].gameObject.GetComponentInChildren<AudioSource>();

                if (scanforLightTimer < 1)
                {
                    distance = Vector3.Distance(this.transform.position, lightObj.transform.position);
                    if (distance < 6 && lightObj.intensity > 0)
                    {
                        playerInLight = true;
                    }
                }

                if (sanityLevel > 2 && MonsterClose)
                {
                    if (!doOnce)
                    {
                        RenderSettings.ambientLight = new Color32(3, 3, 0, 0);
                        doOnce = true;
                    }

                    if (frame % ran1 == 0)
                    {
                        lightObj.intensity = 0;
                        audioSourceObj.volume = 0;
                    }
                    else if (frame % ran2 == 0)
                    {
                        lightObj.intensity = .5f;
                        Vector3 pos = lightObj.transform.position;
                        AudioSource.PlayClipAtPoint(lightOn_Clip, pos, .04f);
                        audioSourceObj.volume = 1;
                    }
                }
            }
        }

        figureGenTimer -= Time.deltaTime;
        if (figureGenTimer < 0 && sanityLevel > 4)
        {
            ran1 = Random.Range(1, 15);
            figureGenTimer += 5;
            if (ran1 == 5)
            {
                if (WhenGenerateFigures != null)
                    WhenGenerateFigures();
            }
        }

        if (sanityLevel < 0)
        {
            sanityLevel = 0;
        }
        else if (sanityLevel > 20)
        {
            sanityLevel = 20;
        }
    }

    void OnEnable()
    {
        MonsterNav.WhenHunted += OnHunted;
        PlayerSensor.WhenMonsterSpotted += SpottedAffects;
        MonsterNav.WhenMonsterClose += WhenMonsterCloseEvent;
        MonsterNav.WhenMonsterNotClose += WhenMonsterNotClose;
        PlayerStatsController.WhenPlayerDamaged += playerHit;
    }

    void OnDisable()
    {
        MonsterNav.WhenHunted -= OnHunted;
        PlayerSensor.WhenMonsterSpotted -= SpottedAffects;
        MonsterNav.WhenMonsterClose -= WhenMonsterCloseEvent;
        MonsterNav.WhenMonsterNotClose -= WhenMonsterNotClose;
        PlayerStatsController.WhenPlayerDamaged -= playerHit;
    }

    void OnHunted(bool beingHunted)
    {
        /*
        print("hunt");
        if (beingHunted)
        {
            sanityLevel += 1;
        }
        if (sanityLevel > 0 && sanityLevel < 25 && beingHunted)
        {
            if (WhenSanityLowered != null)
                WhenSanityLowered(sanityLevel);
        }
        */
    }

    void playerHit()
    {
        sanityLevel += 1;
    }

    void SpottedAffects()
    {
        if (!onCooldown)
        {
            sanityLevel += 1;

            if (WhenSanityLowered != null)
                WhenSanityLowered(sanityLevel);

            onCooldown = true;
            StartCoroutine(SanityDropCoolDown());
        }
    }

    IEnumerator SanityDropCoolDown()
    {
        float timeElapsed = 3;
        while (timeElapsed > 0)
        {
            timeElapsed -= Time.deltaTime;

            yield return null;
        }
        onCooldown = false;
    }

}
