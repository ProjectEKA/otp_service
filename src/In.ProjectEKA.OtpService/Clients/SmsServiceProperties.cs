using System.Dynamic;

namespace In.ProjectEKA.OtpService.Otp
{
    public class SmsServiceProperties
    {
        public string ClientId { get; }

        public string ClientSecret { get; }
        
        public string TokenApi { get; }
        
        public string SmsApi { get; }

        public SmsServiceProperties(string clientId, string clientSecret, string tokenApi, string smsApi)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            TokenApi = tokenApi;
            SmsApi = smsApi;
        }
    }
}