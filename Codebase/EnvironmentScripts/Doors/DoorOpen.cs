using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public int doorType;
    float lerpDuration = 1f;
    public bool doorOpen = false;
    public bool doorLocked = false;
    public bool doorDestroyed = false;
    public int doorHealth = 200;
    private bool animPlaying;
    private Transform trigger;
    Quaternion defaultRotation;

    private Transform doorContainer;
    private MeshFilter doorMesh;
    private GameObject damagedDoor;
    private GameObject damagedDoor2;
    private GameObject damagedDoor3;
    int doOnce = 0;

    private GameObject doorParticleSystemObj;
    private ParticleSystem doorParticleSystem;

    private AudioClip bang1;
    private AudioClip bang2;
    private AudioClip bang3;
    private AudioClip bang4;

    private void Awake()
    {
        defaultRotation = this.transform.rotation;
        trigger = this.gameObject.transform.GetChild(0);
        doorContainer = this.transform.parent;
        //doorContainer = doorContainer.parent;
        doorMesh = this.GetComponentInChildren<MeshFilter>();

        bang1 = Resources.Load("Bang1", typeof(AudioClip)) as AudioClip;
        bang2 = Resources.Load("Bang2", typeof(AudioClip)) as AudioClip;
        bang3 = Resources.Load("Bang3", typeof(AudioClip)) as AudioClip;
        bang4 = Resources.Load("Bang4", typeof(AudioClip)) as AudioClip;
    }

    public void UpdateHealth()
    {

        if (doorHealth < 160 && doOnce < 1)
        {
            doorParticleSystemObj = Instantiate(Resources.Load("Doorhit", typeof(GameObject)) as GameObject, doorMesh.transform.position, doorMesh.transform.rotation, this.transform);
            doorParticleSystem = doorParticleSystemObj.GetComponent<ParticleSystem>();

            AudioSource.PlayClipAtPoint(bang1, doorMesh.transform.position, 1f);
            AudioSource.PlayClipAtPoint(bang4, doorMesh.transform.position, 1f);

            doOnce = 1;
            doorParticleSystem.Play();
        }
        else if (doorHealth < 130 && doOnce == 1)
        {
            if (doorType == 1)
            {
                damagedDoor = Instantiate(Resources.Load("house_damageddoor1", typeof(GameObject)) as GameObject, doorMesh.transform.position, doorMesh.transform.rotation, this.transform);
                Instantiate(Resources.Load("house_Gibs1", typeof(GameObject)) as GameObject, doorMesh.transform.position, doorMesh.transform.rotation);
            }
            else
            {
                damagedDoor = Instantiate(Resources.Load("doordamaged1", typeof(GameObject)) as GameObject, doorMesh.transform.position, doorMesh.transform.rotation, this.transform);
                Instantiate(Resources.Load("DoorGibsModel1", typeof(GameObject)) as GameObject, doorMesh.transform.position, doorMesh.transform.rotation);
            }

            AudioSource.PlayClipAtPoint(bang1, doorMesh.transform.position, 1f);

            Destroy(doorMesh.gameObject);
            doOnce = 2;
            doorParticleSystem.Play();
        }
        else if (doorHealth < 100 && doOnce == 2)
        {
            AudioSource.PlayClipAtPoint(bang2, damagedDoor.transform.position, 1f);

            doOnce = 3;
            doorParticleSystem.Play();
        }
        else if (doorHealth < 50 && doOnce == 3)
        {
            doOnce = 4;

            if (doorType == 1)
            {
                damagedDoor2 = Instantiate(Resources.Load("house_damageddoor2", typeof(GameObject)) as GameObject, damagedDoor.transform.position, damagedDoor.transform.rotation, this.transform);
                Instantiate(Resources.Load("house_gibs2", typeof(GameObject)) as GameObject, damagedDoor.transform.position, damagedDoor.transform.rotation);
            }
            else
            {
                damagedDoor2 = Instantiate(Resources.Load("damageddoor2", typeof(GameObject)) as GameObject, damagedDoor.transform.position, damagedDoor.transform.rotation, this.transform);
                Instantiate(Resources.Load("DoorGibsModel2", typeof(GameObject)) as GameObject, damagedDoor.transform.position, damagedDoor.transform.rotation);
            }

            AudioSource.PlayClipAtPoint(bang2, damagedDoor.transform.position, 1f);
            Destroy(damagedDoor);
        }
        else if (doorHealth <= 0 && doOnce == 4)
        {
            doOnce = 5;
            if (doorType == 1)
            {
                damagedDoor3 = Instantiate(Resources.Load("house_doorHalf", typeof(GameObject)) as GameObject, damagedDoor2.transform.position, damagedDoor2.transform.rotation);
            }
            else
            {
                damagedDoor3 = Instantiate(Resources.Load("DoorHalfGibs", typeof(GameObject)) as GameObject, damagedDoor2.transform.position, damagedDoor2.transform.rotation);
            }

            AudioSource.PlayClipAtPoint(bang3, damagedDoor2.transform.position, 1f);
            AudioSource.PlayClipAtPoint(bang4, damagedDoor2.transform.position, 1f);

            Destroy(damagedDoor2);
            doorDestroyed = true;
        }
    }

    public void PlayAnimation()
    {
        if (transform.rotation.eulerAngles.y < 90)
        {
            //doorOpen = false;
        }
        if (!doorOpen && !animPlaying && !doorLocked)
        {
            StartCoroutine(Rotate90());
        }
    }

    IEnumerator Rotate90()
    {
        animPlaying = true;
        float timeElapsed = 0;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = defaultRotation * Quaternion.Euler(0, 90, 0);
        Quaternion targetRotation2 = defaultRotation * Quaternion.Euler(0, -90, 0);
        while (timeElapsed < lerpDuration && transform.rotation.eulerAngles.y < 90)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        trigger.transform.rotation = targetRotation2;

        //doorOpen = true;
        animPlaying = false;
    }


}
