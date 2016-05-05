using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class TilePicker : MonoBehaviour 
{
	public GameObject mTileItem;
	public GameObject mContent;
	public CurrentTile mCurrentTile;

	public void addAllTiles()
	{
		foreach(string s in EditorController.Instance.spriteAtlasController.getAllAtlasesNames())
		{
			addTile(s, "");
		}
	}

	public void addTile(string tileName, string tileDesc)
	{
		var go = Instantiate(mTileItem);
		go.transform.SetParent(mContent.transform);
		var tile = go.GetComponent<TilePickerItem>();
		tile.mSpriteName = tileName;
		tile.mText = tileDesc;
		tile.mImage.sprite = EditorController.Instance.spriteAtlasController.getIndexedSprite(tileName, 0).sprite;

		go.GetComponent<PropagateDragging>().mainScrollRect = this.gameObject.GetComponentInChildren<ScrollRect>();
	}


}
