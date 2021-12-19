using System.Collections;
using System.Collections.Generic;
using Omega.Types;
using UnityEngine;

public class GameDataReader : IDataReader
{
    private readonly GameDataContainer _gameDataContainer;
    public OnGameObjectAddedNotification OnObjectAddedNotification { get; } = new OnGameObjectAddedNotification();
    public OnSwitchGameModeNotification OnSwitchGameModeNotification { get; } = new OnSwitchGameModeNotification();
    public OnGameObjectUpdatedNotification OnGameObjectUpdatedNotification { get; } = new OnGameObjectUpdatedNotification();
    public OnGameObjectSelectedNotification OnGameObjectSelectedNotification { get; } = new OnGameObjectSelectedNotification();
    
    public GameDataReader(GameDataContainer gameDataContainer)
    {
        _gameDataContainer = gameDataContainer;
    }
    
    public List<GameObjectController> GetAllObjects()
    {
        return _gameDataContainer.AllGameObjectsControllers;
    }
    
    public List<CharacterObjectController> GetCharacterObjectControllers()
    {
        return _gameDataContainer.CharacterObjectControllers;
    }

    public bool IsCharacterControllerActive()
    {
        return _gameDataContainer.CurrentActivePlayer != null;
    }

    public CharacterObjectController GetCurrentActiveCharacter()
    {
        return _gameDataContainer.CurrentActivePlayer;
    }

    public bool IsActivePlayer(CharacterObjectController characterObjectController)
    {
        return _gameDataContainer.CurrentActivePlayer == characterObjectController;
    }

    public GameMode GetCurrentGameMode()
    {
        return _gameDataContainer.CurrentGameMode;
    }
}
