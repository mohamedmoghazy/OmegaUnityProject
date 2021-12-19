using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Omega.Types;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Omega.Services
{
    public class AssetsLoaderService : IService
    {
        private readonly AssetsDataReader _assetsDataReader;
        private readonly AssetsDataWriter _assetsDataWriter;

        private const string RESOURCE_CONFIG_ASSET_LABEL = "ResourceConfig";
        
        public AssetsLoaderService(AssetsDataReader assetsDataReader, AssetsDataWriter assetsDataWriter)
        {
            _assetsDataReader = assetsDataReader;
            _assetsDataWriter = assetsDataWriter;
        }

        public async Task LoadAndAssignAssetsConfigs()
        {
            List<GameAssetConfig> assetConfigs = new List<GameAssetConfig>();
            
            AsyncOperationHandle<IList<ScriptableObject>> operationHandle = Addressables.LoadAssetsAsync<ScriptableObject>(RESOURCE_CONFIG_ASSET_LABEL, config  =>
            {
                if (config is GameAssetConfig gameAssetConfig)
                {
                    assetConfigs.Add(gameAssetConfig);
                }
            });

            await operationHandle.Task;
            
            if (operationHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to load global config file with address '{RESOURCE_CONFIG_ASSET_LABEL}'");
                return;
            }
            
            _assetsDataWriter.SetAssetsConfigs(assetConfigs);
        }
        
        public void Dispose()
        {
        }
    }
}