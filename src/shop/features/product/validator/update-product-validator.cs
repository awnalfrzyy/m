using FluentValidation;

namespace diggie_server.src.features.product.validator;

public class UpdateProductValidator : AbstractValidator<UpdateRequestProduct>
{
    public UpdateProductValidator()
    {
        // Rule: Kalau Name dikirim (nggak null), minimal 3 huruf, nggak boleh cuma spasi
        RuleFor(x => x.Name)
            .MinimumLength(3).WithMessage("Nama produk minimal 3 karakter, Win.")
            .When(x => x.Name != null);

        // Rule: Kalau Price dikirim, minimal harganya 1000 perak misalnya
        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(1000).WithMessage("Harga minimal Rp 1.000!")
            .When(x => x.Price != null);

        // Rule: Kalau Quantity dikirim, nggak boleh minus
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stok nggak bisa minus!")
            .When(x => x.Quantity != null);

        // Rule: Kalau Brand dikirim, minimal 2 huruf
        RuleFor(x => x.Brand)
            .MinimumLength(2).WithMessage("Brand minimal 2 huruf.")
            .When(x => x.Brand != null);
    }
}