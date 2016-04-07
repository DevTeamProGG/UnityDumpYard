using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SpriteAtlasController
{
	private EditorController mEditor;

	private List<SpriteAtlas> mAtlases;

	public SpriteAtlasController(EditorController editor) 
	{
		mEditor = editor;

		mAtlases = new List<SpriteAtlas>();
		loadAtlases();
	}

	private void loadAtlases()
	{
		//TODO: Remove the hard coded string
		mAtlases.Add(new SpriteAtlas("grass"));
	}

	public Sprite getIndexedSprite(String name, int index)
	{
		SpriteAtlas sa = mAtlases.Find(temp => temp.Name == name);
		if(sa == null)
		{
			Debug.Log("Could not find SpriteAtlas with name " + name);
			return null;
		}
		return sa.getIndexedSprite(index);
	}

	public Sprite getRandomizedSprite(String name)
	{
		SpriteAtlas sa = mAtlases.Find(temp => temp.Name == name);
		if(sa == null)
		{
			Debug.Log("Could not find SpriteAtlas with name " + name);
			return null;
		}
		return sa.getRandomSprite();
	}

	public SpriteAtlas getSpriteAtlas(String name)
	{
		return mAtlases.Find(temp => temp.Name == name);
	}
}
