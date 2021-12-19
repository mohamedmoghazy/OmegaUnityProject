using System;

[Serializable]
public class Weather
{
    public Summary summary { get; set; }
    public long timestamp { get; set; }
}