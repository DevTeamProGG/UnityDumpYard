using System.Collections;

using UnityEngine;

public class Cell : MonoBehaviour
{
	private SpriteRenderer mSpriteRenderer;

	public void setSprite(Sprite s)
	{
		if(s == mSpriteRenderer.sprite) return;
		mSpriteRenderer.sprite = s;
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
}
