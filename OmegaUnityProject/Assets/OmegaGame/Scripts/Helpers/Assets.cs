using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public static class Assets
{
    public static async Task<T> LoadAssetAsync<T>(object key) where T : Object
    {
        AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(key);
        await UniTask.WaitUntil(() => handle.IsDone);

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"Failed to load global config file with address '{key}'");
        }

        return handle.Status == AsyncOperationStatus.Succeeded ? handle.Convert<T>().Result : null;
    }
    
    public static async Task LoadAndAssignSprite(AssetReference iconReference, Image image)
    {
        Sprite sprite = await LoadAssetAsync<Sprite>(iconReference);

        if (sprite)
        {
            try
            {
                image.sprite = sprite;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        else
        {
            Debug.LogError($"Failed to load icon {iconReference.RuntimeKey}");
        }
    }

}
