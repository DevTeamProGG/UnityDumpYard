using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class TilePickerItem : MonoBehaviour 
{
	public Image mImage;
	public string mText;
	public string mSpriteName;

	EditorController mEditor;

	void Start()
	{
		mEditor = EditorController.Instance;
	}

	public void cbOnPointerEnter()
	{
		mEditor.mUIController.setInfoPanelTextAndActive(mSpriteName, true);
	}

	public void cbOnPointerLeave()
	{
		mEditor.mUIController.setInfoPanelTextAndActive("", false);
	}

	public void cbOnPointerClick()
	{
		mEditor.mUIController.setCurrentTileBrush(this);
	}
}
