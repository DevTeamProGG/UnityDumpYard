using System;
using System.Collections;

using UnityEngine;

public class Cell : MonoBehaviour
{
	private SpriteRenderer mSpriteRenderer;

	private String mName;
	private int mIndex;

	public String SpriteName {
		get {
			return mName;
		}
	}

	public int Index {
		get {
			return mIndex;
		}
	}

	public void setSprite(jSprite s)
	{
		mName = s.name;
		mIndex = s.index;
		mSpriteRenderer.sprite = s.sprite;
	}

	public Sprite getSprite()
	{
		return mSpriteRenderer.sprite;
	}

	public void setSpriteRenderer(SpriteRenderer sr)
	{
		mSpriteRenderer	= sr;
		mSpriteRenderer.enabled = true;
	}

	public void setVisible(bool visible)
	{
		if(mSpriteRenderer.enabled != visible) mSpriteRenderer.enabled = visible;
	}

	public bool getVisible()
	{
		return mSpriteRenderer.enabled;
	}
}
