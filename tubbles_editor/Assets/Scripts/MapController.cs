using System.Collections;

using UnityEngine;

public class MapController
{
	private EditorController mEditor;

	private GameObject[] map;
	private SpriteAtlasController sac;
	private GameObject mParent;

	private IntVector2 mMapSize;

	public MapController(EditorController editor)
	{
		mEditor = editor;

		this.sac = mEditor.spriteAtlasController;

		mParent = mEditor.gameObject;

		loadDefaultMap();
	}

	public void loadDefaultMap()
	{
		mMapSize = new IntVector2(100, 100);

		map = new GameObject[mMapSize.x*mMapSize.y];
		for(int i = 0; i < mMapSize.x; ++i)
		{
			for(int j = 0; j < mMapSize.y; ++j)
			{
				map[i*mMapSize.x+j] = new GameObject();
				map[i*mMapSize.x+j].transform.parent = mParent.transform;
				map[i*mMapSize.x+j].transform.position = new Vector3(i, j, 0);
				map[i*mMapSize.x+j].transform.name = "cell_" + i + "_" + j;

				Cell c = map[i*mMapSize.x+j].gameObject.AddComponent<Cell>();
				c.setSpriteRenderer(map[i*mMapSize.x+j].gameObject.AddComponent<SpriteRenderer>());
				//TODO: Remove the hard coded string
				c.setSprite(sac.getRandomizedSprite("empty"));
			}
		}
	}

	public Cell getCellAtWorldCoord(Vector2 Position)
	{
		return getCellAtWorldCoord(Position.x, Position.y);
	}

	public Cell getCellAtWorldCoord(float X, float Y)
	{
		int x = (int)Mathf.Round(X);
		int y = (int)Mathf.Round(Y);

		if(x < 0 || y < 0 || x > mMapSize.x-1 || y > mMapSize.y-1)
		{
			Debug.Log("Tried to access cell outside the map boundaries: (" + x + ", " + y + ")");
			return null;
		}
		return map[x*mMapSize.x + y].transform.GetComponents<Cell>()[0];
	}

	public IntVector2 getCurrentMapSize()
	{
		return mMapSize;
	}
}
