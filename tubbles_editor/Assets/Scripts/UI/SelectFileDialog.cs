using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;

public class SelectFileDialog : MonoBehaviour
{
	private Action<SelectFileDialog.Result> callbackWhenDone;
	private SelectFileDialog.Result result;
	private List<SelectFileDialogItem> mItems;
	private SelectFileDialogItem mSelectedItem;
	private DateTime mStamp;
	private string mPath;

	public InputField ExplorerLocationText;
	public InputField FileNameText;
	public GameObject SelectButton;
	public GameObject CancelButton;
	public GameObject UpButton;
	public GameObject Locator;
	public GameObject Item;
	public Text TitleRow;

	[HideInInspector]
	private double mCoolDown = 250.0f;

	public Sprite mDirSprite;
	public Sprite mTransparentSprite;
	public Sprite mSelectorSprite;

	public void Initialize(Action<SelectFileDialog.Result> Callback, string Title)
	{
		TitleRow.text = Title;
		Initialize(Callback);
	}

	public void Initialize(Action<SelectFileDialog.Result> Callback)
	{
		callbackWhenDone = Callback;
		result = new Result();
		result.result = Result.Enum.Cancel;

		mItems = new List<SelectFileDialogItem>();
		ExplorerLocationText.text = Path.GetFullPath(Directory.GetCurrentDirectory()) + "\\Assets\\Maps\\";

		CancelButton.GetComponent<Button>().onClick.AddListener(cbCancelButton);
		SelectButton.GetComponent<Button>().onClick.AddListener(cbOkButton);
		UpButton.GetComponent<Button>().onClick.AddListener(cbUpButton);

		UpdateLocator();
	}

	public void Finish()
	{
		if(callbackWhenDone != null)
			callbackWhenDone(result);
		Destroy(this.transform.parent.gameObject);
	}

	public void cbCancelButton()
	{
		result.result = Result.Enum.Cancel;
		Finish();
	}

	public void cbOkButton()
	{
		if(mSelectedItem != null && mSelectedItem.mItemType == SelectFileDialogItem.ItemType.Directory)
		{
			ExplorerLocationText.text += mSelectedItem.mText.text + "\\";
			UpdateLocator();
		}
		else
		{
			result.result = Result.Enum.Select;
			result.file = ExplorerLocationText.text + FileNameText.text;
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
		}

		if(ExplorerLocationText.text == "")
		{
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

		mItems = new List<SelectFileDialogItem>();

		string[] dirs = Directory.GetDirectories(ExplorerLocationText.text);
		string[] files = Directory.GetFiles(ExplorerLocationText.text);

		foreach(var d in dirs)
		{
			string[] temp = d.Split('\\');
			var inst = Instantiate(Item);
			var sfdi = inst.GetComponent<SelectFileDialogItem>();
			mItems.Add(sfdi);
			sfdi.mImage.sprite = mDirSprite;
			sfdi.mText.text = temp[temp.Length - 1];
			sfdi.mSelectorImage.sprite = mTransparentSprite;
			inst.transform.SetParent(Locator.transform);
			sfdi.Initialize(this, SelectFileDialogItem.ItemType.Directory);
		}

		foreach(var f in files)
		{
			string[] temp = f.Split('\\');
			var inst = Instantiate(Item);
			var sfdi = inst.GetComponent<SelectFileDialogItem>();
			mItems.Add(sfdi);
			sfdi.mImage.sprite = mTransparentSprite;
			sfdi.mText.text = temp[temp.Length - 1];
			sfdi.mSelectorImage.sprite = mTransparentSprite;
			inst.transform.SetParent(Locator.transform);
			sfdi.Initialize(this, SelectFileDialogItem.ItemType.File);
		}
	}

	public void SelectItem(SelectFileDialogItem item)
	{
		double now = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalMilliseconds;
		double last = TimeSpan.FromTicks(mStamp.Ticks).TotalMilliseconds;
		double delta = now - last;

		if(item == mSelectedItem && (delta < mCoolDown))
		{
			if(item.mItemType == SelectFileDialogItem.ItemType.Directory)
			{
				ExplorerLocationText.text += mSelectedItem.mText.text + "\\";
				UpdateLocator();
			}
		}

		mStamp = DateTime.Now;
		mSelectedItem = item;

		foreach(var i in mItems)
		{
			i.mSelectorImage.sprite = mTransparentSprite;
		}

		if(item != null)
		{
			item.mSelectorImage.sprite = mSelectorSprite;
		}

		if(item.mItemType == SelectFileDialogItem.ItemType.File)
		{
			SelectButton.GetComponent<Button>().interactable = true;
		}
		else
		{
			SelectButton.GetComponent<Button>().interactable = false;
		}

		FileNameText.text = item.mText.text;
	}

	public void cbFilenameInputDone()
	{
		if(FileNameText.text != "")
		{
			foreach(var i in mItems)
			{
				if(i.mText.text == FileNameText.text)
				{
					SelectItem(i);
				}
			}
			SelectButton.GetComponent<Button>().interactable = true;
		}
		else
		{
			SelectButton.GetComponent<Button>().interactable = false;
		}


	}

	public class Result
	{
		public enum Enum
		{
			Select,
			Cancel,
		}

		public Result.Enum result;
		public string file;
	}
}
