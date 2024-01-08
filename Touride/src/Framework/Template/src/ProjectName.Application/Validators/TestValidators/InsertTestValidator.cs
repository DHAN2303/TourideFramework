using FluentValidation;
using ProjectName.Abstraction.Constants;
using ProjectName.Abstraction.Models.TestModels;

namespace ProjectName.Application.Validators.TestValidators
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
