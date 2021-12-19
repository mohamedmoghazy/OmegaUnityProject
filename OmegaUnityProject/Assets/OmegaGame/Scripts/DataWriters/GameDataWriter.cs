using System.Collections;
using System.Collections.Generic;
using Omega.Types;
using UnityEngine;

public class GameDataWriter
{
    private GameDataContainer _gameDataContainer;
    
    public GameDataWriter(GameDataContainer gameDataContainer)
    {
        _gameDataContainer = gameDataContainer;
    }

    public void SetCurrentSelectedGameObject(GameObject gameObject)
    {
        _gameDataContainer.CurrentSelectedGameObject = gameObject;
    }

    public void AddGameObject(GameObjectController gameObjectController)
    {
        _gameDataContainer.AllGameObjectsControllers.Add(gameObjectController);

        if (gameObjectController is CharacterObjectController characterObjectController)
        {
            _gameDataContainer.CharacterObjectControllers.Add(characterObjectController);
        }
    }

    public void UpdateGameMode(GameMode gameMode)
    {
        _gameDataContainer.CurrentGameMode = gameMode;
    }

    public void SetCurrentActiveCharacter(CharacterObjectController characterObjectController)
    {
        _gameDataContainer.CurrentActivePlayer = characterObjectController;
    }
}