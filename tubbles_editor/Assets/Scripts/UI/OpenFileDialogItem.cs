using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class OpenFileDialogItem : MonoBehaviour 
{
	public enum ItemType
	{
		Directory,
		File,
	}

	public Text mText;
	public ItemType mItemType;

	public OpenFileDialog mParentDialog;
	public Image mSelectorImage;
	public Image mImage;

	public void Initialize(OpenFileDialog Parent, ItemType Type)
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
