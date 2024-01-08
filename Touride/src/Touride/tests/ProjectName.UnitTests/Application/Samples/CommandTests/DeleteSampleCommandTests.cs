using Microsoft.Extensions.Localization;
using Moq;
using Otokoc2El.Framework.Abstractions.Data.Enums;
using Otokoc2El.Framework.Localization.Resources.Validation;
using ProjectName.Application.Samples.Commands.DeleteSample;
using ProjectName.Domain.AggregatesModel.Samples;
using ProjectName.Domain.Exceptions;
using ProjectName.UnitTests.Base;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectName.UnitTests.Application.Samples.CommandTests
{
    public class DeleteSampleCommandTests : TestBase
    {
        private readonly DeleteSampleCommandHandler _handler;

        private readonly Mock<ISampleRepository> _mockSampleRepository;
        private readonly Mock<IStringLocalizer<ValidationResource>> _localizer;
        public DeleteSampleCommandTests()
        {
            _mockSampleRepository = new Mock<ISampleRepository>();
            _localizer = new Mock<IStringLocalizer<ValidationResource>>();
            _handler = new DeleteSampleCommandHandler(_mockSampleRepository.Object, _localizer.Object);
        }
        public static IEnumerable<object[]> DeleteSampleCommand()
        {
            yield return new object[] {
                new DeleteSampleCommand(new Guid("ff2cd9e2-503a-4c73-85fc-ef1140dacab3"))
            };
        }
        [Theory]
        [MemberData(nameof(DeleteSampleCommand))]
        public async Task DeleteSampleCommandHandler_Throws_Exception_When_Sample_Not_Found_Exception(DeleteSampleCommand command)
        {
            _mockSampleRepository.Setup(q => q.GetAsync(x => x.Id == command.Id, It.IsAny<TrackingBehaviour>()))
                .ReturnsAsync(Sample.Default());

            await Assert.ThrowsAsync<SampleBusinessException>(async () => await _handler.Handle(command, cancellationToken: CancellationToken.None));
        }
        [Theory]
        [MemberData(nameof(DeleteSampleCommand))]
        public async Task DeleteSampleCommandHandler_Should_ReturnResponse_When_Success(DeleteSampleCommand command)
        {
            var sample = GetGeneratedActiveSample();

            _mockSampleRepository.Setup(q => q.GetAsync(x => x.Id == command.Id, It.IsAny<TrackingBehaviour>()))
                .ReturnsAsync(sample);

            var response = await _handler.Handle(command, cancellationToken: CancellationToken.None);
            Assert.IsType<DeleteSampleCommandResponse>(response);
        }
    }
}
