namespace In.ProjectEKA.OtpService.Common
{
	using System;

	public class EnumUtil
    {
        public static T ParseEnum<T>(string value)
        {
            return (T) Enum.Parse(typeof(T), value, true);
        }
    }
}