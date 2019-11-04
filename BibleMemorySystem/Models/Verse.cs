using BibleMemorySystem.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BibleMemorySystem.Models
{
    public class Verse
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Reference { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DefaultValue(1)]
        public int PacketNumber { get; set; }
        [DefaultValue(1)]
        public int SlotNumber { get; set; }
        public string UserId { get; set; }
    }
}
