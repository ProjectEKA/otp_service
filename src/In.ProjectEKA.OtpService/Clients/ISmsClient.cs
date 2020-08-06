namespace In.ProjectEKA.OtpService.Clients
{
	using System.Threading.Tasks;
	using Common;

	public interface ISmsClient
    {
        public Task<Response> Send(string phoneNumber, string message);
    }
}