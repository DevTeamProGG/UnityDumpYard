using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
	public Text myText;

	private Action cbOnClick;
	private MainMenu mMainMenu;

	public void SetupButton(string textToShow, Action callback, MainMenu parentMenu)
	{
		myText.text = textToShow;
		cbOnClick = callback;
		mMainMenu = parentMenu;
	}

	public void onClick()
	{
		mMainMenu.Finish();
		cbOnClick();
	}
}
