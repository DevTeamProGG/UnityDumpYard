using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public enum Mode
	{
		none,
		MainMenu,
	}

	public GameObject buttonPrefab;
	public GameObject buttonRow;
	public Text mTitleRow;

	private EditorController mEditor;
	private Mode mCurrentMode;
	private List<GameObject> buttons;

	void Awake()
	{
		mEditor = EditorController.Instance;
		buttons = new List<GameObject>();
		mCurrentMode = Mode.none;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Finish();
		}
	}

	public void Finish()
	{
		Destroy(this.transform.gameObject);
	}

	public void setMode(Mode mode)
	{
		if(mCurrentMode != mode)
		{
			foreach(var b in buttons)
			{
				buttons.Remove(b);
				Destroy(b);
			}

			mCurrentMode = mode;
			switch(mCurrentMode)
			{
			case Mode.none:
				{
					Debug.Log("Main Menu with mode none.");
					Finish();
					break;
				}
			case Mode.MainMenu:
				{
					mTitleRow.text = "Main Menu, v1.0";
					AddButton("Fill Map", mEditor.mapController.clearMapGrass);
					AddButton("Clear Map", mEditor.mapController.clearMap);
					AddButton("Save map", mEditor.mapController.saveMap);
					AddButton("Save map as", mEditor.mUIController.saveFileDialog);
					AddButton("Load map", mEditor.mUIController.loadFileDialog);
					AddButton("Exit the editor", mEditor.quitGame);
					break;
				}
			default:
				{
					Debug.Log("Main Menu with unknown none.");
					Finish();
					break;
				}
			}
		}
	}

	private void AddButton(string textToShow, Action callback)
	{
		var go = Instantiate(buttonPrefab);
		go.transform.SetParent(buttonRow.transform);

		var bs = go.GetComponentInChildren<ButtonScript>();
		bs.SetupButton(textToShow, callback, this);

		buttons.Add(go);
	}
}
