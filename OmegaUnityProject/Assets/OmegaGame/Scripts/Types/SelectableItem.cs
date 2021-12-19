using System.Collections;
using System.Collections.Generic;
using Omega.Types;
using OmegaGame.DataReaders;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableItem : MonoBehaviour, IPointerDownHandler
{
    private OnGameObjectSelectedNotification _onGameObjectSelectedNotification;

    public void Init()
    {
        _onGameObjectSelectedNotification = DataReaders.Cache.Get<GameDataReader>()?.OnGameObjectSelectedNotification;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _onGameObjectSelectedNotification?.Invoke(gameObject);
    }
}
