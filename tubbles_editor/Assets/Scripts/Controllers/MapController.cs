using System;
using System.IO;
using System.Collections;

using UnityEngine;

public class MapController
{
	private EditorController mEditor;

	private GameObject[] mMap;
	private string mFileLocation = "";
	private GameObject mParent;

	private IntVector2 mMapSize;

	private const byte mMapReaderVersion1 = 0x01; // INCREMENT EACH TIME NON BACKWARDS COMPATIBLE CHANGES TO MAP STRUCTURE ARE MADE
	private byte[] mMapHeaderv1 = {(byte)'M', (byte)'A', (byte)'P', mMapReaderVersion1};
	private const int mMapVersionLocationInHeader = 3;

	public MapController()
	{
		mEditor = EditorController.Instance;
		mParent = mEditor.gameObject;

		generateDefaultMap();
	}


	//----------------------------
	// SAVE MAP ROUTINES
	//----------------------------

	public void saveMap()
	{
		if(mFileLocation == "")
		{
			mEditor.mUIController.newSelectFileDialog(mEditor.mapController.SaveMapAs, "Select a save location", null);
			return;
		}

		saveMapWorker();
	}

	public void SaveMapAs(SelectFileDialog.Result Result)
	{
		if(Result.result == SelectFileDialog.Result.Enum.Select)
		{
			mFileLocation = Result.file;

			if(File.Exists(mFileLocation))
			{
				mEditor.mUIController.newYesNoDialog(Dialog.Type.QOverwriteFile, "Overwrite already existing file:\n" + Result.file + "?", saveMapWorker, SaveMapAsCancel);
				return;
			}

			saveMapWorker();
		}
	}

	private void saveMapWorker()
	{
		var bw = new BinaryWriter(File.Open(mFileLocation, FileMode.Create));

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
		Debug.Log("Map saved sucessfully: " + Path.GetFileName(mFileLocation));
	}

	private void SaveMapAsCancel()
	{
		mFileLocation = "";
	}


	//----------------------------
	// LOAD MAP ROUTINES
	//----------------------------

	public void loadMapAs(SelectFileDialog.Result Result)
	{
		if(Result.result == SelectFileDialog.Result.Enum.Select)
		{
			if(!File.Exists(Result.file))
			{
				mEditor.mUIController.newOkDialog(Dialog.Type.EFileDoesNotExist, "Cannot open file: File does not exist.", null);
				return;
			}

			mFileLocation = Result.file;

			loadMapWorker();
		}
	}

	private void loadMapWorker()
	{
		var br = new BinaryReader(File.Open(mFileLocation, FileMode.Open));
		byte[] header = br.ReadBytes(mMapHeaderv1.Length);

		if(!checkHeader(header))
		{
			Debug.Log("Unsupported map format.");
			mEditor.mUIController.newOkDialog(Dialog.Type.EFileIsNotCorrectFormat, "Cannot open file: Unsupported map format.", null);
		}
		else
		{
			switch(header[mMapVersionLocationInHeader])
			{
			case mMapReaderVersion1:
				{
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
					break;
				}
			default:
				{
					mEditor.mUIController.newOkDialog(Dialog.Type.EFileIsNotCorrectFormat, "Cannot open file: Unsupported map version.", null);
					break;
				}
			}
		}

		br.Close();
	}

	private bool checkHeader(byte[] bytes)
	{
		if(bytes.Length != mMapHeaderv1.Length)
		{
			return false;
		}

		for(int i = 0; i < bytes.Length; ++i)
		{
			if(bytes[i] != mMapHeaderv1[i])
			{
				return false;
			}
		}

		return true;
	}


	//----------------------------
	// VARIOUS UTILITY ROUTINES
	//----------------------------

	private void generateDefaultMap()
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
