using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UIController : MonoBehaviour
{
	public enum OverlayFadeState
	{
		Full,
		Half,
		Hidden,
	}

	private OverlayFadeState mOverlayFadeState = OverlayFadeState.Full;

	private EditorController mEditor;

	private GameObject mCanvas;
	private GameObject mMainInfoPanel;
	private GameObject mTilePicker;
	private GameObject mCurrentBrush;
	private CurrentTile mCurrentTile;

	private List<GameObject> mOverlayPanels;

	void Start()
	{
		mEditor = EditorController.Instance;

		mCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
		mMainInfoPanel = GameObject.FindGameObjectWithTag("MainInfoPanel");

		mOverlayPanels = new List<GameObject>();
		foreach(var go in GameObject.FindGameObjectsWithTag("UIOverlay"))
		{
			mOverlayPanels.Add(go);
		}

		mCurrentBrush = GameObject.Find("CurrentBrush");
		mCurrentTile = mCurrentBrush.GetComponentInChildren<CurrentTile>();

		clearCurrentTileBrush();
	}

	public void LateStart()
	{
		mTilePicker = GameObject.Find("TilePicker");
		// Debug.Log(mTilePicker);
		mTilePicker.GetComponentInChildren<TilePicker>().addAllTiles();
		setInfoPanelTextAndActive("", false);
	}

	void Update()
	{
		// 00 sw
		// 01 se
		// 10 nw
		// 11 ne

		Vector2 pos = new Vector2(0.0f, 0.0f);

		// GET MOUSE POSITION QUADRANT ON SCREEN

		// Debug.Log(Input.mousePosition);
		string[] res = UnityStats.screenRes.Split('x');
		// Debug.Log(int.Parse(res[0]) + " " + int.Parse(res[1]));

		if(Input.mousePosition.x > int.Parse(res[0]) / 2.0f)
		{
			pos.x = 1.0f;
		}

		if(Input.mousePosition.y > int.Parse(res[1]) / 2.0f)
		{
			pos.y = 1.0f;
		}

		mMainInfoPanel.GetComponent<RectTransform>().pivot = pos;

		mMainInfoPanel.transform.position = Input.mousePosition;
	}

	public void setCurrentTileBrush(TilePickerItem copy)
	{
		mCurrentTile.mImage.sprite = copy.mImage.sprite;
		mCurrentTile.mSpriteName = copy.mSpriteName;
		mCurrentTile.mText = copy.mText;
	}

	public string getCurrentTileBrushName()
	{
		return mCurrentTile.mSpriteName;
	}

	public void clearCurrentTileBrush()
	{
		mCurrentTile.mImage.sprite = mEditor.spriteAtlasController.TransparentSprite;
		mCurrentTile.mSpriteName = "";
		mCurrentTile.mText = "";
	}

	public void setInfoPanelTextAndActive(string text, bool active)
	{
		mMainInfoPanel.GetComponentInChildren<Text>().text = text;
		mMainInfoPanel.gameObject.SetActive(active);
	}

	public void cycleOverlayVisibility()
	{
		switch(mOverlayFadeState)
		{
		case OverlayFadeState.Full:
			{
				mOverlayFadeState = OverlayFadeState.Half;
				foreach(var go in mOverlayPanels)
				{
					go.SetActive(true);
					var im = go.GetComponent<Image>();
					if(im != null)
					{
						im.color = new Color(im.color.r, im.color.g, im.color.b, 0.5f);
					}
				}
				break;
			}
		case OverlayFadeState.Half:
			{
				mOverlayFadeState = OverlayFadeState.Hidden;
				foreach(var go in mOverlayPanels)
				{
					go.SetActive(false);
					var im = go.GetComponent<Image>();
					im.color = new Color(im.color.r, im.color.g, im.color.b, 0.0f);
				}
				break;
			}
		case OverlayFadeState.Hidden:
			{
				mOverlayFadeState = OverlayFadeState.Full;
				foreach(var go in mOverlayPanels)
				{
					go.SetActive(true);
					var im = go.GetComponent<Image>();
					im.color = new Color(im.color.r, im.color.g, im.color.b, 1.0f);
				}
				break;
			}
		}
	}

	public void showMainMenu()
	{
		var go = Instantiate(mEditor.mPrefabSelector.MainMenu);
		go.transform.SetParent(mCanvas.transform, false);

		var mm = go.GetComponent<MainMenu>();
		mm.setMode(MainMenu.Mode.MainMenu);
	}

	public void saveFileDialog()
	{
		mEditor.mUIController.newSelectFileDialog(mEditor.mapController.SaveMapAs, "Select a save location", null);
	}

	public void loadFileDialog()
	{
		mEditor.mUIController.newSelectFileDialog(mEditor.mapController.loadMapAs, "Select a file to load", new string[]{".map"});
	}

	public void newSelectFileDialog(Action<SelectFileDialog.Result> Callback, string Title, string[] FileExtensions)
	{
		var go = Instantiate(mEditor.mPrefabSelector.SelectFileDialog);
		go.transform.SetParent(mCanvas.transform, false);
		var sfd = go.GetComponentInChildren<SelectFileDialog>();

		sfd.Initialize(Callback, Title, FileExtensions);
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

