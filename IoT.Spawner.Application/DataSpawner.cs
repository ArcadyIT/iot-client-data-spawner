using System;
using IoT.Spawner.Application.Models;

namespace IoT.Spawner.Application
{
    public class DataSpawner
    {
        private static readonly Random Random = new Random();
        
        private static double GetRandomDouble(double start, double end)
        {
            return (Random.NextDouble() * Math.Abs(end - start)) + start;
        }

        public static SensorReading GenerateRandomSensorReadingMessage()
        {
            return new SensorReading
            {
                Temperature = GetRandomDouble(-40, 50),
                Lumen = GetRandomDouble(70, 89),
                Decibels = GetRandomDouble(0, 100_000),
                Humidity = GetRandomDouble(0, 150),
            };
        }
    }
}
