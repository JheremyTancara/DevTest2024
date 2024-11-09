using FluentValidation;
using Project.Core.Entities.Options.DTOs;
using Project.Core.Entities.Products.DTOs;
using Project.Core.Utils.ErrorMsg.Interface;

namespace Project.Core.Entities.Products.Validation.DTOs.Products
{
  public class VoteDTOValidator : AbstractValidator<RegisterVoteDTO>
  {
    private readonly IFormatValidationMsg _formatValService;

    public VoteDTOValidator(
      IFormatValidationMsg formatValService)
    {
      _formatValService = formatValService;

      RuleFor(x => x.VoterEmail)
        .Matches(@"^[a-zA-Z0-9._%+-]+@gmail\.com$").WithMessage(_formatValService.GetInvalidEmailFormatMsg("VoterEmail"));

    }
  }
}
