using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UiDisplay : MonoBehaviour, IDisposable
{
    public abstract void Init();

    public abstract void Dispose();
}
