using System.Collections.Generic;
using System.Linq;

namespace In.ProjectEKA.OtpService.Notification
{
    public class NotificationProperties
    {
       public string PatientIdName { get; }
       private IEnumerable<string> WhiteListedNumbers { get; }
        
        public NotificationProperties(string patientIdName, IEnumerable<string> whiteListedNumbers)
        {
            PatientIdName = patientIdName;
            WhiteListedNumbers = whiteListedNumbers;
        }
        
        public bool isWhiteListed(string mobileNumber)
        {
            return mobileNumber != null && WhiteListedNumbers.Any(number => number.Contains(mobileNumber));
        }
    }
}