namespace In.ProjectEKA.OtpService.Clients
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Net.Mime;
	using System.Text;
	using System.Threading.Tasks;
    using System.Runtime.Caching;
	using Common;
	using Common.Logger;
	using Microsoft.Net.Http.Headers;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using Optional;
	using Otp;

	public class GatewaySmsClient : ISmsClient
    {
        private readonly SmsServiceProperties smsServiceProperties;
        private readonly HttpClient client;
        private MemoryCache cache;

        public GatewaySmsClient(SmsServiceProperties smsServiceProperties)
        {
            this.smsServiceProperties = smsServiceProperties;
            client = new HttpClient();
            cache = MemoryCache.Default;
        }

        public async Task<Response> Send(string phoneNumber, string message)
        {
            
            if (phoneNumber.Contains('-'))
                phoneNumber = phoneNumber.Split('-').Last();

            var accessToken = "accessToken";
            var tokenValue = await getAccessToken(accessToken);            

            if (tokenValue == "")
            {
                return new Response(ResponseType.InternalServerError, "Unable to get token");
            }

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post,
                    smsServiceProperties.SmsApi);
                request.Headers.Add(HeaderNames.Authorization, tokenValue);
                var requestBody = new
                {
                    msisdn = phoneNumber, message
                };

                var jsonData = JsonConvert.SerializeObject(requestBody);

                //Parse the json object
                var jsonObject = JObject.Parse(jsonData);

                request.Content = new StringContent(jsonObject.ToString(), Encoding.UTF8,
                    MediaTypeNames.Application.Json);
                var response = await client
                    .SendAsync(request)
                    .ConfigureAwait(false);
                if (response.StatusCode == (HttpStatusCode) 200)
                    return new Response(ResponseType.Success, "Notification sent");
                Log.Error(response.StatusCode,response.Content);
            }
            catch (Exception exception)
            {
                Log.Error(exception, exception.StackTrace);
                return new Response(ResponseType.InternalServerError, "Unable to create otp message.");
            }

            return new Response(ResponseType.Success, "Error in sending notification");
        }

        private async Task<string> getAccessToken(string accessToken)
        {
            if (cache.Contains(accessToken))
            {
                return cache.Get(accessToken) as string;
            }

            var token = await GetToken().ConfigureAwait(false);
            var tokenValue = token.ValueOr("");
            if (tokenValue != "")
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(smsServiceProperties.AccessTokenTTL);
                cache.Set(accessToken, tokenValue, policy);
            }

            return tokenValue;
        }

        public async Task<Option<string>> GetToken()
        {
            try
            {
                var auth = smsServiceProperties.ClientId + ":" + smsServiceProperties.ClientSecret;

                var requestContent = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                };

                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Basic", Convert.ToBase64String(
                            Encoding.ASCII.GetBytes(
                                auth)));

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,
                    smsServiceProperties.TokenApi);
                request.Content = new FormUrlEncodedContent(requestContent);

                var responseMessage = await client.SendAsync(request)
                    .ConfigureAwait(false);
                var response = await responseMessage.Content.ReadAsStringAsync();

                var definition = new {access_token = "", token_type = ""};
                var result = JsonConvert
                    .DeserializeAnonymousType(response, definition);
                var token = Option.Some($"Bearer {result.access_token}");
                return token;
            }
            catch (Exception exception)
            {
                Log.Error(exception, exception.StackTrace);
                return Option.None<string>();
            }
        }
    }
}