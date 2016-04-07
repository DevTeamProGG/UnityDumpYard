using System;
using System.Collections;

using UnityEngine;

public class Cell : MonoBehaviour
{
	private SpriteRenderer mSpriteRenderer;

	private String mName;

	public String SpriteName {
		get {
			return mName;
		}
	}

	public void setSprite(jSprite s)
	{
		mName = s.name;
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
