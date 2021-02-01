using System.Threading.Tasks;
using In.ProjectEKA.OtpService.Clients;
using In.ProjectEKA.OtpService.Common;
using In.ProjectEKA.OtpService.Otp;
using Newtonsoft.Json.Linq;

namespace In.ProjectEKA.OtpService.Notification
{
	public class NotificationService : INotificationService
	{
		private readonly ISmsClient smsClient;
		private readonly NotificationProperties notificationProperties;
		private readonly SmsServiceProperties smsServiceProperties;

		public NotificationService(ISmsClient smsClient, NotificationProperties notificationProperties, SmsServiceProperties smsServiceProperties)
		{
			this.smsClient = smsClient;
			this.notificationProperties = notificationProperties;
			this.smsServiceProperties = smsServiceProperties;
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
				Action.HIPVisitSMSNotification => await smsClient.Send(
					notification.Communication.Value,
					GenerateHIPSMSNotificationMessage(notification.Content), notification.GetTemplateID()),
				Action.ConsentManagerIdRecovered => await smsClient.Send(
					notification.Communication.Value,
					GenerateConsentManagerIdRecoveredMessage(notification.Content), notification.GetTemplateID()),
				_ => new Response(ResponseType.InternalServerError, "")
				};
		}

		private string GenerateConsentRequestMessage(JToken notificationContent)
		{
			var content = notificationContent.ToObject<Content>();
			var message =
				$"Hello, {content.Requester} is requesting your consent for accessing health data for {content.HiTypes}. On providing" +
				$" consent, {content.Requester} will get access to all the health data for which you have provided consent. " +
				$"To view request, please tap on the link: {content.DeepLinkUrl} {smsServiceProperties.SmsSuffix}";
			return message;
		}

		private string GenerateConsentManagerIdRecoveredMessage(JToken notificationContent)
		{
			var consentManagerIdContent = notificationContent.ToObject<ConsentManagerIdContent>();
			var message =
				$"The {notificationProperties.PatientIdName} associated with your details is {consentManagerIdContent.ConsentManagerId}." +
				$" To make sure that your account is secure, we request you to reset the password {smsServiceProperties.SmsSuffix}";
			return message;
		}
		
		private string GenerateHIPSMSNotificationMessage(JToken notificationContent)
		{
			var content = notificationContent.ToObject<HIPSMSNotificationContent>();
			var message =
				$"Hi {content.ReceiverName}, You can now access your {content.CareContextInfo} " +
				$"from {content.HospitalName} digitally, please use the link below {content.DeeplinkUrl} {smsServiceProperties.SmsSuffix}";
			return message;
		}
	}
}
