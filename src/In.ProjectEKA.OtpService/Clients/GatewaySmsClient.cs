using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using In.ProjectEKA.OtpService.Otp;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Optional;

namespace In.ProjectEKA.OtpService.Clients
{
    using System.Threading.Tasks;
    using Common;
    using Common.Logger;
    using Newtonsoft.Json.Linq;

    public class GatewaySmsClient : ISmsClient
    {
        private readonly SmsServiceProperties smsServiceProperties;
        private readonly HttpClient client;

        public GatewaySmsClient(SmsServiceProperties smsServiceProperties)
        {
            this.smsServiceProperties = smsServiceProperties;
            client = new HttpClient();
        }

        public async Task<Response> Send(string phoneNumber, string message)
        {
            
            if (phoneNumber.Contains('-'))
                phoneNumber = phoneNumber.Split('-').Last();

            var token = await GetToken().ConfigureAwait(false);

            var tokenValue = token.ValueOr("");

            if (tokenValue == "")
            {
                return new Response(ResponseType.Success, "Unable to get token");
            }

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,
                    smsServiceProperties.SmsApi);
                if (token != null)
                {
                    request.Headers.Add(HeaderNames.Authorization, tokenValue);
                }

                var requestBody = new
                {
                    msisdn = phoneNumber, message
                };

                string json_data = JsonConvert.SerializeObject(requestBody);

                //Parse the json object
                JObject json_object = JObject.Parse(json_data);

                request.Content = new StringContent(json_object.ToString(), Encoding.UTF8,
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
            }

            return new Response(ResponseType.Success, "Error in sending notification");
        }

        public async Task<Option<string>> GetToken()
        {
            try
            {
                var auth = smsServiceProperties.ClientId + ":" + smsServiceProperties.ClientSecret;

                var nvc = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                };

                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json")); //ACCEPT header

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Basic", Convert.ToBase64String(
                            Encoding.ASCII.GetBytes(
                                auth)));

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,
                    smsServiceProperties.TokenApi);
                request.Content = new FormUrlEncodedContent(nvc);

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