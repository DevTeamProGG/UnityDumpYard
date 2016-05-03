using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class SelectFileDialogItem : MonoBehaviour 
{
	public enum ItemType
	{
		Directory,
		File,
	}

	public Text mText;
	public ItemType mItemType;

	public SelectFileDialog mParentDialog;
	public Image mSelectorImage;
	public Image mImage;

	public void Initialize(SelectFileDialog Parent, ItemType Type)
	{
		mParentDialog = Parent;
		mItemType = Type;
	}

	public void Delete()
	{
		Destroy(this.gameObject);
	}

	public void cbTextItemSelected()
	{
		mParentDialog.SelectItem(this);
	}
}
