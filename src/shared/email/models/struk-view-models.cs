namespace diggie_server.src.shared.email.models;

public class StrukViewModel
{
    public string AppName { get; set; } = "Diggie";
    public string Product { get; set; } = "Spotify Premium";
    public string Brand { get; set; } = "Spotify";
    public int Quantity { get; set; } = 1;
    public string Plan { get; set; } = "Premium Pro";
    public DateTime Duration { get; set; } = DateTime.UtcNow.AddDays(30);
    public decimal Price { get; set; }
    public string Payments { get; set; } = "Credit Card";
    public string Status { get; set; } = "Succes";
    public string Name { get; set; } = "Aswin Alfarizi";
}