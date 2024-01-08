using AutoMapper;
using MediatR;
using Touride.Abstraction.Dtos;
using Touride.Abstraction.Models.TestModels;
using Touride.Abstraction.Services;
using Touride.Framework.Abstractions.Application.Models;
using Touride.Framework.Abstractions.Data.TransactionManagement;
using Touride.Framework.EventBus.Abstractions;

namespace Touride.Application.Handler.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
    {

        private readonly IEventBus _eventBus;
        private readonly IUserService _testService;
        public readonly IMapper _mapper;
        public CreateUserCommandHandler(IMapper mapper, IEventBus eventBus, IUserService testService)
        {
            _mapper = mapper;
            _eventBus = eventBus;
            _testService = testService;
        }

        [Transactional]
        public virtual async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            var result = await _testService.Insert(_mapper.Map<CreateTestModel>(request));

            if (result.Success)
            {
                //await _eventBus.PublishAsync(new TestIntegrationEvent(request.Id, request.Name));

                return new SuccessResult<UserDto>(result.Data);
            }
            return new NoContentResult<UserDto>();
        }
    }
}
