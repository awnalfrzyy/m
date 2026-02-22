// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// HOW TO USE
// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

// public class ResetRequestValidator : AbstractValidator<ResetRequest> 
// {
//     public ResetRequestValidator() 
//     {
//         AuthValidator.ApplyEmailRules(RuleFor(x => x.Email));
//         AuthValidator.ApplyPasswordRules(RuleFor(x => x.NewPassword));

//         RuleFor(x => x.ConfirmPassword)
//             .Equal(x => x.NewPassword).WithMessage("Konfirmasi password tidak cocok");
//     }
// }

// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++



using System.Text.RegularExpressions;

namespace diggie_server.src.shared.validation;

public static class ValidationGuard
{
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@(gmail|yahoo|outlook|hotmail)\.com$", RegexOptions.Compiled);
    private static readonly Regex PasswordRegex = new(@"^[a-zA-Z0-9]{8,}$", RegexOptions.Compiled);

    public static void ValidateAuth(string email, string password)
    {
        if (!EmailRegex.IsMatch(email))
            throw new ArgumentException("Email domains must be .gmail, .yahoo, .outlook, or .hotmail and end in .com");

        if (!PasswordRegex.IsMatch(password))
            throw new ArgumentException("Password must be at least 8 characters and must not contain symbols");
    }
}