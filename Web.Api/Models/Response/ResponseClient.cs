namespace WebApi.Models.Response
{
    public class ResponseClient
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessenger { get; set; }
        public string Token { get; set; }
    }
}
