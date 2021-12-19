using System.Collections;
using System.Collections.Generic;
using Omega.Types;
using OmegaGame.DataReaders;
using UnityEngine;
using UnityEngine.UI;

public class CreatorModeUiState : UiState
{
    [SerializeField] private Button _gameModeButton = default;
    [SerializeField] private ItemsListContainerUiDisplay _itemsListContainerUiDisplay = default;
    [SerializeField] private SelectCityUiDisplay _selectCityUiDisplay = default;
    public override void Populate()
    {
        _gameModeButton.onClick.AddListener(OnGameModeClicked);
        _itemsListContainerUiDisplay.Init();
        _selectCityUiDisplay.Init();
    }

    private void OnGameModeClicked()
    {
        DataReaders.Cache.Get<GameDataReader>().OnSwitchGameModeNotification.Invoke(GameMode.PlayMode);
        UiDirector.Instance.SwitchUiState(GameMode.PlayMode);    }

    public override void Dispose()
    {
        _itemsListContainerUiDisplay.Dispose();
        _gameModeButton.onClick.RemoveListener(OnGameModeClicked);
        _selectCityUiDisplay.Dispose();
    }
}
