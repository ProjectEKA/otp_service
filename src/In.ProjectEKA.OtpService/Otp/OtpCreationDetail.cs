namespace In.ProjectEKA.OtpService.Otp
{
    using Common;

    public class OtpCreationDetail
    {
        public string SystemName { get; set; }
        public Action Action { get; set; }

        public OtpCreationDetail(string systemName, string action)
        {
            SystemName = systemName;
            Action = EnumUtil.ParseEnum<Action>(action);
        }
    }
}