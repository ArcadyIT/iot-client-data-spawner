using System;

namespace IoT.Spawner.Application.Models
{
    public class SensorReading
    {
        public Guid SensorReadingId { get; set; }

        public Guid DeviceId { get; set; }

        public int Temperature { get; set; }

        public int LightIntensity { get; set; }

        public int SoundLevel { get; set; }

        public int Humidity { get; set; }

    }
}
