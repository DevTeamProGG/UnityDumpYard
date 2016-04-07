using System.Collections;

using UnityEngine;

public class MapController
{
	private EditorController mEditor;

	private GameObject[] map;
	private SpriteAtlasController sac;
	private GameObject mParent;

	private IntVector2 mMapSize;

	public MapController(EditorController editor, IntVector2 mapSize)
	{
		mEditor = editor;

		this.sac = mEditor.spriteAtlasController;
		mMapSize = mapSize;

		mParent = mEditor.gameObject;

		initializeCells(mMapSize.x, mMapSize.y);
	}

	public void initializeCells(int x, int y)
	{
		map = new GameObject[x*y];
		for(int i = 0; i < x; ++i)
		{
			for(int j = 0; j < y; ++j)
			{
				map[i*x+j] = new GameObject();
				map[i*x+j].transform.parent = mParent.transform;
				map[i*x+j].transform.position = new Vector3(i, j, 0);
				map[i*x+j].transform.name = "cell_" + i + "_" + j;

				Cell c = map[i*x+j].gameObject.AddComponent<Cell>();
				c.setSpriteRenderer(map[i*x+j].gameObject.AddComponent<SpriteRenderer>());
				//TODO: Remove the hard coded string
				c.setSprite(sac.getRandomizedSprite("grass"));
			}
		}
	}

	public Cell getCellAtWorldCoord(Vector2 Position)
	{
		return map[((int)Mathf.Round(Position.x))*mMapSize.x + ((int)Mathf.Round(Position.y))].transform.GetComponents<Cell>()[0];
	}
}
