using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Omega.Services;
using OmegaGame.DataReaders;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Omega.Systems
{
    public class Bootstrapper : MonoBehaviour
    { 
        public void Awake()
        { 
            ServiceLocator.Initialize();
            TaskRunner.Start(InitServices()); 
        }
        
        private async Task InitServices()
        {
            await InitializeAssetsLoaderService();
            await InitializeGameService();
            await InitializeWeatherService();
            await InitializeUiService();
        }

        private async Task InitializeUiService()
        {
            UiDirector.Instance.Init();
            await Task.CompletedTask;
        }
        
        private async Task InitializeGameService()
        {
            GameDataContainer gameData = new GameDataContainer();
            GameDataReader gameDataReader = new GameDataReader(gameData);
            DataReaders.Cache.Add(gameDataReader);
            GameDataWriter gameDataWriter = new GameDataWriter(gameData);
            ServiceLocator.Instance.Register(new GameService(gameDataReader, gameDataWriter));
            await Task.CompletedTask;
        }
        
        private async Task InitializeWeatherService()
        {
            var weatherData = new WeatherDataContainer();
            var weatherDataReader = new WeatherDataReader(weatherData);
            DataReaders.Cache.Add(weatherDataReader);
            var weatherDataWriter = new WeatherDataWriter(weatherData);
            ServiceLocator.Instance.Register(new WeatherService(weatherDataReader, weatherDataWriter));
            await Task.CompletedTask;
        }

        private async Task InitializeAssetsLoaderService()
        {
            AssetsDataContainer assetsDataContainer = new AssetsDataContainer();
            AssetsDataReader assetsDataReader = new AssetsDataReader(assetsDataContainer);
            DataReaders.Cache.Add(assetsDataReader);
            AssetsDataWriter assetsDataWriter = new AssetsDataWriter(assetsDataContainer);
            AssetsLoaderService assetsLoaderService = new AssetsLoaderService(assetsDataReader, assetsDataWriter);
            ServiceLocator.Instance.Register(assetsLoaderService);
            await assetsLoaderService.LoadAndAssignAssetsConfigs();
        }
        
        private static void UnregisterServices()
        { 
            ServiceLocator.Instance.Unregister<WeatherService>();
            ServiceLocator.Instance.Unregister<AssetsLoaderService>();
            ServiceLocator.Instance.Unregister<GameService>();
        }

        public void OnDestroy()
        {
            UnregisterServices();
        }
    }
}