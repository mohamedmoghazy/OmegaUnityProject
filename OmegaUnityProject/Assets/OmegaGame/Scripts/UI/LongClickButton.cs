using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pointerDown;
    private float pointerDownTimer;

    public UnityEvent onLongClick;
    public UnityEvent onLongRelease;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Reset();
        onLongRelease.Invoke();
    }

    private void Update()
    {
        if (pointerDown)
        {
            if (onLongClick != null)
            {
                onLongClick.Invoke();
            }
        }
    }

    private void Reset()
    {
        pointerDown = false;
    }

}