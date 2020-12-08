namespace In.ProjectEKA.OtpService.Otp
{
	using System;
	using System.Text;

	public class SmsServiceProperties
    {
        public string ClientId { get; }

        public string ClientSecret { get; }
        
        public string TokenApi { get; }
        
        public string SmsApi { get; }
        
        public int AccessTokenTTL { get; }

        public SmsServiceProperties(string clientId, string clientSecret, string tokenApi, string smsApi, int accessTokenTtl)
        {
            ClientId = GetDecodedString(clientId);
            ClientSecret = GetDecodedString(clientSecret);
            TokenApi = GetDecodedString(tokenApi);
            SmsApi = GetDecodedString(smsApi);
            AccessTokenTTL = accessTokenTtl;
        }

        public static string GetDecodedString(string value)
        {
            var data = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(data);
        }
    }
}