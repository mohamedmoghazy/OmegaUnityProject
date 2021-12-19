using System;
using System.Collections;
using System.Collections.Generic;
using Omega.Systems;
using Omega.Types;
using OmegaGame.DataReaders;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragableItem : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Action<PointerEventData> _onObjectUpdated;
    private Action<PointerEventData> _onPointerDown;

    public void Init(Action<PointerEventData> onDragObject, Action<PointerEventData> onPointerDown)
    {
        _onObjectUpdated = onDragObject;
        _onPointerDown = onPointerDown;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _onObjectUpdated?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _onObjectUpdated?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _onPointerDown?.Invoke(eventData);
    }
}
