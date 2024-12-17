namespace LoginService.Models
{
    public class ValidateActivationModel
    {
        public string Username { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
    }

    public class ValidateActivationResponse
    {
        public string referenceNo { get; set; } = string.Empty;
        public bool status { get; set; } = false;
    }
}
