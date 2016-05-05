using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PropagateDragging : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{
	public ScrollRect mainScrollRect;

	public void OnBeginDrag(PointerEventData eventData)
	{
		mainScrollRect.OnBeginDrag(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		mainScrollRect.OnDrag(eventData);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		mainScrollRect.OnEndDrag(eventData);
	}

	public void OnScroll(PointerEventData eventData)
	{
		mainScrollRect.OnScroll(eventData);
	}
}
