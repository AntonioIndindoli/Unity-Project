using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseHover : MonoBehaviour
{
	public Text box1;
	public MainMenu menu;

	void Start()
	{

	}

	void OnMouseEnter()
	{
		box1.color = Color.gray;

		if (box1.text.Equals("Start"))
		{
			//menu.isStart = true;
		}
		if (box1.text.Equals("Options"))
		{
			//menu.isOptions = true;
		}
		if (box1.text.Equals("Quit"))
		{
			//menu.isQuit = true;
		}
	}

	void OnMouseExit()
	{
		box1.color = Color.white;

		//menu.isStart = false;
		//menu.isOptions = false;
		//menu.isQuit = false;
	}
}
