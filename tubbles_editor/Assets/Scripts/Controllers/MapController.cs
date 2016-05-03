using System;
using System.IO;
using System.Collections;

using UnityEngine;

public class MapController
{
	private EditorController mEditor;

	private GameObject[] mMap;
	private String mMapName = "";
	private string mLastSaveName;
	private GameObject mParent;

	private IntVector2 mMapSize;

	private const byte mHead1 = 0x4D, mHead2 = 0x41, mHead3 = 0x50;
	private const byte mMapReaderVersion = 0x01; // INCREMENT EACH TIME NON BACKWARDS COMPATIBLE CHANGES TO MAP STRUCTURE ARE MADE
	private byte[] mMapHeaderv1 = {mHead1, mHead2, mHead3, mMapReaderVersion};

	public MapController()
	{
		mEditor = EditorController.Instance;
		mParent = mEditor.gameObject;

		generateDefaultMap();
	}

	public void saveMap(BinaryWriter bw)
	{
		if(mMapName == "")
		{
			Debug.Log("Cannot save a map without a name. Define a name first.");
			return;
		}

		foreach(var b in mMapHeaderv1)
		{
			bw.Write(b);
		}

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

		Debug.Log("Map saved sucessfully: " + mMapName);
	}

	public void saveMapAs(String s)
	{
		mMapName = s;
		saveMap();
	}

	public void saveMap(SelectFileDialog.Result Result)
	{
		if(Result.result == SelectFileDialog.Result.Enum.Select)
		{
			string[] temparr = Result.file.Split('\\');
			mMapName = temparr[temparr.Length - 1];

			if(File.Exists(Result.file))
			{
				mLastSaveName = Result.file;
				mEditor.mUIController.newYesNoDialog(Dialog.Type.QOverwriteFile, "Overwrite already existing file:\n" + Result.file + "?", saveMap, null);
				return;
			}

			var bw = new BinaryWriter(File.Open(Result.file, FileMode.Create));
			saveMap(bw);
		}
		else
		{

		}
	}

	public void saveMap()
	{
		var bw = new BinaryWriter(File.Open(mLastSaveName, FileMode.Create));
		saveMap(bw);
	}

	public void loadMap(SelectFileDialog.Result Result)
	{
		if(Result.result == SelectFileDialog.Result.Enum.Select)
		{
			string[] temparr = Result.file.Split('\\');
			mMapName = temparr[temparr.Length - 1];

			if(!File.Exists(Result.file))
			{
				mEditor.mUIController.newOkDialog(Dialog.Type.EFileDoesNotExist, "Cannot open file: File does not exist.", null);
				return;
			}

			var br = new BinaryReader(File.Open(Result.file, FileMode.Open));
			loadMap(br);
		}
		else
		{
			
		}
	}

	private void loadMap(BinaryReader br)
	{
		if(br.PeekChar() == mMapHeaderv1[0])
		{
			Debug.Log("Map v1 detected");
			for(int i = 0; i < mMapHeaderv1.Length; ++i)
			{
				byte b = br.ReadByte();
				if(b != mMapHeaderv1[i])
				{
					mEditor.mUIController.newOkDialog(Dialog.Type.EFileIsNotCorrectFormat, "Cannot open file: File is corrupted.", null);
					Debug.Log("Map corrupted. Exiting.");
					return;
				}
			}
		}

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
		Debug.Log("Map loaded sucessfully");
	}

	public void generateDefaultMap()
	{
		mMapSize = new IntVector2(100, 100);
		clearMap();
	}

	public void clearMap()
	{
		clearMap("empty");
	}

	public void clearMap(string clearSprite)
	{
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
				c.setSprite(mEditor.spriteAtlasController.getRandomizedSprite(clearSprite));
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
