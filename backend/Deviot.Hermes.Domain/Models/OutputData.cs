using System;

namespace Deviot.Hermes.Domain.Models
{
    public class OutputData
    {
        public DateTime DateTime { get; private set; }

        public string Path { get; private set; }

        public object Value { get; private set; }

        public bool Quality { get; private set; }
    }
}
