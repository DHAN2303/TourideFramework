using Microsoft.Extensions.Localization;
using Moq;
using Otokoc2El.Framework.Localization.Resources.Validation;
using ProjectName.Application.Samples.Commands.CreateSample;
using ProjectName.Domain.AggregatesModel.Samples;
using ProjectName.Domain.Exceptions;
using ProjectName.UnitTests.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectName.UnitTests.Application.Samples.CommandTests
{
    public class CreateSampleCommandTests : TestBase
    {
        private readonly CreateSampleCommandHandler _handler;

        private readonly Mock<ISampleRepository> _mockSampleRepository;
        private readonly Mock<IStringLocalizer<ValidationResource>> _localizer;
        public CreateSampleCommandTests()
        {
            _mockSampleRepository = new Mock<ISampleRepository>();
            _localizer = new Mock<IStringLocalizer<ValidationResource>>();
            _handler = new CreateSampleCommandHandler(_mockSampleRepository.Object, _localizer.Object);
        }
        public static IEnumerable<object[]> CreateSampleCommand()
        {
            yield return new object[] {
                new CreateSampleCommand()
                {
                    Name = "Hakan"
                }
            };
        }
        [Theory]
        [MemberData(nameof(CreateSampleCommand))]
        public async Task CreateSampleCommandHandler_Throws_Exception_When_Sample_Not_Created_Exception(CreateSampleCommand command)
        {
            _mockSampleRepository.Setup(q => q.InsertAsync(It.IsAny<Sample>(), false, CancellationToken.None))
                .ReturnsAsync(Sample.Default());

            await Assert.ThrowsAsync<PageBusinessException>(async () => await _handler.Handle(command, cancellationToken: CancellationToken.None));
        }
        [Theory]
        [MemberData(nameof(CreateSampleCommand))]
        public async Task CreateSampleCommandHandler_Should_ReturnResponse_When_Success(CreateSampleCommand command)
        {
            var sample = GetGeneratedActiveSample();

            _mockSampleRepository.Setup(q => q.InsertAsync(It.IsAny<Sample>(), false, CancellationToken.None))
                .ReturnsAsync(sample);

            var response = await _handler.Handle(command, cancellationToken: CancellationToken.None);
            Assert.IsType<CreateSampleCommandResponse>(response);
        }
    }
}
