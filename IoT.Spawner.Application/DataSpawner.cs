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

        private static int GetRandomInt(int start, int end)
        {
            return (Random.Next(start, end));
        }

        public static SensorReading GenerateRandomSensorReadingMessage()
        {
            return new SensorReading
            {
                SensorReadingId = Guid.NewGuid(),
                DeviceId = new Guid(ConfigurationManager.AppSettings["DeviceId"]),
                Temperature = GetRandomInt(-40, 50),
                LightIntensity = GetRandomInt(70, 90),
                SoundLevel = GetRandomInt(0, 120),
                Humidity = GetRandomInt(0, 150)
            };
        }
    }
}
