using FluentValidation;
using Touride.Abstraction.Models.TestModels;

namespace Touride.Application.Validators.TestValidators
{
    public abstract class BaseTestValidator<T> : AbstractValidator<T> where T : CreateTestModel
    {
        public BaseTestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
