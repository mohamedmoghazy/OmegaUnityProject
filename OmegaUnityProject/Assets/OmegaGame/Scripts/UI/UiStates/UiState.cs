using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UiState : MonoBehaviour , IDisposable
{
    public virtual void Populate(){}

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    public virtual void Dispose() {}
}
