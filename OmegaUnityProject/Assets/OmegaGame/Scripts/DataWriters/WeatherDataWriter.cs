using GraphQlClient.Core;

public class WeatherDataWriter
{
    private WeatherDataContainer _weatherDataContainer;
    
    public WeatherDataWriter(WeatherDataContainer weatherDataContainer)
    {
        _weatherDataContainer = weatherDataContainer;
    }

    public void SetCurrentCity(string city)
    {
        _weatherDataContainer.CurrentCity = city;
    }
    
    public void SetCurrentWeatherGraphView(GraphApi weatherGraph)
    {
        _weatherDataContainer.WeatherGraph = weatherGraph;
    }
}