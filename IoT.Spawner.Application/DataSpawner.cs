using System;
using System.Configuration;
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
                SensorReadingId = Guid.NewGuid(),
                DeviceId = new Guid(ConfigurationManager.AppSettings["DeviceId"]),
                Temperature = GetRandomDouble(-40, 50),
                LightIntensity = GetRandomDouble(70, 90),
                SoundLevel = GetRandomDouble(0, 120),
                Humidity = GetRandomDouble(0, 150)
            };
        }
    }
}
