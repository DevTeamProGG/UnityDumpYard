using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollRectMouseScroll : MonoBehaviour, IScrollHandler
{
	RectTransform mRect;
	ScrollRect mScrollRect;
	RectTransform mContent;

	[SerializeField]
	float mScrollSpeed = 40;

	float mMinScroll = 0;
	float mMaxScroll;

	// Use this for initialization
	void Start () 
	{
		mRect = GetComponent<RectTransform>();
		mScrollRect = GetComponent<ScrollRect>();

		mContent = mScrollRect.content;

		mMaxScroll = mContent.rect.height - mRect.rect.height;
	}

	public void OnScroll(PointerEventData eventData)
	{
		Vector2 ScrollDelta = eventData.scrollDelta;

		mContent.anchoredPosition += new Vector2(0, -ScrollDelta.y * mScrollSpeed);

		if(mContent.anchoredPosition.y < mMinScroll)
		{
			mContent.anchoredPosition = new Vector2(0, mMinScroll);
		}

		if(mContent.anchoredPosition.y > mMaxScroll)
		{
			mContent.anchoredPosition = new Vector2(0, mMaxScroll);
		}
	}
}
