using System;

namespace In.ProjectEKA.OtpService.Notification
{
    public class HIPSMSNotificationContent
    {
        public string ReceiverName { get; }
        public string HospitalName { get; }
        public string DeeplinkUrl { get; }
        public string CareContextInfo { get; }

        public HIPSMSNotificationContent(string receiverName, string hospitalName, string deeplinkUrl, string careContextInfo)
        {
            ReceiverName = receiverName;
            HospitalName = hospitalName;
            DeeplinkUrl = deeplinkUrl;
            CareContextInfo = careContextInfo;
        }
    }
}