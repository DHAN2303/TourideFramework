using FluentValidation;
using Touride.Abstraction.Constants;
using Touride.Abstraction.Models.TestModels;

namespace Touride.Application.Validators.TestValidators
{
    public class InsertAboutValidator : BaseTestValidator<CreateTestModel>
    {
        public InsertAboutValidator() : base()
        {
            RuleFor(x => x.Name).NotNull().
                WithMessage("{PropertyName}" + ValidationConstants.NotNull).
                NotEmpty().
                WithMessage("{PropertyName}" + ValidationConstants.NotEmpty);
        }
    }
}
