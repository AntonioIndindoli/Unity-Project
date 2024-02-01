using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSound : MonoBehaviour
{
    private AudioSource audio2;
    public AudioClip[] monster_Clip;
    private float soundTimer = 0;

    // Start is called before the first frame update
    void Awake()
    {
        audio2 = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        soundTimer -= Time.deltaTime;

        if (soundTimer < 0)
        {
            soundTimer = 50;
            audio2.volume = Random.Range(.3f, 1f);
            audio2.clip = monster_Clip[Random.Range(0, monster_Clip.Length)];
            audio2.Play();
        }

    }
}
