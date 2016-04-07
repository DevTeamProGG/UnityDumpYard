using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SpriteAtlas
{

	private List<Sprite> mSprites;
	private String name;

	public String Name {
		get {
			return name;
		}
	}

	public SpriteAtlas (String _name) 
	{
		this.name = _name;
		mSprites = new List<Sprite>();

		Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/" + name);

		if(sprites.Length == 0)
		{
			Debug.Log("Could not load any sprites from resource folder with name " + name);
		}

		foreach(Sprite s in sprites)
		{
			mSprites.Add(s);
		}
	}

	public Sprite getRandomSprite()
	{
		if(mSprites == null || mSprites.Count == 0)
		{
			Debug.Log("mSprites.Count: " + mSprites.Count);
			Debug.LogWarning("getRandomSprite(): Tried to load a sprite when the list was not initialized, or empty..");
			return  null;
		}

		return mSprites[(int)UnityEngine.Random.Range(0,mSprites.Count)];
	}

	public Sprite getIndexedSprite(int index)
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
		foreach(Sprite s in mSprites)
		{
			if(s == sprite) return true;
		}
		return false;
	}
}
