using Microsoft.Extensions.Localization;
using Moq;
using Otokoc2El.Framework.Abstractions.Data.Enums;
using Otokoc2El.Framework.Localization.Resources.Validation;
using ProjectName.Application.Samples.Commands.UpdateSample;
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
    public class UpdateSampleCommandTests : TestBase
    {
        private readonly UpdateSampleCommandHandler _handler;

        private readonly Mock<ISampleRepository> _mockSampleRepository;
        private readonly Mock<IStringLocalizer<ValidationResource>> _localizer;
        public UpdateSampleCommandTests()
        {
            _mockSampleRepository = new Mock<ISampleRepository>();
            _localizer = new Mock<IStringLocalizer<ValidationResource>>();
            _handler = new UpdateSampleCommandHandler(_mockSampleRepository.Object, _localizer.Object);
        }
        public static IEnumerable<object[]> UpdateSampleCommand()
        {
            yield return new object[] {
                new UpdateSampleCommand()
                {
                    Id = new Guid("ff2cd9e2-503a-4c73-85fc-ef1140dacab3"),
                    Name = "Hakan"
                }
            };
        }
        [Theory]
        [MemberData(nameof(UpdateSampleCommand))]
        public async Task UpdateSampleCommandHandler_Throws_Exception_When_Sample_Not_Found_Exception(UpdateSampleCommand command)
        {
            _mockSampleRepository.Setup(q => q.GetAsync(x => x.Id == command.Id, It.IsAny<TrackingBehaviour>()))
                .ReturnsAsync(Sample.Default());

            await Assert.ThrowsAsync<SampleBusinessException>(async () => await _handler.Handle(command, cancellationToken: CancellationToken.None));
        }
        [Theory]
        [MemberData(nameof(UpdateSampleCommand))]
        public async Task UpdateSampleCommandHandler_Should_ReturnResponse_When_Success(UpdateSampleCommand command)
        {
            var sample = GetGeneratedActiveSample();

            _mockSampleRepository.Setup(q => q.GetAsync(x => x.Id == command.Id, It.IsAny<TrackingBehaviour>()))
                .ReturnsAsync(sample);

            var response = await _handler.Handle(command, cancellationToken: CancellationToken.None);
            Assert.IsType<UpdateSampleCommandResponse>(response);
        }
    }
}
