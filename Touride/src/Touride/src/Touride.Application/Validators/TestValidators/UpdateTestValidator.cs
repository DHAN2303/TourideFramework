using FluentValidation;
using Touride.Abstraction.Constants;
using Touride.Abstraction.Models.TestModels;

namespace Touride.Application.Validators.TestValidators
{
    public class UpdateTestValidator : BaseTestValidator<UpdateTestModel>
    {
        public UpdateTestValidator() : base()
        {
            RuleFor(x => x.Id).
                NotNull().
                WithMessage("{PropertyName}" + ValidationConstants.NotNull).
                NotEmpty().
                WithMessage("{PropertyName}" + ValidationConstants.NotEmpty);
        }
    }
}
