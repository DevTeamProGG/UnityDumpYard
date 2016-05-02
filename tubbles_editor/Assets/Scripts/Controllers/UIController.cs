using System;

using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	private EditorController mEditor;

	private GameObject mCanvas;
	private GameObject mFullscreenPanel;

	void Start()
	{
		mEditor = EditorController.Instance;

		mCanvas = GameObject.FindGameObjectWithTag("MainCanvas");

		mFullscreenPanel = new GameObject();
		JUI.setupFullscreenPanel(mFullscreenPanel, mCanvas, "Fullscreen Panel");
		mFullscreenPanel.SetActive(false);
	}

	private void toggleUI(GameObject panel)
	{
		panel.SetActive(!panel.activeSelf);
	}

	public void hideUI(GameObject panel)
	{
		panel.SetActive(false);
	}

	public void showUI(GameObject panel)
	{
		panel.SetActive(true);
	}

	public void newOpenFileDialog(Action<OpenFileDialog.Result> Callback)
	{
		var go = Instantiate(mEditor.mPrefabSelector.OpenFileDialog);
		var ofd = go.GetComponent<OpenFileDialog>();
		ofd.Initialize(mFullscreenPanel, Callback);
		mFullscreenPanel.SetActive(true);
	}
}

