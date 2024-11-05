//using FgssrApi.Models;
//using Microsoft.Extensions.Options;
//using Twilio;
//using Twilio.Rest.Api.V2010.Account;
//using Twilio.Types;

//namespace FgssrApi.CustomMethodsServices
//{
//    public class SMSSystem : ISMSSystem
//    {

//        private readonly TwilioSetting _twilio;

//        public SMSSystem(IOptions<TwilioSetting> twilio)
//        {
//            _twilio = twilio.Value;
//        }

//        public async Task<bool> SendSms(string receiverPhoneNumber, string code)
//        {

//            TwilioClient.Init(_twilio.AccountSID, _twilio.AuthToken);

//            var sms = await MessageResource.CreateAsync(
//                    body: $"Your Phone number Verification code is: {code} pls dont share this with anyone",
//                    from: new PhoneNumber(_twilio.TwilioPhoneNumber),
//                    to: new PhoneNumber($"+2{receiverPhoneNumber}")
//                );

//            return true;
//        }
//    }
//}
