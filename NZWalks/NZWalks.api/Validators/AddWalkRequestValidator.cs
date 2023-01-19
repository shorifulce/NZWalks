using FluentValidation;

namespace NZWalks.api.Validators
{
    public class AddWalkRequestValidator : AbstractValidator<Models.DTO.AddWalksRequest>
    {
        public AddWalkRequestValidator() 
        {
          
        }
    }
}
