using FluentValidation;
using Project.Core.Entities.Polls.DTOs;
using Project.Core.Entities.Products.DTOs;
using Project.Core.Utils.ErrorMsg.Interface;

namespace Project.Core.Entities.Products.Validation.DTOs.Products
{
  public class PollDTOValidator : AbstractValidator<RegisterPollDTO>
  {
    private readonly IRequiredValidationMsg _requiredValService;
    private readonly IContentValidationMsg _contentValService;
    private readonly IRangeValidationMsg _rangeValService;

    public PollDTOValidator(
      IRequiredValidationMsg requiredValService, 
      IContentValidationMsg contentValService, 
      IRangeValidationMsg rangeValService)
    {
      _requiredValService = requiredValService;
      _contentValService = contentValService;
      _rangeValService = rangeValService;

      RuleFor(x => x.Name)
        .NotEmpty().WithMessage(_requiredValService.GetRequiredFieldMsg("Name"))
        .Matches(@"^[a-zA-Z0-9ñÑáéíóúÁÉÍÓÚ\s\n\t]*$").WithMessage(_contentValService.GetNoSpecialCharactersMsg("Name"))
        .Length(3, 50).WithMessage(_rangeValService.GetRangeLengthMsg("Name", 3, 50));
    }
  }
}
