namespace In.ProjectEKA.OtpService.Notification
{
    public class HIPSMSNotificationContent
    {
        public string ReceiverName { get; }
        public string HospitalName { get; }
        public string DeeplinkUrl { get; }

        public HIPSMSNotificationContent(string receiverName, string hospitalName, string deeplinkUrl)
        {
            ReceiverName = receiverName;
            HospitalName = hospitalName;
            DeeplinkUrl = deeplinkUrl;
        }
    }
}