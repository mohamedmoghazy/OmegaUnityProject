using System.Linq;
using System.Threading.Tasks;
using GraphQlClient.Core;
using Newtonsoft.Json;
using UnityEngine;

namespace Omega.Services
{
    public class WeatherService : IService
    {
        private readonly WeatherDataReader _weatherDataReader;
        private readonly WeatherDataWriter _weatherDataWriter;
        public WeatherService(WeatherDataReader reader, WeatherDataWriter writer)
        {
            _weatherDataReader = reader;
            _weatherDataWriter = writer;
            TaskRunner.Start(LoadGraphApiAsset());
        }

        public async Task LoadGraphApiAsset()
        {
            GraphApi graphApi = await Assets.LoadAssetAsync<GraphApi>("Weather");
            _weatherDataWriter.SetCurrentWeatherGraphView(graphApi);
        }

        public async Task<GetCityById> GetWeatherData(int cityId)
        {
            var weatherGraph = _weatherDataReader.GetWeatherGraph();
            var query = weatherGraph.GetQueryByName("GetWeatherData", GraphApi.Query.Type.Query);
            query.SetArgs(new {id = cityId.ToString()});
            var request = await weatherGraph.Post(query);
            while(!request.isDone)
            {
                Task.Yield();
            }

            if (request.error?.Length > 0)
            {
                Debug.LogError(request.error.FirstOrDefault().ToString());
                return null;
            }
            
            WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(request.downloadHandler.text);

            if (weatherData is null)
            {
                Debug.LogWarning("request failed to return data");
                return null;
            }
            
            return weatherData?.data?.getCityById?.FirstOrDefault();
        }
        
        public void Dispose()
        {
        }
    }
}