public record ResetRequest(
    string Email,
    string NewPassword,
    string ConfirmPassword
);