namespace In.ProjectEKA.OtpService.Notification
{
    public class NotificationProperties
    {
       public string PatientIdName { get; }
        
        public NotificationProperties(string patientIdName)
        {
            PatientIdName = patientIdName;
        }
    }
}