using System.Collections;
using System.Collections.Generic;
using OmegaGame.DataReaders;
using UnityEngine;
using UnityEngine.UI;

public class MobileControllerUiDisplay : UiDisplay
{
    [SerializeField] private LongClickButton _fireButton = default; 
    public override void Init()
    {
    }
    
    public void Show(GameDataReader gameDataReader)
    {
        gameObject.SetActive(gameDataReader.IsCharacterControllerActive());
        gameDataReader.GetCurrentActiveCharacter()?.SubscribeToFireButton(_fireButton);
    }

    public override void Dispose()
    {
        _fireButton.onLongClick.RemoveAllListeners();
        _fireButton.onLongRelease.RemoveAllListeners();
    }

    public void Hide(GameDataReader gameDataReader)
    {
        _fireButton.onLongClick.RemoveAllListeners();
        _fireButton.onLongRelease.RemoveAllListeners();
    }
}
