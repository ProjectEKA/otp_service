namespace In.ProjectEKA.OtpService.Otp
{
    public class OtpGenerationRequest
    {
        public string SessionId { get; set; }
        public Communication Communication { get; set; }
        public OtpGenerationDetail GenerationDetail { get; set; }

        public OtpGenerationRequest(string sessionId, Communication communication, OtpGenerationDetail generationDetail)
        {
            SessionId = sessionId;
            Communication = communication;
            GenerationDetail = generationDetail;
        }
    }
}