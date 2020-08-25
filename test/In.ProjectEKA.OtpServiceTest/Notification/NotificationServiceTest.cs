namespace In.ProjectEKA.OtpServiceTest.Notification
{
	using Builder;
	using FluentAssertions;
	using Moq;
	using OtpService.Clients;
	using OtpService.Common;
	using OtpService.Notification;
	using Xunit;

	public class NotificationServiceTest
    {
        private readonly Mock<ISmsClient> notificationWebHandler = new Mock<ISmsClient>();
        private readonly NotificationProperties notificationProperties = new NotificationProperties("consent manager ID");
        private readonly NotificationService notificationService;

        public NotificationServiceTest()
        {
            notificationService = new NotificationService(notificationWebHandler.Object, notificationProperties);
        }

        [Fact]
        private async void ReturnSuccessResponse()
        {
            var expectedResponse = new Response(ResponseType.Success, "Notification sent");
            notificationWebHandler.Setup(e => e.Send(It.IsAny<string>(),
                    It.IsAny<string>())).ReturnsAsync(expectedResponse);

            var response = await notificationService.SendNotification(TestBuilder.GenerateNotificationMessage());

            response.Should().Be(expectedResponse);
        }
        
        [Fact]
        private async void ReturnSuccessResponseForConsentManagerIdRecovered()
        {
            var expectedResponse = new Response(ResponseType.Success, "Notification sent");
            notificationWebHandler.Setup(e => e.Send(It.IsAny<string>(),
                It.IsAny<string>())).ReturnsAsync(expectedResponse);

            var response = await notificationService.SendNotification(TestBuilder.GenerateNotificationMessageForConsentManagerIdRecovered());

            response.Should().Be(expectedResponse);
        }

        [Fact]
        private async void ReturnInternalServerError()
        {
            var expectedResponse = new Response(ResponseType.InternalServerError, "Internal server error");
            notificationWebHandler.Setup(e => e.Send(It.IsAny<string>(),
                It.IsAny<string>())).ReturnsAsync(expectedResponse);

            var response = await notificationService.SendNotification(TestBuilder.GenerateNotificationMessage());

            response.ResponseType.Should().Be(expectedResponse.ResponseType);
        }
        
        [Fact]
        private async void ReturnInternalServerErrorForConsentManagerIdRecovered()
        {
            var expectedResponse = new Response(ResponseType.InternalServerError, "Internal server error");
            notificationWebHandler.Setup(e => e.Send(It.IsAny<string>(),
                It.IsAny<string>())).ReturnsAsync(expectedResponse);

            var response = await notificationService.SendNotification(TestBuilder.GenerateNotificationMessageForConsentManagerIdRecovered());

            response.ResponseType.Should().Be(expectedResponse.ResponseType);
        }

    }
}