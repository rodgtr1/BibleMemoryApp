using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BibleMemorySystem.Data;
using BibleMemorySystem.Models;
using System.Threading;
using BibleMemorySystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BibleMemorySystem.Models.Configuration;
using Microsoft.Extensions.Options;

namespace BibleMemorySystem.Controllers
{
    public class VersesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly TwilioAccountDetails _twilioAccountDetails;
        private readonly TwilioPhoneDetails _twilioPhoneDetails;

        public VersesController(ApplicationDbContext context, IOptions<TwilioAccountDetails> twilioAccountDetails, IOptions<TwilioPhoneDetails> twilioPhoneDetails)
        {
            _context = context;
            _twilioAccountDetails = twilioAccountDetails.Value ?? throw new ArgumentException(nameof(twilioAccountDetails));
            _twilioPhoneDetails = twilioPhoneDetails.Value ?? throw new ArgumentException(nameof(twilioPhoneDetails));
        }

        [Authorize]
        // GET: Verses
        public async Task<IActionResult> Index()
        {
            var verses = _context.Verse;
            return View(await verses.Where(v => v.UserId == GetCurrentUserId()).ToListAsync());
            return View(await _context.Verse.ToListAsync());
        }

        // GET: Verses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var verse = await _context.Verse
                .FirstOrDefaultAsync(m => m.Id == id);
            if (verse == null)
            {
                return NotFound();
            }

            return View(verse);
        }

        [Authorize]
        // GET: Verses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Verses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,Reference,StartDate")] Verse verse)
        {
            if (ModelState.IsValid)
            {
                verse.UserId = GetCurrentUserId();
                _context.Add(verse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(verse);
        }

        // GET: Verses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var verse = await _context.Verse.FindAsync(id);
            if (verse == null)
            {
                return NotFound();
            }
            return View(verse);
        }

        // POST: Verses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,Reference,StartDate")] Verse verse)
        {
            if (id != verse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(verse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VerseExists(verse.Id))
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
            return View(verse);
        }

        // GET: Verses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var verse = await _context.Verse
                .FirstOrDefaultAsync(m => m.Id == id);
            if (verse == null)
            {
                return NotFound();
            }

            return View(verse);
        }

        // POST: Verses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var verse = await _context.Verse.FindAsync(id);
            _context.Verse.Remove(verse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VerseExists(int id)
        {
            return _context.Verse.Any(e => e.Id == id);
        }

        public async Task VerseNotificationCycle()
        {
            DateTime currentDate = DateTime.Today;

            List<string> currentVerses = new List<string>();
            List<string> reviewVerses = new List<string>();

            var currentUserName = User.Identity.Name;
            var userResult = _context.Users.FirstOrDefault(u => u.UserName == currentUserName);
            var numbersResult = _context.Receiver.Select(r => r.UserPhone).ToList();
            var userPhone = userResult.PhoneNumber;

            List<string> otherNumbers = new List<string>();
            foreach(var number in numbersResult)
            {
                otherNumbers.Add(number);
            }
            otherNumbers.Add(userPhone);


            foreach (var verse in _context.Verse)
            {
                if (verse.UserId == GetCurrentUserId())
                {
                    TimeSpan interval = currentDate - verse.StartDate;

                    // we want the slot to start at 1 on the first day
                    var intervalDays = interval.Days + 1;

                    if (intervalDays <= 7 && verse.PacketNumber == 1)
                    {
                        verse.SlotNumber = 1;
                        if (verse.PacketNumber == 1 && verse.SlotNumber == 1)
                        {
                            currentVerses.Add(verse.Reference);
                        }
                    }
                    else if (intervalDays > 7 && intervalDays <= 14)
                    {
                        verse.SlotNumber = 2;
                        if (verse.PacketNumber == 1 && verse.SlotNumber == 2)
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (intervalDays > 14 && intervalDays <= 21)
                    {
                        verse.SlotNumber = 3;
                        if (verse.PacketNumber == 1 && verse.SlotNumber == 3)
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 21 && interval.Days <= 28)
                    {
                        verse.SlotNumber = 4;
                        if ((verse.PacketNumber == 1 && verse.SlotNumber == 4) && (intervalDays == 23 || intervalDays == 25 || intervalDays == 27))
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 28 && interval.Days <= 35)
                    {
                        verse.SlotNumber = 5;
                        if ((verse.PacketNumber == 1 && verse.SlotNumber == 5) && (intervalDays == 30 || intervalDays == 32 || intervalDays == 34))
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 35 && interval.Days <= 42)
                    {
                        verse.SlotNumber = 6;
                        if ((verse.PacketNumber == 1 && verse.SlotNumber == 6) && (intervalDays == 38 || intervalDays == 40))
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 42 && interval.Days <= 49)
                    {
                        verse.SlotNumber = 7;
                        if ((verse.PacketNumber == 1 && verse.SlotNumber == 7) && (intervalDays == 45 || intervalDays == 47))
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 49 && interval.Days <= 56)
                    {
                        verse.SlotNumber = 1;
                        if (verse.PacketNumber == 2 && verse.SlotNumber == 1 && intervalDays == 56)
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 56 && interval.Days <= 63)
                    {
                        verse.SlotNumber = 2;
                        if (verse.PacketNumber == 2 && verse.SlotNumber == 2 && intervalDays == 63)
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 63 && interval.Days <= 70)
                    {
                        verse.SlotNumber = 3;
                        if (verse.PacketNumber == 2 && verse.SlotNumber == 3 && intervalDays == 70)
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 70 && interval.Days <= 77)
                    {
                        verse.SlotNumber = 4;
                        if (verse.PacketNumber == 2 && verse.SlotNumber == 4 && intervalDays == 77)
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 77 && interval.Days <= 105)
                    {
                        verse.SlotNumber = 1;
                        if (verse.PacketNumber == 3 && verse.SlotNumber == 1 && intervalDays == 78)
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 105 && interval.Days <= 133)
                    {
                        verse.SlotNumber = 2;
                        if (verse.PacketNumber == 3 && verse.SlotNumber == 2 && intervalDays == 106)
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 133 && interval.Days <= 161)
                    {
                        verse.SlotNumber = 3;
                        if (verse.PacketNumber == 3 && verse.SlotNumber == 3 && intervalDays == 134)
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 161 && interval.Days <= 189)
                    {
                        verse.SlotNumber = 1;
                        if (verse.PacketNumber == 4 && verse.SlotNumber == 1 && intervalDays == 169)
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 189 && interval.Days <= 217)
                    {
                        verse.SlotNumber = 2;
                        if (verse.PacketNumber == 4 && verse.SlotNumber == 2 && intervalDays == 197)
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 217 && interval.Days <= 245)
                    {
                        verse.SlotNumber = 3;
                        if (verse.PacketNumber == 4 && verse.SlotNumber == 3 && intervalDays == 225)
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 245 && interval.Days <= 273)
                    {
                        verse.SlotNumber = 1;
                        if (verse.PacketNumber == 5 && verse.SlotNumber == 1 && intervalDays == 260)
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 273 && interval.Days <= 301)
                    {
                        verse.SlotNumber = 2;
                        if (verse.PacketNumber == 5 && verse.SlotNumber == 2 && intervalDays == 288)
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 301 && interval.Days <= 329)
                    {
                        verse.SlotNumber = 3;
                        if (verse.PacketNumber == 5 && verse.SlotNumber == 3 && intervalDays == 316)
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 329 && interval.Days <= 357)
                    {
                        verse.SlotNumber = 1;
                        if (verse.PacketNumber == 6 && verse.SlotNumber == 1 && intervalDays == 351)
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 357 && interval.Days <= 385)
                    {
                        verse.SlotNumber = 2;
                        if (verse.PacketNumber == 6 && verse.SlotNumber == 2 && intervalDays == 379)
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 385 && interval.Days <= 413)
                    {
                        verse.SlotNumber = 3;
                        if (verse.PacketNumber == 6 && verse.SlotNumber == 3 && intervalDays == 407)
                        {
                            reviewVerses.Add(verse.Reference);
                        }
                    }
                    else if (interval.Days > 413)
                    {
                        verse.PacketNumber = 99;
                        verse.SlotNumber = 99;
                    }
                }
            }
            _context.SaveChanges();
            BibleMemorySystem.Services.SMS.SendMessage(currentVerses, reviewVerses, otherNumbers, _twilioAccountDetails.AccountSid, _twilioAccountDetails.AuthToken, _twilioPhoneDetails.TwilioPhoneNumber);
        }

        public async Task VerseLifeCycle()
        {
            DateTime currentDate = DateTime.Today;


            Console.WriteLine("Im in the lifecycle task");
            System.Diagnostics.Debug.WriteLine("In that verse lifecycle");

            foreach (var verse in _context.Verse)
            {
                if (verse.UserId == GetCurrentUserId())
                {
                    TimeSpan interval = currentDate - verse.StartDate;

                    if (interval.Days <= 7)
                    {
                        verse.PacketNumber = 1;
                        verse.SlotNumber = 1;
                    }
                    else if (interval.Days > 7 && interval.Days <= 14)
                    {
                        verse.PacketNumber = 1;
                        verse.SlotNumber = 2;
                    }
                    else if (interval.Days > 14 && interval.Days <= 21)
                    {
                        verse.PacketNumber = 1;
                        verse.SlotNumber = 3;
                    }
                    else if (interval.Days > 21 && interval.Days <= 28)
                    {
                        verse.PacketNumber = 1;
                        verse.SlotNumber = 4;
                    }
                    else if (interval.Days > 28 && interval.Days <= 35)
                    {
                        verse.PacketNumber = 1;
                        verse.SlotNumber = 5;
                    }
                    else if (interval.Days > 35 && interval.Days <= 42)
                    {
                        verse.PacketNumber = 1;
                        verse.SlotNumber = 6;
                    }
                    else if (interval.Days > 42 && interval.Days <= 49)
                    {
                        verse.PacketNumber = 1;
                        verse.SlotNumber = 7;
                    }
                    else if (interval.Days > 49 && interval.Days <= 56)
                    {
                        verse.PacketNumber = 2;
                        verse.SlotNumber = 1;
                    }
                    else if (interval.Days > 56 && interval.Days <= 63)
                    {
                        verse.PacketNumber = 2;
                        verse.SlotNumber = 2;
                    }
                    else if (interval.Days > 63 && interval.Days <= 70)
                    {
                        verse.PacketNumber = 2;
                        verse.SlotNumber = 3;
                    }
                    else if (interval.Days > 70 && interval.Days <= 77)
                    {
                        verse.PacketNumber = 2;
                        verse.SlotNumber = 4;
                    }
                    else if (interval.Days > 77 && interval.Days <= 105)
                    {
                        verse.PacketNumber = 3;
                        verse.SlotNumber = 1;
                    }
                    else if (interval.Days > 105 && interval.Days <= 133)
                    {
                        verse.PacketNumber = 3;
                        verse.SlotNumber = 2;
                    }
                    else if (interval.Days > 133 && interval.Days <= 161)
                    {
                        verse.PacketNumber = 3;
                        verse.SlotNumber = 3;
                    }
                    else if (interval.Days > 161 && interval.Days <= 189)
                    {
                        verse.PacketNumber = 4;
                        verse.SlotNumber = 1;
                    }
                    else if (interval.Days > 189 && interval.Days <= 217)
                    {
                        verse.PacketNumber = 4;
                        verse.SlotNumber = 2;
                    }
                    else if (interval.Days > 217 && interval.Days <= 245)
                    {
                        verse.PacketNumber = 4;
                        verse.SlotNumber = 3;
                    }
                    else if (interval.Days > 245 && interval.Days <= 273)
                    {
                        verse.PacketNumber = 5;
                        verse.SlotNumber = 1;
                    }
                    else if (interval.Days > 273 && interval.Days <= 301)
                    {
                        verse.PacketNumber = 5;
                        verse.SlotNumber = 2;
                    }
                    else if (interval.Days > 301 && interval.Days <= 329)
                    {
                        verse.PacketNumber = 5;
                        verse.SlotNumber = 3;
                    }
                    else if (interval.Days > 329 && interval.Days <= 357)
                    {
                        verse.PacketNumber = 6;
                        verse.SlotNumber = 1;
                    }
                    else if (interval.Days > 357 && interval.Days <= 385)
                    {
                        verse.PacketNumber = 6;
                        verse.SlotNumber = 2;
                    }
                    else if (interval.Days > 385 && interval.Days <= 413)
                    {
                        verse.PacketNumber = 6;
                        verse.SlotNumber = 3;
                    }
                    else if (interval.Days > 413)
                    {
                        verse.PacketNumber = 99;
                        verse.SlotNumber = 99;
                    }
                }
            }
            _context.SaveChanges();
        }

        private string GetCurrentUserId()
        {
            ClaimsPrincipal currentUser = User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            return currentUserID;
        }
    }
}
