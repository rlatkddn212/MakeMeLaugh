using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragDropEvent : UIBaseEvent,IDragHandler,IBeginDragHandler,IEndDragHandler
{
	private Canvas m_Canvas = null;

	private bool IsError => m_Canvas == null;

	private Action m_OnBeginDrag = null;

	private Action m_OnDrag = null;

	private Action m_OnEndDrag = null;

	protected override void Awake()
	{
		base.Awake();

		m_Canvas = gameObject.GetComponentInParent<Canvas>();
	}

	public void OnDrag(PointerEventData _data)
	{
		if(IsError)
		{
			return;
		}

		UIRectTransform.anchoredPosition += _data.delta/m_Canvas.scaleFactor;

		m_OnDrag?.Invoke();
	}

	public void OnBeginDrag(PointerEventData _data)
	{
		transform.SetAsLastSibling();

		m_OnBeginDrag?.Invoke();
	}

	public void OnEndDrag(PointerEventData _data)
	{
		m_OnEndDrag?.Invoke();
	}

	public void AddBeginDragEvent(Action _onBeginDrag)
	{
		m_OnBeginDrag -= _onBeginDrag;
		m_OnBeginDrag += _onBeginDrag;
	}
	public void AddDragEvent(Action _onDrag)
	{
		m_OnDrag -= _onDrag;
		m_OnDrag += _onDrag;
	}

	public void AddEndDragEvent(Action _onEndDrag)
	{
		m_OnEndDrag -= _onEndDrag;
		m_OnEndDrag += _onEndDrag;
	}

	public void RemoveBeginDragEvent(Action _onBeginDrag)
	{
		m_OnBeginDrag -= _onBeginDrag;
	}

	public void RemoveDragEvent(Action _onDrag)
	{
		m_OnDrag -= _onDrag;
	}

	public void RemoveEndDragEvent(Action _onEndDrag)
	{
		m_OnEndDrag -= _onEndDrag;
	}
}