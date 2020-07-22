namespace In.ProjectEKA.OtpService.Otp
{
    public class OtpCreationDetail
    {
        public string SystemName { get; set; }
        public Action Action { get; set; }

        public OtpCreationDetail(string systemName, Action action)
        {
            SystemName = systemName;
            Action = action;
        }
    }
}