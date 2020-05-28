namespace In.ProjectEKA.OtpService.Notification
{
    using System.Threading.Tasks;
    using Clients;
    using Common;
    using Newtonsoft.Json.Linq;

    public class NotificationService : INotificationService
    {
        private readonly ISmsClient smsClient;

        public NotificationService(ISmsClient smsClient)
        {
            this.smsClient = smsClient;
        }

        public async Task<Response> SendNotification(Notification notification)
        { 
            switch(notification.Action)
            {
                case Action.ConsentRequestCreated:
                    await smsClient.Send(
                        notification.Communication.Value,
                        GenerateConsentRequestMessage(notification.Content));
                    return new Response(ResponseType.InternalServerError, "");

                case Action.CmidRecovered:
                    await smsClient.Send(
                        notification.Communication.Value,
                        GenerateConsentRequestMessage(notification.Content));
                    return new Response(ResponseType.InternalServerError, "");
                
                default: return null;
            }
        }

        private static string GenerateConsentRequestMessage(JObject notificationContent)
        {
            var content = notificationContent.ToObject<Content>();
            var message =
                $"Hello, {content.Requester} is requesting your consent for accessing health data for {content.HiTypes}. On providing" +
                $" consent, {content.Requester} will get access to all the health data for which you have provided consent. " + 
                $"To view request, please tap on the link: {content.DeepLinkUrl}";
            return message;
        }

        private static string GenerateCmidRecoveryMessage(JObject notificationContent)
        {
            var cMidContent = notificationContent.ToObject<CMidContent>();
            var message =
                $"The consent manager ID associated with you details is {cMidContent.CMid}@ncg." +
                $" To make sure that your account is secure, we request you to reset the password";
            return message;
        }
    }
}