namespace In.ProjectEKA.OtpService.Otp
{
	using System;
	using System.Text;

	public class SmsServiceProperties
    {
        public string ClientId { get; }

        public string ClientSecret { get; }
        public string SmsApi { get; }
        public string Signature { get; }
        public string EntityId { get; }
        public string SmsSuffix { get; }
        public int AccessTokenTTL { get; }

        public SmsServiceProperties(string clientId, string clientSecret, string smsApi, string signature, string entityId, int accessTokenTtl, string smsSuffix)
        {
            ClientId = GetDecodedString(clientId);
            ClientSecret = GetDecodedString(clientSecret);
            SmsApi = GetDecodedString(smsApi);
            Signature = GetDecodedString(signature);
            EntityId = GetDecodedString(entityId);
            SmsSuffix = GetDecodedString(smsSuffix);
            AccessTokenTTL = accessTokenTtl;
        }

        public static string GetDecodedString(string value)
        {
            var data = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(data);
        }
    }
}