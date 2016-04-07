using System;
using System.IO;
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

		loadAtlases();
	}

	private void loadAtlases()
	{
		mAtlases = new List<SpriteAtlas>();

		var sa = Directory.GetDirectories(Application.dataPath + "/Resources/Sprites/Tiles");

		foreach(var s in sa)
		{
			String[] splits = s.Split('\\');
			String load = splits[splits.Length - 1];
			Debug.Log("Loading with string " + load);
			mAtlases.Add(new SpriteAtlas(load));
		}

		//TODO: Remove the hard coded string
		// mAtlases.Add(new SpriteAtlas("grass"));
		// mAtlases.Add(new SpriteAtlas("empty"));
	}

	public jSprite getIndexedSprite(String name, int index)
	{
		SpriteAtlas sa = mAtlases.Find(temp => temp.Name == name);
		if(sa == null)
		{
			Debug.Log("Could not find SpriteAtlas with name " + name);
			return null;
		}
		return sa.getIndexedSprite(index);
	}

	public jSprite getRandomizedSprite(String name)
	{
		SpriteAtlas sa = mAtlases.Find(temp => temp.Name == name);
		if(sa == null)
		{
			Debug.Log("Could not find SpriteAtlas with name " + name);
			return null;
		}
		return sa.getRandomSprite();
	}

	public bool spriteBelongsToAtlas(Sprite sprite, String name)
	{
		SpriteAtlas sa = mAtlases.Find(temp => temp.Name == name);
		if(sa == null) return false;
		return sa.spriteBelongsToAtlas(sprite);
	}

	public SpriteAtlas getSpriteAtlas(String name)
	{
		return mAtlases.Find(temp => temp.Name == name);
	}
}
