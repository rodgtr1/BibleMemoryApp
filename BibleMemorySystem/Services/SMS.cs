using System;
using System.Collections.Generic;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace BibleMemorySystem.Services
{
    public class SMS
    {
        public static void SendMessage(List<string> current, List<string> review, string userPhone, string sid, string token, string twilioPhone)
        {
            
            current.Reverse();
            review.Reverse();

            var currentVerses = string.Join(", ", current);
            var reviewVerses = string.Join(", ", review);

            TwilioClient.Init(sid, token);

            var message = MessageResource.Create(
                body: $"Your current verses are [ {currentVerses} ] and your review verses are [ {reviewVerses} ]",
                from: new Twilio.Types.PhoneNumber($"+{twilioPhone}"),
                to: new Twilio.Types.PhoneNumber($"+{userPhone}")
            );

            System.Diagnostics.Debug.WriteLine($"Your current verses are [ {currentVerses} ] and your review verses are [ {reviewVerses} ]");

            //Console.WriteLine(message.Sid);
        }
    }
}
