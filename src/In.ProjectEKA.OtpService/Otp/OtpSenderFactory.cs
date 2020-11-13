namespace In.ProjectEKA.OtpService.Otp
{
	using System.Collections.Generic;
	using System.Linq;
    using Common.Logger;
    using System;

	public class OtpSenderFactory
    {
        private readonly IEnumerable<string> whitelistedNumbers;
        private readonly OtpSender otpSender;
        private readonly FakeOtpSender fakeOtpSender;

        public OtpSenderFactory(OtpSender otpSender,
            FakeOtpSender fakeOtpSender,
            IEnumerable<string> whitelistedNumbers)
        {
            this.whitelistedNumbers = whitelistedNumbers ?? new List<string>();
            this.otpSender = otpSender;
            this.fakeOtpSender = fakeOtpSender;
        }

        public IOtpSender ServiceFor(string mobileNumber)
        {
            Console.WriteLine("Checking for mobile number");
            Console.WriteLine(mobileNumber);
            Console.WriteLine("All numbers");
            foreach (string s in whitelistedNumbers)
            {
                Console.WriteLine(s);
            }
            if (mobileNumber != null && whitelistedNumbers.Any(number => number.Contains(mobileNumber)))            
            {
                Console.WriteLine("matched");
                return fakeOtpSender;
            }

            return otpSender;
        }
    }
}
