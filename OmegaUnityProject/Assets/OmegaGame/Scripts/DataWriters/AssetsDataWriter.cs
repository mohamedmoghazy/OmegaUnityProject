using System.Collections.Generic;

public class AssetsDataWriter
{
    private AssetsDataContainer _assetsDataContainer; 
    public AssetsDataWriter(AssetsDataContainer assetsDataContainer)
    {
        _assetsDataContainer = assetsDataContainer;
    }

    public void SetAssetsConfigs(List<GameAssetConfig> assetConfigs)
    {
        _assetsDataContainer.AssetsConfigs = assetConfigs;
    }
}
