using System;

namespace IoT.Spawner.Application.Models
{
    public class SensorReading
    {
        public Guid SensorReadingId { get; set; }

        public Guid DeviceId { get; set; }

        public double Temperature { get; set; }

        public double LightIntensity { get; set; }

        public double SoundLevel { get; set; }

        public double Humidity { get; set; }

    }
}
