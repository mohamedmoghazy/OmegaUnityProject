using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable][CreateAssetMenu(menuName = "Omega/GameAssetConfig", fileName = "NewAssetConfig.asset")]
public class GameAssetConfig : ScriptableObject
{
    [SerializeField] private AssetReference _gameAssetReference = default;
    [SerializeField] private AssetReference _iconeAssetReference = default;
    
    public AssetReference GameAssetReference => _gameAssetReference;
    public AssetReference IconAssetReference => _iconeAssetReference;
}