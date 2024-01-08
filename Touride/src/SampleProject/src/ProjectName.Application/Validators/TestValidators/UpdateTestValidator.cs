using FluentValidation;
using ProjectName.Abstraction.Constants;
using ProjectName.Abstraction.Models.TestModels;

namespace ProjectName.Application.Validators.TestValidators
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
