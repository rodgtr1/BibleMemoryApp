using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BibleMemorySystem.Data;
using BibleMemorySystem.Models;

namespace BibleMemorySystem.Controllers
{
    public class PacketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PacketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Packets
        public async Task<IActionResult> Index()
        {
            return View(await _context.Packet.ToListAsync());
        }

        // GET: Packets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packet = await _context.Packet
                .FirstOrDefaultAsync(m => m.PacketId == id);
            if (packet == null)
            {
                return NotFound();
            }

            return View(packet);
        }

        // GET: Packets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Packets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PacketId,Name")] Packet packet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(packet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(packet);
        }

        // GET: Packets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packet = await _context.Packet.FindAsync(id);
            if (packet == null)
            {
                return NotFound();
            }
            return View(packet);
        }

        // POST: Packets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PacketId,Name")] Packet packet)
        {
            if (id != packet.PacketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(packet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacketExists(packet.PacketId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(packet);
        }

        // GET: Packets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var packet = await _context.Packet
                .FirstOrDefaultAsync(m => m.PacketId == id);
            if (packet == null)
            {
                return NotFound();
            }

            return View(packet);
        }

        // POST: Packets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var packet = await _context.Packet.FindAsync(id);
            _context.Packet.Remove(packet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacketExists(int id)
        {
            return _context.Packet.Any(e => e.PacketId == id);
        }
    }
}
