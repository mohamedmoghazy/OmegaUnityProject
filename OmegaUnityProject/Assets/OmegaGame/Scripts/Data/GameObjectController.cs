using System;
using System.Collections;
using System.Collections.Generic;
using Omega.Types;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SelectableItem))]
public class GameObjectController : MonoBehaviour, IDisposable
{
    [SerializeField] public MeshFilter _boundaryBox = default;
    
    private SelectableItem _selectableItem;
    private OnSwitchGameModeNotification _onSwitchGameModeNotification;
    private GameObjectData _cashedGameObjectData;
    public virtual void Init(OnSwitchGameModeNotification onSwitchGameModeNotification)
    {
        _selectableItem = GetComponent<SelectableItem>();
        
        Transform objectTransform = gameObject.transform;
        
        Vector3 initialPosition = new Vector3
        {
            x = objectTransform.position.x,
            y = objectTransform.position.y,
            z = objectTransform.position.z
        };
        
        Quaternion initialRotation = new Quaternion
        {
            x = objectTransform.rotation.x,
            y = objectTransform.rotation.y,
            z = objectTransform.rotation.z,
            w = objectTransform.rotation.w,
            eulerAngles = objectTransform.rotation.eulerAngles
        };
            
        _cashedGameObjectData = new GameObjectData
        {
            InitialPosition = initialPosition,
            InitialRotation = initialRotation
        };
        
        _onSwitchGameModeNotification = onSwitchGameModeNotification;
        _onSwitchGameModeNotification?.AddListener(OnSwitchGameMode);
        _selectableItem.Init();
    }


    public void UpdatePosition(Vector3 position)
    {
        gameObject.transform.position = new Vector3
        {
            x = position.x,
            y = position.y,
            z = position.z
        };

        _cashedGameObjectData.InitialPosition = new Vector3
        {
            x = position.x,
            y = position.y,
            z = position.z
        };
    }

    public void UpdateScale(float yDistance)
    {
        Vector3 newScale = new Vector3(yDistance, yDistance, yDistance);
        
        if (newScale.x <= 1)
        {
            return;
        }
        
        gameObject.transform.localScale = newScale;
    }
    
    public void UpdateRotation(float xDistance)
    {
        var newQuatenion = Quaternion.Euler(Vector3.down * xDistance );
           var current = newQuatenion * _cashedGameObjectData.InitialRotation;

           transform.rotation = current;
    }

    private void OnSwitchGameMode(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.CreatorMode:
                SetCreatorModeActive(true);
                Reset();
                break;
            case GameMode.PlayMode:
                SetCreatorModeActive(false);
                break;
        }
    }

    protected virtual void SetCreatorModeActive(bool isCreatoerMode)
    {
        _selectableItem.enabled = isCreatoerMode;
    }

    public void Reset()
    {
        gameObject.transform.position = new Vector3
        {
          x = _cashedGameObjectData.InitialPosition.x,
          y = _cashedGameObjectData.InitialPosition.y,
          z = _cashedGameObjectData.InitialPosition.z
        };
        
        gameObject.transform.rotation = new Quaternion
        {
            x = _cashedGameObjectData.InitialRotation.x,
            y = _cashedGameObjectData.InitialRotation.y,
            z = _cashedGameObjectData.InitialRotation.z,
            w = _cashedGameObjectData.InitialRotation.w,
            eulerAngles = _cashedGameObjectData.InitialRotation.eulerAngles
        };
    }


    public virtual void Dispose()
    {
        _onSwitchGameModeNotification?.RemoveListener(OnSwitchGameMode);
    }

    public void SetInitialRotation()
    {
        var rotation = gameObject.transform.rotation;
        
        _cashedGameObjectData.InitialRotation = new Quaternion
        {
            x = rotation.x,
            y = rotation.y,
            z = rotation.z,
            w = rotation.w,
            eulerAngles = rotation.eulerAngles
        };
    }
}
