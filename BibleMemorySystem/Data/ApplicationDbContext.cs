using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BibleMemorySystem.Models;

namespace BibleMemorySystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BibleMemorySystem.Models.Verse> Verse { get; set; }
        public DbSet<BibleMemorySystem.Models.Packet> Packet { get; set; }
        public DbSet<BibleMemorySystem.Models.Slot> Slot { get; set; }
        public DbSet<BibleMemorySystem.Models.Receiver> Receiver { get; set; }
    }
}
