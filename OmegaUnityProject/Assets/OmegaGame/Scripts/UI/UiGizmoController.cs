using System;
using Omega.Types;
using OmegaGame.DataReaders;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityStandardAssets.Cameras;

public class UiGizmoController : MonoBehaviour, IDisposable
{
    [SerializeField] private RectTransform _gizmoPanelRectTransform = default;
    [SerializeField] private DragableItem dragableItem = default;
    [SerializeField] private LayerMask _groundMask = default;

    private GameObjectController _currentSelectedController;
    private GameDataReader _gameDataReader;
    private OnGameObjectSelectedNotification _onGameObjectSelectedNotification;
    private OnSwitchGameModeNotification _onSwitchGameModeNotification;
    private float _scaleAmount;
    private float _initialScale;
    private RangeAttribute _inputRange = new RangeAttribute(1, 5);

    public void Init()
    {
        _gameDataReader = DataReaders.Cache.Get<GameDataReader>();
        _onGameObjectSelectedNotification = _gameDataReader?.OnGameObjectSelectedNotification;
        _onGameObjectSelectedNotification?.AddListener(OnGameObjectSelected);
        _onSwitchGameModeNotification = _gameDataReader?.OnSwitchGameModeNotification;
        _onSwitchGameModeNotification?.AddListener(OnSwitchGameMode);
        SetActive(false);
        dragableItem.Init(OnDrag, OnPonterDwon, OnPointerUp);
    }

    private void OnPointerUp(PointerEventData obj)
    {
        _currentSelectedController.SetInitialRotation();
    }

    private void OnPonterDwon(PointerEventData obj)
    {
        _scaleAmount = 0;
    }

    private void SetActive(bool value)
    {
        _gizmoPanelRectTransform.gameObject.SetActive(value);
        dragableItem.enabled = value;
    }
    
    private void OnSwitchGameMode(GameMode gameMode)
    {
        if (gameMode == GameMode.PlayMode)
        {
            SetActive(false);
        }
    }

    private void OnDrag(PointerEventData pointerEventData)
    {
        switch (pointerEventData.pointerPressRaycast.gameObject.tag)
        {
            case "MoveGizmo":
                OnMoveHandleDragged();
                break;
            case "ScaleGizmo":
                OnScaleButtonDragged(pointerEventData);
                break;
            case "RotateGizmo":
                OnRotateButtonDragged(pointerEventData);
                break;
        }
        
        Rect mewRect = GetRectFromBoundaryBox();
        _gizmoPanelRectTransform.sizeDelta = mewRect.size;
        _gizmoPanelRectTransform.position = mewRect.position;
    }

    private void OnScaleButtonDragged(PointerEventData pointerEventData)
    {
        _scaleAmount += pointerEventData.delta.y * Time.deltaTime;
        
        Debug.Log(_scaleAmount + "  " );
        _scaleAmount = Mathf.Clamp(_scaleAmount, _inputRange.min, _inputRange.max);

        _currentSelectedController.gameObject.transform.localScale = new Vector3(_scaleAmount, _scaleAmount, _scaleAmount);
    }

    public Rect GetRectFromBoundaryBox()
    {
        Vector3[] points = _currentSelectedController._boundaryBox.mesh.vertices;

        // Convert the point to world space
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = _currentSelectedController._boundaryBox.transform.TransformPoint(points[i]);
        }

        Vector2[] screenPoints = new Vector2[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            screenPoints[i] = Camera.main.WorldToScreenPoint(points[i]);
        }

        Vector2 min = screenPoints[0];
        Vector2 max = screenPoints[0];

        // Find the bottom left and top right coordinates
        foreach (Vector2 v in screenPoints)
        {
            min = Vector2.Min(min, v);
            max = Vector2.Max(max, v);
        }

        return new Rect(min, max - min);
    }

    private void OnRotateButtonDragged(PointerEventData pointerEventData)
    {
        float xDistanceBetweenPressedPoints = (pointerEventData.position - pointerEventData.pressPosition).x;
        _currentSelectedController.UpdateRotation(xDistanceBetweenPressedPoints);
    }

    private void OnMoveHandleDragged()
    {
        RaycastHit hit;

        if (Camera.main is null)
        {
            Debug.LogError("Main camera can't be found");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        bool isGroundHit = Physics.Raycast(ray, out hit, Mathf.Infinity, _groundMask);

        if (!isGroundHit)
        {
            return;
        }

        _currentSelectedController.UpdatePosition(hit.point);
    }
    
    private void OnGameObjectSelected(GameObject obj)
    {
        SetActive(true);

        _currentSelectedController = obj.GetComponent<GameObjectController>();
        
        Rect mewRect = GetRectFromBoundaryBox();
        _gizmoPanelRectTransform.sizeDelta = mewRect.size;
        _gizmoPanelRectTransform.position = mewRect.position;
    }

    public void Dispose()
    {
        _onGameObjectSelectedNotification?.RemoveListener(OnGameObjectSelected);
    }
}
