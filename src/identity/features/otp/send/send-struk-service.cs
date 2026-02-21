using diggie_server.src.finance.features.receipt.send;
using diggie_server.src.shared.email.models;
namespace diggie_server.src.identity.features.otp;

public class SendStrukHandler
{
    private readonly IEmailService _emailService;

    public SendStrukHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<bool> Handle(SendStrukRequest request)
    {
        var viewModel = new StrukViewModel
        {
            Product = request.ProductName,
            Plan = request.PlanName,
            Price = request.Price,
            Payments = request.PaymentMethod,
            Name = "Aswin Alfarizi",
            Status = "Success"
        };

        return await _emailService.SendAsync(
            request.Email,
            $"Receipt for {request.ProductName}",
                viewModel,
            "struk-view-template"
        );
    }
}