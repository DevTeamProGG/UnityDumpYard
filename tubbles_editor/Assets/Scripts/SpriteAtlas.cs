using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SpriteAtlas
{

	private List<jSprite> mSprites;
	private String name;

	public String Name {
		get {
			return name;
		}
	}

	public SpriteAtlas (String _name) 
	{
		name = _name;
		mSprites = new List<jSprite>();

		Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Tiles/" + name);

		if(sprites.Length == 0)
		{
			Debug.Log("Could not load any sprites from resource folder with name " + name);
		}

		foreach(Sprite s in sprites)
		{
			mSprites.Add(new jSprite(s, name));
		}
	}

	public jSprite getRandomSprite()
	{
		if(mSprites == null || mSprites.Count == 0)
		{
			Debug.Log("mSprites.Count: " + mSprites.Count);
			Debug.LogWarning("getRandomSprite(): Tried to load a sprite when the list was not initialized, or empty..");
			return  null;
		}

		return mSprites[(int)UnityEngine.Random.Range(0,mSprites.Count)];
	}

	public jSprite getIndexedSprite(int index)
	{
		if(mSprites == null || mSprites.Count <= index)
		{
			Debug.Log("mSprites.Count: " + mSprites.Count + ", index: " + index);
			Debug.LogWarning("getRandomSprite(): Tried to load a sprite when the list was not initialized, or not sufficiently large..");
			return  null;
		}

		return mSprites[index];
	}

	public bool spriteBelongsToAtlas(Sprite sprite)
	{
		foreach(jSprite js in mSprites)
		{
			if(js.sprite == sprite) return true;
		}
		return false;
	}

	public bool spriteBelongsToAtlas(String name)
	{
		foreach(jSprite js in mSprites)
		{
			if(js.name == name) return true;
		}
		return false;
	}
}
