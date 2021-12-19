using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotationHandler : MonoBehaviour, IPointerUpHandler
{
    private Action<PointerEventData> _onPointerUp;
    public void Init(Action<PointerEventData> onPointerUp)
    {
        _onPointerUp = onPointerUp;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        _onPointerUp.Invoke(eventData);
    }
}
