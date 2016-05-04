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
	private double mCoolDown = 250.0f;
	private static string mLastFolderLocation = "";
	private string[] mFileExtensions = null;

	public InputField ExplorerLocationText;
	public InputField FileNameText;
	public GameObject SelectButton;
	public GameObject CancelButton;
	public GameObject UpButton;
	public GameObject Locator;
	public GameObject Item;
	public Text TitleRow;
	public Sprite mDirSprite;
	public Sprite mTransparentSprite;
	public Sprite mSelectorSprite;

	public void Initialize(Action<SelectFileDialog.Result> Callback, string Title, string[] FileExtensions)
	{
		mFileExtensions = FileExtensions;

		if(Title != null && Title != "")
		{
			TitleRow.text = Title;
		}

		callbackWhenDone = Callback;
		result = new Result();
		result.result = Result.Enum.Cancel;

		mItems = new List<SelectFileDialogItem>();
		if(!Directory.Exists(mLastFolderLocation))
		{
			ExplorerLocationText.text = Path.GetFullPath(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar);
		}
		else
		{
			ExplorerLocationText.text = mLastFolderLocation;
		}

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
			string[] temp = d.Split(Path.DirectorySeparatorChar);
			// string[] temp = d.Split('\\');
			string dirname = temp[temp.Length - 1];

			var inst = Instantiate(Item);
			var sfdi = inst.GetComponent<SelectFileDialogItem>();
			mItems.Add(sfdi);
			sfdi.mImage.sprite = mDirSprite;
			sfdi.mText.text = dirname;
			sfdi.mSelectorImage.sprite = mTransparentSprite;
			inst.transform.SetParent(Locator.transform);
			sfdi.Initialize(this, SelectFileDialogItem.ItemType.Directory);
		}

		foreach(var f in files)
		{
			string[] temp = f.Split(Path.DirectorySeparatorChar);
			// string[] temp = f.Split('\\');
			string filename = temp[temp.Length - 1];
			bool extensionFound = false;

			// CHECK FOR FILE EXTENSIONS
			if(mFileExtensions != null)
			{
				foreach(var s in mFileExtensions)
				{
					if(Path.GetExtension(filename) == s)
					{
						extensionFound = true;
					}
				}
			}
			else
			{
				extensionFound = true;
			}

			if(extensionFound)
			{
				var inst = Instantiate(Item);
				var sfdi = inst.GetComponent<SelectFileDialogItem>();
				mItems.Add(sfdi);
				sfdi.mImage.sprite = mTransparentSprite;
				sfdi.mText.text = filename;
				sfdi.mSelectorImage.sprite = mTransparentSprite;
				inst.transform.SetParent(Locator.transform);
				sfdi.Initialize(this, SelectFileDialogItem.ItemType.File);
			}
		}

		mLastFolderLocation = ExplorerLocationText.text;
	}

	public void SelectItem(SelectFileDialogItem item)
	{
		double now = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalMilliseconds;
		double last = TimeSpan.FromTicks(mStamp.Ticks).TotalMilliseconds;
		double delta = now - last;

		// Debug.Log("delta: " + delta);

		if(item == mSelectedItem && (delta < mCoolDown))
		{
			if(item.mItemType == SelectFileDialogItem.ItemType.Directory)
			{
				ExplorerLocationText.text += mSelectedItem.mText.text + Path.DirectorySeparatorChar;
				// ExplorerLocationText.text += mSelectedItem.mText.text + "\\";
				UpdateLocator();
			}
			if(item.mItemType == SelectFileDialogItem.ItemType.File)
			{
				cbOkButton();
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

	public void SelectItemWithoutClick(SelectFileDialogItem item)
	{
		mStamp = DateTime.Now;
		mSelectedItem = item;

		foreach(var i in mItems)
		{
			i.mSelectorImage.sprite = mTransparentSprite;
		}

		if(item != null)
		{
			item.mSelectorImage.sprite = mSelectorSprite;
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
			ExplorerLocationText.text += mSelectedItem.mText.text + Path.DirectorySeparatorChar;
			// ExplorerLocationText.text += mSelectedItem.mText.text + "\\";
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
		string[] temp = ExplorerLocationText.text.Split(Path.DirectorySeparatorChar);

		ExplorerLocationText.text = "";

		for(int i = 0; i < temp.Length - 2; ++i)
		{
			ExplorerLocationText.text += temp[i] + Path.DirectorySeparatorChar;
		}

		if(ExplorerLocationText.text == "")
		{
			ExplorerLocationText.text = temp[0] + Path.DirectorySeparatorChar;
		}

		UpdateLocator();
	}

	public void cbFilenameInputDone()
	{
		if(FileNameText.text != "")
		{
			bool found = false;
			foreach(var i in mItems)
			{
				if(i.mText.text == FileNameText.text)
				{
					SelectItemWithoutClick(i);
					found = true;
				}
			}
			if(found == false)
			{
				SelectItemWithoutClick(null);
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
