using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip alert_Clip;
    public AudioClip panic_Clip;
    public AudioClip lowSanity_Clip;
    public AudioClip open_Clip;
    public AudioClip squeak_Clip;
    public AudioClip close_Clip;
    public AudioClip lock_Clip;
    public AudioClip unlock_Clip;
    public AudioClip lockJiggle_Clip;
    public AudioSource audio1;
    private bool animPlaying;
    private int frame;
    private float musicTimer;
    private bool hunting = false;


    // Start is called before the first frame update
    void Start()
    {
        audio1 = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        frame++;
        musicTimer -= Time.deltaTime;
        if (hunting)
        {
            if (musicTimer < 0)
            {
                audio1.PlayOneShot(panic_Clip, .4f);
                musicTimer = 24;
            }
        }

    }

    void OnEnable()
    {
        MonsterNav.WhenHunted += OnHunted;
        //PlayerSensor.WhenMonsterSpotted += SpottedAffects;
        SanityScript.WhenSanityLowered += OnLoweredMusic;
        DoorRaycast.WhenDoorEvent += DoorEvent;
    }

    void OnDisable()
    {
        MonsterNav.WhenHunted -= OnHunted;
        //PlayerSensor.WhenMonsterSpotted -= SpottedAffects;
        SanityScript.WhenSanityLowered -= OnLoweredMusic;
        DoorRaycast.WhenDoorEvent -= DoorEvent;
    }

    void OnHunted(bool beingHunted)
    {
        if (beingHunted)
        {
            hunting = true;
        }
        else
        {
            hunting = false;
        }
        //print("playSound!!");
        //alert_Sound.volume = 1;
        //alert_Sound.clip = alert_Clip;
        //if (!alert_Sound.isPlaying)
        //    alert_Sound.Play();
    }

    /*
    void SpottedAffects()
    {
        //if (!alert_Sound.isPlaying)
        //   alert_Sound.Play();

        audio1.PlayOneShot(alert_Clip, .3f);

    }

    */

    void OnLoweredMusic (int sanity1)
    {
        audio1.PlayOneShot(alert_Clip, .3f);

        if (sanity1 > 5 && sanity1 <= 10)
        audio1.PlayOneShot(lowSanity_Clip, .2f);

        if (sanity1 > 10 && sanity1 <= 15)
            audio1.PlayOneShot(lowSanity_Clip, .3f);

        if (sanity1 > 15)
            audio1.PlayOneShot(lowSanity_Clip, .6f);
    }

    void DoorEvent(int eventNumber, Vector3 pos)
    {
        if (eventNumber == 1)
        {
            AudioSource.PlayClipAtPoint(open_Clip, pos, .1f);
            animPlaying = true;
            StopAllCoroutines();
            StartCoroutine(Delay1Sec());
        }


        if (eventNumber == 2)
            AudioSource.PlayClipAtPoint(squeak_Clip, pos, .1f);

        if (eventNumber == 3 && !animPlaying) { 
            AudioSource.PlayClipAtPoint(close_Clip, pos, .1f);
            animPlaying = true;
            StopAllCoroutines();
            StartCoroutine(Delay1Sec());
        }

        if (eventNumber == 4)
        {
            AudioSource.PlayClipAtPoint(lock_Clip, pos, .5f);
        }

        if (eventNumber == 5)
        {
            AudioSource.PlayClipAtPoint(unlock_Clip, pos, .5f);
        }

        if (eventNumber == 6)
        {
            AudioSource.PlayClipAtPoint(lockJiggle_Clip, pos, .5f);
        }
    }

    IEnumerator Delay1Sec()
    {
        frame = 0;
        yield return new WaitUntil(() => frame >= 70);
        animPlaying = false;
    }

}
