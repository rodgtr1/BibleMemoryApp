using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibleMemorySystem.Models
{
    public class Slot
    {
        public int SlotId { get; set; }
        public int Number { get; set; }
        public string SlotName { get; set; }
        public string Frequency { get; set; }
        public string FrequencyDetail { get; set; }
        public string FrequencyDay { get; set; }
        public string FrequencyMonth { get; set; }
        public int PacketId { get; set; }
        public virtual Packet Packet { get; set; }
    }
}
