namespace diggie_server.src.shared.email.models;

public class OtpEmailViewModel 
{
    public string AppName { get; set; } = "Diggie";
    public string OtpCode { get; set; } = string.Empty;
    public int ExpiredMinutes { get; set; }    
    public string Name { get; set; } = string.Empty;
}