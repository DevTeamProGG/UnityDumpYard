using System;

using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	private EditorController mEditor;

	private GameObject mPanel;

	public void setEditorController(EditorController controller)
	{
		mEditor = controller;
	}

	void Start()
	{
		mPanel = GameObject.FindGameObjectWithTag("MainPanel");
		toggleUI();
		JUI.addNewButton(mPanel, "TestButton", "Button");
	}

	public void toggleUI()
	{
		mPanel.SetActive(!mPanel.activeSelf);
	}

	public void hideUI()
	{
		mPanel.SetActive(false);
	}

	public void showUI()
	{
		mPanel.SetActive(true);
	}
}

