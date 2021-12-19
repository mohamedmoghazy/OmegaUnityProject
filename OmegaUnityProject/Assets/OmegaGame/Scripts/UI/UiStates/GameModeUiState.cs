using Omega.Types;
using OmegaGame.DataReaders;
using UnityEngine;
using UnityEngine.UI;

public class GameModeUiState : UiState
{
    [SerializeField] private Button _creatorModeButton = default;
    [SerializeField] private MobileControllerUiDisplay _mobileControllerUiDisplay = default;
    
    private GameDataReader _gameDataReader;
    private OnSwitchGameModeNotification _onSwitchGameModeNotification;
    
    public override void Populate()
    {
        _gameDataReader = DataReaders.Cache.Get<GameDataReader>();
        _onSwitchGameModeNotification = _gameDataReader.OnSwitchGameModeNotification;
        _onSwitchGameModeNotification?.AddListener(OnSwitchGameMode);
        _creatorModeButton.onClick.AddListener(OnCreatorModeClicked);
        _mobileControllerUiDisplay.Init();
    }

    private void OnSwitchGameMode(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.CreatorMode:
                _mobileControllerUiDisplay.Hide(_gameDataReader);
                break;
            case GameMode.PlayMode:
                _mobileControllerUiDisplay.Show(_gameDataReader);
                break;
        }
    }

    private void OnCreatorModeClicked()
    {
        DataReaders.Cache.Get<GameDataReader>().OnSwitchGameModeNotification.Invoke(GameMode.CreatorMode);
        UiDirector.Instance.SwitchUiState(GameMode.CreatorMode);
    }

    public override void Dispose()
    {
        _creatorModeButton.onClick.RemoveListener(OnCreatorModeClicked);
        _onSwitchGameModeNotification?.RemoveListener(OnSwitchGameMode);
    }
}
