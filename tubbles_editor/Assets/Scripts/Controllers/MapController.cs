using System;
using System.IO;
using System.Collections;

using UnityEngine;

public class MapController
{
	private EditorController mEditor;

	private GameObject[] mMap;
	private String mMapName;
	private GameObject mParent;

	private IntVector2 mMapSize;

	public MapController(EditorController editor)
	{
		mEditor = editor;
		mParent = mEditor.gameObject;

		loadDefaultMap();
	}

	public void saveMap()
	{
		var bw = new BinaryWriter(File.Open(Application.dataPath + "/Maps/" + mMapName + ".map", FileMode.Create));

		bw.Write(mMapSize.x);
		bw.Write(mMapSize.y);

		for(int i = 0; i < mMapSize.x; ++i)
		{
			for(int j = 0; j < mMapSize.y; ++j)
			{
				Cell c = getCellAtWorldCoord(i,j);
				bw.Write(c.SpriteName);
				bw.Write(c.Index);
			}
		}

		bw.Close();

		Debug.Log("Map saved sucessfully: " + mMapName + ".map");
	}

	public void saveMapAs(String s)
	{
		mMapName = s;
		saveMap();
	}

	public void loadMap(String s)
	{
		mMapName = s;

		var br = new BinaryReader(File.Open(Application.dataPath + "/Maps/" + mMapName + ".map", FileMode.Open));

		mMapSize = new IntVector2(br.ReadInt32(), br.ReadInt32());

		mMap = new GameObject[mMapSize.x*mMapSize.y];
		for(int i = 0; i < mMapSize.x; ++i)
		{
			for(int j = 0; j < mMapSize.y; ++j)
			{
				mMap[i*mMapSize.x+j] = new GameObject();
				mMap[i*mMapSize.x+j].transform.parent = mParent.transform;
				mMap[i*mMapSize.x+j].transform.position = new Vector3(i, j, 0);
				mMap[i*mMapSize.x+j].transform.name = "cell_" + i + "_" + j;

				Cell c = mMap[i*mMapSize.x+j].gameObject.AddComponent<Cell>();
				c.setSpriteRenderer(mMap[i*mMapSize.x+j].gameObject.AddComponent<SpriteRenderer>());
				c.setSprite(mEditor.spriteAtlasController.getIndexedSprite(br.ReadString(), br.ReadInt32()));
			}
		}

		br.Close();

		Debug.Log("Map loaded sucessfully: " + mMapName + ".map");
	}

	public void loadDefaultMap()
	{
		mMapSize = new IntVector2(100, 100);

		mMap = new GameObject[mMapSize.x*mMapSize.y];
		for(int i = 0; i < mMapSize.x; ++i)
		{
			for(int j = 0; j < mMapSize.y; ++j)
			{
				mMap[i*mMapSize.x+j] = new GameObject();
				mMap[i*mMapSize.x+j].transform.parent = mParent.transform;
				mMap[i*mMapSize.x+j].transform.position = new Vector3(i, j, 0);
				mMap[i*mMapSize.x+j].transform.name = "cell_" + i + "_" + j;

				Cell c = mMap[i*mMapSize.x+j].gameObject.AddComponent<Cell>();
				c.setSpriteRenderer(mMap[i*mMapSize.x+j].gameObject.AddComponent<SpriteRenderer>());
				c.setSprite(mEditor.spriteAtlasController.getRandomizedSprite("empty"));
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
			// Debug.Log("Tried to access cell outside the map boundaries: (" + x + ", " + y + ")");
			return null;
		}
		return mMap[x*mMapSize.x + y].transform.GetComponents<Cell>()[0];
	}

	public IntVector2 getCurrentMapSize()
	{
		return mMapSize;
	}
}
