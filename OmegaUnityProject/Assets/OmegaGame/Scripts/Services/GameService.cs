using System.Collections.Generic;
using Omega.Types;
using UnityEngine;

namespace Omega.Services
{
    public class GameService : IService
    {
        private readonly GameDataReader _gameDataReader;
        private readonly GameDataWriter _gameDataWriter;
        
        public GameService(GameDataReader gameDataReader, GameDataWriter gameDataWriter)
        {
            _gameDataReader = gameDataReader;
            _gameDataWriter = gameDataWriter;
            _gameDataReader.OnObjectAddedNotification.AddListener(OnGameObjectAdded);
            _gameDataReader.OnSwitchGameModeNotification.AddListener(SwitchGameMode);
            _gameDataReader.OnGameObjectSelectedNotification.AddListener(OnSelectGameObject);
            Camera.main.GetComponent<GameObjectController>().Init(_gameDataReader.OnSwitchGameModeNotification);
        }

        private void OnSelectGameObject(GameObject gameObject)
        {
            _gameDataWriter.SetCurrentSelectedGameObject(gameObject);
        }

        private void OnGameObjectAdded(GameObject gameObject)
        {
            GameObjectController gameDataController = gameObject.GetComponent<GameObjectController>();
            gameDataController.Init(_gameDataReader.OnSwitchGameModeNotification);
            _gameDataWriter.AddGameObject(gameDataController);

            if (gameDataController is CharacterObjectController characterObjectController)
            {
                _gameDataWriter.SetCurrentActiveCharacter(characterObjectController);
            }
        }

        private void SwitchGameMode(GameMode mode)
        {
            _gameDataWriter.UpdateGameMode(mode);
        }

        public void Dispose()
        {
            _gameDataReader.OnObjectAddedNotification.RemoveListener(OnGameObjectAdded);
            _gameDataReader.OnSwitchGameModeNotification.RemoveListener(SwitchGameMode);
            _gameDataReader.OnGameObjectSelectedNotification.RemoveListener(OnSelectGameObject);
        }
    }
}