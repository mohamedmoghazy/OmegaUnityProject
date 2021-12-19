using System;
using System.Collections;
using System.Collections.Generic;
using Omega.Types;
using UnityEngine;
using Random = UnityEngine.Random;

public class UiDirector : MonoBehaviour
{
    [SerializeField] private UiState _creatorModeUiState = default;
    [SerializeField] private UiState _gameModeUiState = default;
    [SerializeField] private UiGizmoController _uiGizmoController = default;
    
    public static UiDirector Instance = default;
    
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Init()
    {
        InitUiStates();
        _uiGizmoController.Init();
    }

    public void SwitchUiState(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.CreatorMode:
                _creatorModeUiState.Show();
                _gameModeUiState.Hide();
                break;
            case GameMode.PlayMode:
                _creatorModeUiState.Hide();
                _gameModeUiState.Show();
                break;
        }
    }

    private void InitUiStates()
    {
        _creatorModeUiState.Populate();
        _gameModeUiState.Populate();
    }
}