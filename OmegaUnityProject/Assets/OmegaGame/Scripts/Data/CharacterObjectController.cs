using System;
using OmegaGame.DataReaders;
using UnityEngine;

[RequireComponent(typeof(SelectableItem))]
public class CharacterObjectController : GameObjectController, IDisposable
{
    [SerializeField] private PlayerMovement _playerMovement = default;
    [SerializeField] private PlayerShooting _playerShooting = default;

    private GameDataReader _gameDataReader;

    public override void Init(OnSwitchGameModeNotification onSwitchGameModeNotification)
    {
        base.Init(onSwitchGameModeNotification);
        _gameDataReader = DataReaders.Cache.Get<GameDataReader>();
    }

    public void SubscribeToFireButton(LongClickButton button)
    {
        button.onLongClick.AddListener(_playerShooting.Fire);
        button.onLongRelease.AddListener(_playerShooting.DisableEffects);
    }

    protected override void SetCreatorModeActive(bool isCreatorMode)
    {
        base.SetCreatorModeActive(isCreatorMode);

        if (!_gameDataReader.IsActivePlayer(this))
        {
            return;
        }

        _playerMovement.enabled = !isCreatorMode;
        _playerShooting.enabled = !isCreatorMode;

        if (Camera.main is null)
        {
            return;
        }

        CameraFollow cameraFollow = Camera.main.gameObject.GetComponent<CameraFollow>();

        if (cameraFollow is null)
        {
            return;
        }

        if (!isCreatorMode)
        {
            cameraFollow.SetTarget(gameObject.transform);
            cameraFollow.enabled = true;
        }
        else
        {
            cameraFollow.SetTarget(null);
            cameraFollow.enabled = false;
            _playerShooting.DisableEffects();
        }
    }
}