using System;
using FluentAssertions;
using In.ProjectEKA.OtpService.Clients;
using In.ProjectEKA.OtpService.Common;
using In.ProjectEKA.OtpService.Otp;
using In.ProjectEKA.OtpServiceTest.Otp.Builder;
using Moq;
using Xunit;
using Action = In.ProjectEKA.OtpService.Otp.Action;

namespace In.ProjectEKA.OtpServiceTest.Otp
{
    [Collection("Otp Service Tests")]
    public class OtpSenderTest
    {
        private readonly OtpSender otpSender;
        private readonly Mock<IOtpRepository> otpRepository = new Mock<IOtpRepository>();
        private readonly Mock<IOtpGenerator> otpGenerator = new Mock<IOtpGenerator>();
        private readonly Mock<ISmsClient> otpWebHandler = new Mock<ISmsClient>();
        private readonly OtpProperties otpProperties = new OtpProperties(5);
        private readonly SmsServiceProperties smsServiceProperties = new SmsServiceProperties(String.Empty,
            String.Empty, String.Empty, String.Empty, String.Empty, 0, String.Empty);
        public OtpSenderTest()
        {
            otpSender = new OtpSender(otpRepository.Object, otpGenerator.Object, otpWebHandler.Object, otpProperties, smsServiceProperties);
        }

        [Fact]
        private async void ReturnSuccessResponse()
        {
            var sessionId = TestBuilder.Faker().Random.Hash();
            const string otpToken = "123456";
            var systemName = TestBuilder.Faker().Random.Word();
            var phoneNumber = TestBuilder.Faker().Phone.PhoneNumber();
            var testOtpResponse = new Response(ResponseType.Success, "Otp Created");
            var otpCreationDetail = new OtpGenerationDetail(systemName, Action.FORGOT_PIN.ToString());
            var otpRequest = new OtpGenerationRequest(sessionId, new Communication("MOBILE"
                , phoneNumber), otpCreationDetail);
            var generatedMessage = otpSender.GenerateMessage(otpCreationDetail, otpToken);
            otpGenerator.Setup(e => e.GenerateOtp()).Returns(otpToken);
            otpWebHandler.Setup(e => e.Send(otpRequest.Communication.Value, generatedMessage, otpRequest.GenerationDetail.GetTemplateID()))
                .ReturnsAsync(testOtpResponse);
            otpRepository.Setup(e => e.Save(otpToken, sessionId))
                .ReturnsAsync(testOtpResponse);

            var otpResponse = await otpSender.GenerateOtp(otpRequest);

            otpGenerator.Verify();
            otpWebHandler.Verify();
            otpRepository.Verify();
            otpResponse.Should().BeEquivalentTo(testOtpResponse);
        }
    }
}