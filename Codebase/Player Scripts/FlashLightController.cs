using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FlashLightController : MonoBehaviour
{
	public delegate void BatteryRemoved();
	public static event BatteryRemoved WhenBatteryRemoved;

	bool flashOn = false;
	bool delayLightFlicker = false;
	private Light flashLightObj;
	public AudioClip flashLight_Clip;
	public AudioClip flashLightFlicker_Clip;
	private float batteryLife = 12f;
	private float batteryDecay = .5f;
	int frame;
	public InventoryManager InventoryManagerScript;

	// Start is called before the first frame update
	void Start()
	{
		flashLightObj = this.gameObject.GetComponent<Light>();
		InventoryManagerScript = this.GetComponentInParent<InventoryManager>();
	}

    // Update is called once per frame
    void Update()
    {
		frame++;
		//print(batteryLife);
		if (batteryLife < 0)
        {
			flashLightObj.intensity = 0;
			flashOn = false;
		}

		//flashlight flickering when low battery
		if (batteryLife < 10f & batteryLife > 9.8f && flashOn)
		{
			if (!delayLightFlicker)
			AudioSource.PlayClipAtPoint(flashLightFlicker_Clip, flashLightObj.transform.position, .5f);

			flashLightObj.intensity = .4f;
			delayLightFlicker = true;
			StartCoroutine(Delay2Sec());
		}
		if (batteryLife < 9.8f && flashOn)
		{
			flashLightObj.intensity = 1f;
		}
		if (batteryLife < 9.75f && flashOn)
		{
			flashLightObj.intensity = 0f;
		}
		if (batteryLife < 9.7f && flashOn)
		{
			flashLightObj.intensity = 1f;
		}
		if (batteryLife < 7f & batteryLife > 6.8f && flashOn)
		{
			if (!delayLightFlicker)
				AudioSource.PlayClipAtPoint(flashLightFlicker_Clip, flashLightObj.transform.position, 1f);

			flashLightObj.intensity = .4f;
			delayLightFlicker = true;
			StartCoroutine(Delay2Sec());
		}
		if (batteryLife < 6.8f && flashOn)
		{
			flashLightObj.intensity = .8f;
		}
		if (batteryLife < 6.75f && flashOn)
		{
			flashLightObj.intensity = 1f;
		}
		if (batteryLife < 6.7f && flashOn)
		{
			flashLightObj.intensity = .8f;
		}
		if (batteryLife < 6.65f && flashOn)
		{
			flashLightObj.intensity = 1f;
		}
		if (batteryLife < 6.6f && flashOn)
		{
			flashLightObj.intensity = .8f;
		}
		if (batteryLife < .5f && flashOn)
		{
			if (!delayLightFlicker)
				AudioSource.PlayClipAtPoint(flashLightFlicker_Clip, flashLightObj.transform.position, 1f);

			flashLightObj.intensity = .1f;
			delayLightFlicker = true;
			StartCoroutine(Delay2Sec());
		}
		if (batteryLife < .45f && flashOn)
		{
			flashLightObj.intensity = .7f;
		}
		if (batteryLife < .4f && flashOn)
		{
			flashLightObj.intensity = .5f;
		}
		if (batteryLife < .1f && flashOn)
		{
			flashLightObj.intensity = .2f;
		}
		if (batteryLife < .05f && flashOn)
		{
			flashLightObj.intensity = .5f;
		}

		//battery decay
		if (flashOn && batteryLife > 0f)
		{
			batteryLife -= batteryDecay * Time.deltaTime;
		}
	}

    void OnFlashLightToggle()
    {
		if (flashOn)
		{
			AudioSource.PlayClipAtPoint(flashLight_Clip, flashLightObj.transform.position, .2f);
			flashLightObj.intensity = 0;
			flashOn = false;
		}
		else if (batteryLife > 0)
		{
			AudioSource.PlayClipAtPoint(flashLight_Clip, flashLightObj.transform.position, .4f);
			flashLightObj.intensity = 1;
			flashOn = true;
		}
	}

	void OnReload()
	{
		if (batteryLife < 30 && InventoryManagerScript.batteryCount > 0)
        {
			if (WhenBatteryRemoved != null)
				WhenBatteryRemoved();

			batteryLife = 100;
		}
	}

	IEnumerator Delay2Sec()
	{
		yield return new WaitForSeconds(2);
		delayLightFlicker = false;
	}
}
