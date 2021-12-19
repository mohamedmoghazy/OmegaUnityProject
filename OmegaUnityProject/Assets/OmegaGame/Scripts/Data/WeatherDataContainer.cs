using System.Collections.Generic;
using GraphQlClient.Core;

public class WeatherDataContainer
{
        public GraphApi WeatherGraph { get; set; }
        public string CurrentCity { get; set; }

        public Dictionary<string, int> Cities { get; set; } = new Dictionary<string, int>
        {
                {"London" , 2643743},
                {"Berlin" , 2950159}, 
                {"Moscow" , 524901},
                {"Sydney" , 2147714}, 
                {"NewYork" , 5128581},
                {"Hongkong" , 1819729}
        };

        public Dictionary<string, int> CitiesGmtOffset { get; } = new Dictionary<string, int>
        {
                {"London" , 0},
                {"Berlin" , 1}, 
                {"Moscow" , 3},
                {"Sydney" , 11}, 
                {"NewYork" , -5},
                {"Hongkong" , 8}
        };
}
