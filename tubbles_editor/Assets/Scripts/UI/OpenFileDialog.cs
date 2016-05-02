using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;

public class OpenFileDialog : MonoBehaviour
{
	private Action<OpenFileDialog.Result> callbackWhenDone;
	private OpenFileDialog.Result result;
	private List<OpenFileDialogItem> mItems;
	private OpenFileDialogItem mSelectedItem;
	private string mPath;

	public InputField ExplorerLocationText;
	public GameObject OpenButton;
	public GameObject CancelButton;
	public GameObject UpButton;
	public GameObject Locator;
	public GameObject Item;

	public Sprite mDirSprite;
	public Sprite mTransparentSprite;
	public Sprite mSelectorSprite;

	public void Initialize(GameObject Parent, Action<OpenFileDialog.Result> Callback)
	{
		this.gameObject.transform.SetParent(Parent.transform, false);
		callbackWhenDone = Callback;
		result = new Result();
		result.result = Result.Enum.Cancel;

		mItems = new List<OpenFileDialogItem>();
		ExplorerLocationText.text = Path.GetFullPath(Directory.GetCurrentDirectory()) + "\\Assets\\Maps\\";

		CancelButton.GetComponent<Button>().onClick.AddListener(cbCancelButton);
		OpenButton.GetComponent<Button>().onClick.AddListener(cbOkButton);
		UpButton.GetComponent<Button>().onClick.AddListener(cbUpButton);

		UpdateLocator();
	}

	public void Finish()
	{
		if(callbackWhenDone != null)
			callbackWhenDone(result);
		this.gameObject.transform.parent.gameObject.SetActive(false);
		Destroy(this.gameObject);
	}

	public void cbCancelButton()
	{
		result.result = Result.Enum.Cancel;
		Finish();
	}

	public void cbOkButton()
	{
		if(mSelectedItem.mItemType == OpenFileDialogItem.ItemType.Directory)
		{
			ExplorerLocationText.text += mSelectedItem.mText.text + "\\";
			UpdateLocator();
		}
		else
		{
			result.result = Result.Enum.Open;
			result.file = ExplorerLocationText.text + mSelectedItem.mText.text;
			Finish();
		}
	}

	public void cbUpButton()
	{
		string[] temp = ExplorerLocationText.text.Split('\\');

		ExplorerLocationText.text = "";

		for(int i = 0; i < temp.Length - 2; ++i)
		{
			ExplorerLocationText.text += temp[i] + "\\";
			// if(i < temp.Length - 2) ExplorerLocationText.text += "\\";
		}

		if(ExplorerLocationText.text == "")
		{
			Debug.Log(temp[0]);
			ExplorerLocationText.text = temp[0] + "\\";
		}

		UpdateLocator();
	}

	public void UpdateLocator()
	{
		foreach(var i in mItems)
		{
			i.Delete();
		}

		mItems = new List<OpenFileDialogItem>();

		string[] dirs = Directory.GetDirectories(ExplorerLocationText.text);
		string[] files = Directory.GetFiles(ExplorerLocationText.text);

		foreach(var d in dirs)
		{
			string[] temp = d.Split('\\');
			var inst = Instantiate(Item);
			mItems.Add(inst.GetComponent<OpenFileDialogItem>());
			inst.GetComponent<OpenFileDialogItem>().mImage.sprite = mDirSprite;
			inst.GetComponent<OpenFileDialogItem>().mText.text = temp[temp.Length - 1];
			inst.GetComponent<OpenFileDialogItem>().mSelectorImage.sprite = mTransparentSprite;
			inst.transform.SetParent(Locator.transform);
			inst.GetComponent<OpenFileDialogItem>().Initialize(this, OpenFileDialogItem.ItemType.Directory);
		}

		foreach(var f in files)
		{
			string[] temp = f.Split('\\');
			var inst = Instantiate(Item);
			mItems.Add(inst.GetComponent<OpenFileDialogItem>());
			inst.GetComponent<OpenFileDialogItem>().mImage.sprite = mTransparentSprite;
			inst.GetComponent<OpenFileDialogItem>().mText.text = temp[temp.Length - 1];
			inst.GetComponent<OpenFileDialogItem>().mSelectorImage.sprite = mTransparentSprite;
			inst.transform.SetParent(Locator.transform);
			inst.GetComponent<OpenFileDialogItem>().Initialize(this, OpenFileDialogItem.ItemType.File);
		}
	}

	public void SelectItem(OpenFileDialogItem item)
	{
		mSelectedItem = item;

		foreach(var i in mItems)
		{
			i.mSelectorImage.sprite = mTransparentSprite;
		}
		item.mSelectorImage.sprite = mSelectorSprite;

		OpenButton.GetComponent<Button>().interactable = true;
	}

	public class Result
	{
		public enum Enum
		{
			Open,
			Cancel,
		}

		public Result.Enum result;
		public string file;
	}
}
