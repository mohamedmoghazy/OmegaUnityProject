using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using OmegaGame.DataReaders;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class ItemsListContainerUiDisplay : UiDisplay
{
    [SerializeField] private Animator _animator = default;
    [SerializeField] private Transform _content = default;
    [SerializeField] private SlotItemUiDisplay _slotItemUiDisplayPrefab = default;

    private List<SlotItemUiDisplay> _itemUiDisplays = new List<SlotItemUiDisplay>();
    private AssetsDataReader _assetsDataReader;
    private static readonly int Show = Animator.StringToHash("Show");

    public override void Init()
    {
        _assetsDataReader = DataReaders.Cache.Get<AssetsDataReader>();
        TaskRunner.Start(DrawAllGameAssetsAsync(_assetsDataReader.GetGameAssetConfigs()));
    }

    private async Task DrawAllGameAssetsAsync(List<GameAssetConfig> configs)
    {
        List<Task> tasksToComplete = new List<Task>();
        
        foreach (GameAssetConfig config in configs)
        {
            SlotItemUiDisplay slotItemUiDisplay = Instantiate(_slotItemUiDisplayPrefab, _content);
            _itemUiDisplays.Add(slotItemUiDisplay);
            tasksToComplete.Add(slotItemUiDisplay.Populate(config));
        }

        Parallel.ForEach(tasksToComplete, task => TaskRunner.Start(task));
        await UniTask.WaitUntil(() => Task.WhenAll(tasksToComplete).IsCompleted);
        _animator.keepAnimatorControllerStateOnDisable = true;
        _animator.SetTrigger(Show);
    }

    public override void Dispose()
    {
        if (_itemUiDisplays != null)
        {
            foreach (var display in _itemUiDisplays)
            {
                display.Dispose();
            }
        }
    }
}
