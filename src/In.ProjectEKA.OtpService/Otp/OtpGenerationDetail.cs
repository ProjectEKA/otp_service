namespace In.ProjectEKA.OtpService.Otp
{
	using Common;

	public class OtpGenerationDetail
    {
        public string SystemName { get; set; }
        public Action Action { get; set; }

        public OtpGenerationDetail(string systemName, string action)
        {
            SystemName = systemName;
            Action = EnumUtil.ParseEnum<Action>(action);
        }
    }
}