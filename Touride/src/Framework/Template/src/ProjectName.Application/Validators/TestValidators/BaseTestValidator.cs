using FluentValidation;
using ProjectName.Abstraction.Models.TestModels;

namespace ProjectName.Application.Validators.TestValidators
{
    public abstract class BaseTestValidator<T> : AbstractValidator<T> where T : CreateTestModel
    {
        public BaseTestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
