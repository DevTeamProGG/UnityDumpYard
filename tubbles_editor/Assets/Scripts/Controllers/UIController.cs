using System;

using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	private EditorController mEditor;

	private GameObject mCanvas;

	void Start()
	{
		mEditor = EditorController.Instance;

		mCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
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

	public void newSelectFileDialog(Action<SelectFileDialog.Result> Callback, string Title)
	{
		var go = Instantiate(mEditor.mPrefabSelector.SelectFileDialog);
		go.transform.SetParent(mCanvas.transform, false);
		var ofd = go.GetComponentInChildren<SelectFileDialog>();

		ofd.Initialize(Callback, Title);
	}

	public void newSelectFileDialog(Action<SelectFileDialog.Result> Callback)
	{
		var go = Instantiate(mEditor.mPrefabSelector.SelectFileDialog);
		go.transform.SetParent(mCanvas.transform, false);
		var ofd = go.GetComponentInChildren<SelectFileDialog>();

		ofd.Initialize(Callback);
	}

	public void newOkDialog(Dialog.Type reason, string msg, Action cbWhenOk)
	{
		var go = Instantiate(mEditor.mPrefabSelector.Dialog);
		var d = go.GetComponentInChildren<Dialog>();

		d.InitializeOk(mCanvas, reason, msg, cbWhenOk);
	}

	public void newYesNoDialog(Dialog.Type reason, string msg, Action cbWhenYes, Action cbWhenNo)
	{
		var go = Instantiate(mEditor.mPrefabSelector.Dialog);
		var d = go.GetComponentInChildren<Dialog>();

		d.InitializeYesNo(mCanvas, reason, msg, cbWhenYes, cbWhenNo);
	}
}

