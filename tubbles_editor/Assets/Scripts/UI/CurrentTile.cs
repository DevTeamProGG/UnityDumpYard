using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class CurrentTile : MonoBehaviour 
{
	public Image mImage;
	public string mText;
	public string mSpriteName;

	public void cbOnPointerEnter()
	{
		EditorController.Instance.mUIController.setInfoPanelTextAndActive(mSpriteName, true);
	}

	public void cbOnPointerLeave()
	{
		EditorController.Instance.mUIController.setInfoPanelTextAndActive("", false);
	}
}
