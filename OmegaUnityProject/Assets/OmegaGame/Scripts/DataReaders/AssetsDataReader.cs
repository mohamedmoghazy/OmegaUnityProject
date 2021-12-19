using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetsDataReader : IDataReader
{
    private AssetsDataContainer _assetsDataContainer;

    public AssetsDataReader(AssetsDataContainer assetsDataContainer)
    {
        _assetsDataContainer = assetsDataContainer;
    }

    public List<GameAssetConfig> GetGameAssetConfigs()
    {
        return _assetsDataContainer.AssetsConfigs;
    }
}
