using System.Collections;
using System.Collections.Generic;
using Omega.Types;
using OmegaGame.DataReaders;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Task = System.Threading.Tasks.Task;

public class SlotItemUiDisplay : UiDisplay , IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Image _icon = default;
    [SerializeField] private LayerMask _groundMask = default;
    
    private OnGameObjectAddedNotification _onGameObjectAddedNotification;
    private GameAssetConfig _config;
    private GameObject _placementGameObject;
    private bool _isDraggingStarted;
    
    public override void Init()
    {
    }
    
    public async Task Populate(GameAssetConfig gameAssetConfig)
    {
        _config = gameAssetConfig;
        _onGameObjectAddedNotification = DataReaders.Cache.Get<GameDataReader>()?.OnObjectAddedNotification;
        await Assets.LoadAndAssignSprite(_config.IconAssetReference, _icon);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _placementGameObject = null;
        TaskRunner.Start(LoadAndInstantiateAsset(_config.GameAssetReference));
    }

    public void OnDrag(PointerEventData eventData)
    {
        _isDraggingStarted = true;
        
        RaycastHit hit;

        if (Camera.main is null)
        {
            Debug.LogError("Main camera can't be found");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        bool isGroundHit = Physics.Raycast(ray, out hit, Mathf.Infinity, _groundMask);

        if (_placementGameObject == null)
        {
            return;
        }
        
        if (!isGroundHit)
        {
            _placementGameObject.SetActive(false);
            return;
        }

        _placementGameObject.transform.position = hit.point;
        _placementGameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        RaycastHit hit;

        if (Camera.main is null)
        {
            Debug.LogError("Main camera can't be found");
            return;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundMask) && _isDraggingStarted)
        {
            _onGameObjectAddedNotification?.Invoke(_placementGameObject);
        }
        else
        {
            Destroy(_placementGameObject);
        }

        _isDraggingStarted = false;
    }

    private async Task LoadAndInstantiateAsset(AssetReference assetReference)
    {
        GameObject assetPrefab = await Assets.LoadAssetAsync<GameObject>(assetReference);
        _placementGameObject = Instantiate(assetPrefab);
        _placementGameObject.SetActive(false);
    }
    
    public override void Dispose()
    {
    }
}
