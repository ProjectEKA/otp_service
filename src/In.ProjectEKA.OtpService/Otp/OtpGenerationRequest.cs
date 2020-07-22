namespace In.ProjectEKA.OtpService.Otp
{
    public class OtpGenerationRequest
    {
        public string SessionId { get; set; }
        public Communication Communication { get; set; }
        public OtpCreationDetail CreationDetail { get; set; }

        public OtpGenerationRequest(string sessionId, Communication communication, OtpCreationDetail creationDetail)
        {
            SessionId = sessionId;
            Communication = communication;
            CreationDetail = creationDetail;
        }
    }
}