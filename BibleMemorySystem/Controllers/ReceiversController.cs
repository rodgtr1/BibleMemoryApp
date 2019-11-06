using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BibleMemorySystem.Data;
using BibleMemorySystem.Models;
using BibleMemorySystem.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.TwiML;


namespace BibleMemorySystem.Controllers
{
    public class ReceiversController : TwilioController
    {
        private readonly ApplicationDbContext _context;
        private readonly SignupDetails _signupDetails;

        public ReceiversController(ApplicationDbContext context, IOptions<SignupDetails> signupDetails)
        {
            _context = context;
            _signupDetails = signupDetails.Value ?? throw new ArgumentException(nameof(signupDetails));
        }

        //[HttpPost]
        public TwiMLResult Index(SmsRequest incomingMessage, Receiver receiver)
        {
            
            var messagingResponse = new MessagingResponse();
            string unauthorized = "Sorry you are not authorized to use this service";
            if (incomingMessage.Body.Contains(_signupDetails.Signup))
            {
                var m = incomingMessage.Body.Split(' ');
                var dup = _context.Receiver.FirstOrDefault(b => b.UserPhone == m[2]);

                // We want to update
                if (dup != null)
                {
                    if (m[1] == "add")
                    {
                        dup.Active = true;
                        _context.Update(dup);
                        _context.SaveChanges();
                    }
                    else if (m[1] == "remove")
                    {
                        dup.Active = false;
                        _context.Update(dup);
                        _context.SaveChanges();
                    }
                    else
                    {
                        messagingResponse.Message("You are only allowed to 'add' or 'remove' your number");
                    }
                }
                // We have a new record
                else
                {
                    if (m[2].Length != 11)
                    {
                        messagingResponse.Message("Remember to include the '1'. Must be 11 digits total. Try again.");
                    }
                    else
                    {
                        if (m[1] == "add")
                        {
                            receiver.Active = true;
                            receiver.UserPhone = m[2];
                            _context.Add(receiver);
                            _context.SaveChanges();
                        }
                        else if (m[1] == "remove")
                        {
                            receiver.Active = false;
                            receiver.UserPhone = m[2];
                            _context.Add(receiver);
                            _context.SaveChanges();
                        }
                        else
                        {
                            messagingResponse.Message("You are only allowed to 'add' or 'remove' your number");
                        }
                    }
                }
            }
            else
            {
                messagingResponse.Message(unauthorized);
            }

            return TwiML(messagingResponse);
        }
    }
}