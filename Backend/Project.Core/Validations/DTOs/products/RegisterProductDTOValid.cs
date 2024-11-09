using FluentValidation;
using Project.Core.Entities.Products.DTOs;
using Project.Core.Utils.ErrorMsg.Interface;

namespace Project.Core.Entities.Products.Validation.DTOs.Products
{
  public class RegisterProductDTOValidator : AbstractValidator<RegisterProductDTO>
  {
    private readonly IRequiredValidationMsg _requiredValService;
    private readonly IContentValidationMsg _contentValService;
    private readonly IRangeValidationMsg _rangeValService;
    private readonly ITypeValuesValidationMsg _typeValuesValService;

    public RegisterProductDTOValidator(
      IRequiredValidationMsg requiredValService, 
      IContentValidationMsg contentValService, 
      IRangeValidationMsg rangeValService, 
      ITypeValuesValidationMsg typeValuesValService)
    {
      _requiredValService = requiredValService;
      _contentValService = contentValService;
      _rangeValService = rangeValService;
      _typeValuesValService = typeValuesValService;

      RuleFor(x => x.Name)
        .NotEmpty().WithMessage(_requiredValService.GetRequiredFieldMsg("Name"))
        .Must(x => x is string).WithMessage(_typeValuesValService.GetValidateStringMsg("Name"))
        .Matches(@"^[a-zA-Z0-9ñÑáéíóúÁÉÍÓÚ\s\n\t]*$").WithMessage(_contentValService.GetNoSpecialCharactersMsg("Name"))
        .Length(3, 50).WithMessage(_rangeValService.GetRangeLengthMsg("Name", 3, 50));

      RuleFor(x => x.Price)
        .NotEmpty().WithMessage(_requiredValService.GetRequiredFieldMsg("Price"))
        .Must(x => double.TryParse(x.ToString(), out _)).WithMessage(_typeValuesValService.GetValidateDoubleMsg("Price"))
        .InclusiveBetween(0.1, 1000000).WithMessage(_rangeValService.GetRangeErrorMsg("Price", 0.1, 1000000));

      RuleFor(x => x.ProductStatus)
        .NotEmpty().WithMessage(_requiredValService.GetRequiredFieldMsg("productStatus"))
        .IsInEnum().WithMessage(_typeValuesValService.GetInvalidEnumValueMsg("productStatus"));
    }
  }
}
