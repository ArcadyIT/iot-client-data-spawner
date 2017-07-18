using System;
using IoT.Spawner.Application.Models;

namespace IoT.Spawner.Application
{
    public class DataSpawner
    {
        private static readonly Random Random = new Random();

        public static SensorReading GenerateRandomTemperature()
        {
            var data = Random.Next(-40, 50);

            return new SensorReading
            {
                Type = SensorDataTypeEnum.TemperatureCelsius,
                Value = data.ToString()
            };
        }

        public static SensorReading GenerateRandomHumidity()
        {
            var data = Random.Next(70, 90);

            return new SensorReading
            {
                Type = SensorDataTypeEnum.Humidity,
                Value = data.ToString()
            };
        }

        public static SensorReading GenerateRandomLux()
        {
            var data = Random.Next(0, 100_000);

            return new SensorReading
            {
                Type = SensorDataTypeEnum.Lux,
                Value = data.ToString()
            };
        }

        public static SensorReading GenerateRandomDecibels()
        {
            var data = Random.Next(0, 150);

            return new SensorReading
            {
                Type = SensorDataTypeEnum.Decibel,
                Value = data.ToString()
            };
        }
    }
}
