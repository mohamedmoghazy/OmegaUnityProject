using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Omega.Services;
using Omega.Systems;
using Omega.Types;
using OmegaGame.DataReaders;
using UnityEngine;
using UnityEngine.UI;

public class SelectCityUiDisplay : UiDisplay
{
    [SerializeField] private Dropdown _citiesDropdown = default;

    private WeatherDataReader _weatherDataReader;
    private WeatherService _weatherService;
    private GameObject _weatherEffectGO;
    private GameDataReader _gameDataReader;
    private OnSwitchGameModeNotification _onSwitchGameModeNotification;

    public override void Init()
    {
        _weatherDataReader = DataReaders.Cache.Get<WeatherDataReader>();
        _weatherService = ServiceLocator.Instance.GetService<WeatherService>();
        PopulateCities();
        _gameDataReader = DataReaders.Cache.Get<GameDataReader>();
        _onSwitchGameModeNotification = _gameDataReader.OnSwitchGameModeNotification;
        _onSwitchGameModeNotification.AddListener(SwitchCity);
    }

    private void SwitchCity(GameMode mode)
    {
        if (mode == GameMode.PlayMode)
        {
            var cityName = _citiesDropdown.options[_citiesDropdown.value].text;
            TaskRunner.Start(ChangeCityAsync(_weatherDataReader.GetCityId(cityName), cityName));
        }
        else
        {
            if (_weatherEffectGO != null)
            {
                Destroy(_weatherEffectGO);
            }
        }
    }

    private async Task ChangeCityAsync(int id, string cityName)
    {
        List<Task> tasksToRun = new List<Task>();
        var weatherData = await _weatherService.GetWeatherData(id);

        if (weatherData == null)
        {
            Debug.LogError("weatherData returns null");
            return;
        }
        
        Debug.Log($"weatherData: {weatherData}" );

        var unixTime = weatherData.weather.timestamp;
        var gmtTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime.ToLocalTime();
        var localTimeHour = _weatherDataReader.GetCityLocalTimeHour(cityName, gmtTime);
        
        PartOfDay currentPartOfDay = GetPartOfDay(localTimeHour);
        tasksToRun.Add(SwitchSkyBox(currentPartOfDay));
        var weatherCode = GetWeatherCode(weatherData.weather.summary.title);
        tasksToRun.Add(AssignWeatherEffect(weatherCode));
        
        Parallel.ForEach(tasksToRun, task => TaskRunner.Start(task));
    }

    private async Task AssignWeatherEffect(WeatherCode code)
    {

        if (_weatherEffectGO != null && _weatherEffectGO.name == code.ToString())
        {
            return;
        }
        
        if (code == WeatherCode.Snow || code == WeatherCode.Rain || code == WeatherCode.Thunderstorm || code == WeatherCode.Clouds)
        {
            var assetName = code.ToString();
            var prefab = await Assets.LoadAssetAsync<GameObject>(assetName);
            _weatherEffectGO = Instantiate(prefab);
        }
        else
        {
            if (_weatherEffectGO != null)
            {
                Destroy(_weatherEffectGO);
            }
        }
    }

    private async Task SwitchSkyBox(PartOfDay currentPartOfDay)
    {
        string materialName = currentPartOfDay.ToString();
        Material skyboxMaterial = await Assets.LoadAssetAsync<Material>(materialName);
        if (skyboxMaterial == null)
        {
            return;
        }
        RenderSettings.skybox = skyboxMaterial;
    }

    private void PopulateCities()
    {
        _citiesDropdown.options.Clear();

        foreach (string city in _weatherDataReader.GetCities().Keys)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData
            {
                text = city
            };
            
           _citiesDropdown.options.Add(optionData); 
        }
    }

    private PartOfDay GetPartOfDay(int hours)
    {
        PartOfDay partOfDay;
        if (hours >= 4 && hours < 8)
        {
            partOfDay = PartOfDay.Sunrise;
        }
        else if (hours >= 8 && hours < 14)
        {
            partOfDay = PartOfDay.Morning;
        }
        else if (hours >= 14 && hours < 18)
        {
            partOfDay = PartOfDay.Afternoon;
        }
        else
        {
            partOfDay = PartOfDay.Night;
        }
        
        return partOfDay;
    }

    private WeatherCode GetWeatherCode(string code)
    {
        WeatherCode weatherCode;
        switch (code)
        {
            case "Snow":
                weatherCode = WeatherCode.Snow;
                break;
            case "Rain":
                weatherCode = WeatherCode.Rain;
                break;
            case "Clouds":
                weatherCode = WeatherCode.Clouds;
                break;
            case "Thunderstorm":
                weatherCode = WeatherCode.Thunderstorm;
                break;
            default:
                weatherCode = WeatherCode.Clear;
                break;
        }

        return weatherCode;
    }

    public override void Dispose()
    {
    }
}
public enum PartOfDay
{
    Sunrise,
    Morning,
    Afternoon,
    Evening,
    Night,
    Sunset,
}

public enum WeatherCode
{
    Clear,
    Snow,
    Rain,
    Thunderstorm,
    Clouds,
}
