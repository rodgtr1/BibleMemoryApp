using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibleMemorySystem.Models
{
    public class Packet
    {
        public int PacketId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Slot> Slots { get; set; }
    }
}
