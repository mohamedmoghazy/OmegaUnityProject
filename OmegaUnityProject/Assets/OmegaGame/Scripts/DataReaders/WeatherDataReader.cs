using System;
using System.Collections.Generic;
using GraphQlClient.Core;
using UnityEngine;

public class WeatherDataReader : IDataReader
{
    private readonly WeatherDataContainer _weatherDataContainer;
    
    public WeatherDataReader(WeatherDataContainer weatherDataContainer)
    {
        _weatherDataContainer = weatherDataContainer;
    }

    public string GetCurrentCity()
    {
       return _weatherDataContainer.CurrentCity;
    }

    public GraphApi GetWeatherGraph()
    {
        return _weatherDataContainer.WeatherGraph;
    }

    public Dictionary<string, int> GetCities()
    {
        return _weatherDataContainer.Cities; 
    }

    public int GetCityId(string cityName)
    {
        if (_weatherDataContainer.Cities.TryGetValue(cityName, out int cityId))
        {
            return cityId;
        }

        Debug.LogError("City was not added to cities dictionary");
        return -1;
    }

    public int GetCityLocalTimeHour(string city, DateTime gmtTime)
    {
        int timeOffset = 0;

        if (_weatherDataContainer.CitiesGmtOffset.TryGetValue(city, out int offset))
        {
            timeOffset = offset;
        }
        
        return  gmtTime.AddHours(timeOffset).Hour;
    }
}
