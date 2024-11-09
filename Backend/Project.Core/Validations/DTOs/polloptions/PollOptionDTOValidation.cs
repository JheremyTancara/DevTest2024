using FluentValidation;
using Project.Core.Entities.Options.DTOs;
using Project.Core.Entities.Products.DTOs;
using Project.Core.Utils.ErrorMsg.Interface;

namespace Project.Core.Entities.Products.Validation.DTOs.Products
{
  public class PollOptionDTOValidator : AbstractValidator<RegisterPollOptionsDTO>
  {
    private readonly IRequiredValidationMsg _requiredValService;

    public PollOptionDTOValidator(
      IRequiredValidationMsg requiredValService)
    {
      _requiredValService = requiredValService;

      RuleFor(x => x.Name)
        .NotEmpty().WithMessage(_requiredValService.GetRequiredFieldMsg("Name"));
    }
  }
}