using System.ComponentModel;

namespace LoginService.Models.Enum
{
    public enum OnboardingStatusEnum
    {
        [Description("Success")]
        Success = 1,

        [Description("Fail")]
        Fail = 2,

        [Description("Account Existed")]
        AccountExisted = 3,

        [Description("Error")]
        Error = 4
    }
}
