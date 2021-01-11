using System.Threading.Tasks;
using In.ProjectEKA.OtpService.Clients;
using In.ProjectEKA.OtpService.Common;
using Newtonsoft.Json.Linq;

namespace In.ProjectEKA.OtpService.Notification
{
	public class NotificationService : INotificationService
	{
		private readonly ISmsClient smsClient;
		private readonly NotificationProperties notificationProperties;

		public NotificationService(ISmsClient smsClient, NotificationProperties notificationProperties)
		{
			this.smsClient = smsClient;
			this.notificationProperties = notificationProperties;
		}

		public async Task<Response> SendNotification(Notification notification)
		{
			if (notificationProperties.isWhiteListed(notification?.Communication?.Value))
			{
				return new Response(ResponseType.Success, "Notification sent");
			}
			return notification.Action switch
			{
				Action.ConsentRequestCreated => await smsClient.Send(
					notification.Communication.Value,
					GenerateConsentRequestMessage(notification.Content), notification.GetTemplateID()),
				Action.ConsentManagerIdRecovered => await smsClient.Send(
					notification.Communication.Value,
					GenerateConsentManagerIdRecoveredMessage(notification.Content), notification.GetTemplateID()),
				_ => new Response(ResponseType.InternalServerError, "")
				};
		}

		private static string GenerateConsentRequestMessage(JToken notificationContent)
		{
			var content = notificationContent.ToObject<Content>();
			var message =
				$"Hello, {content.Requester} is requesting your consent for accessing health data for {content.HiTypes}. On providing" +
				$" consent, {content.Requester} will get access to all the health data for which you have provided consent. " +
				$"To view request, please tap on the link: {content.DeepLinkUrl}";
			return message;
		}

		private string GenerateConsentManagerIdRecoveredMessage(JToken notificationContent)
		{
			var consentManagerIdContent = notificationContent.ToObject<ConsentManagerIdContent>();
			var message =
				$"The {notificationProperties.PatientIdName} associated with your details is {consentManagerIdContent.ConsentManagerId}." +
				" To make sure that your account is secure, we request you to reset the password";
			return message;
		}
	}
}
