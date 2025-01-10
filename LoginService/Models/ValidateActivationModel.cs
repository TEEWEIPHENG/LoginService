namespace LoginService.Models
{
    public class RequestOTPModel
    {
        public string username { get; set; } = string.Empty;
        public string mobileNo { get; set; } = string.Empty;
    }

    public class RequestOTPResponse
    {
        public string referenceNo { get; set; } = string.Empty;
        public bool status { get; set; } = false;
    }
}
