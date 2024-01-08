using Microsoft.Extensions.Localization;
using Moq;
using Otokoc2El.Framework.Abstractions.Data.Enums;
using Otokoc2El.Framework.Localization.Resources.Validation;
using ProjectName.Application.Samples.Queries.GetSample;
using ProjectName.Domain.AggregatesModel.Samples;
using ProjectName.Domain.Exceptions;
using ProjectName.UnitTests.Base;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectName.UnitTests.Application.Samples.QueryTests
{
    public class GetSampleQueryTests : TestBase
    {
        private readonly GetSampleQueryHandler _handler;

        private readonly Mock<ISampleRepository> _mockSampleRepository;
        private readonly Mock<IStringLocalizer<ValidationResource>> _localizer;
        public GetSampleQueryTests()
        {
            _mockSampleRepository = new Mock<ISampleRepository>();
            _localizer = new Mock<IStringLocalizer<ValidationResource>>();
            _handler = new GetSampleQueryHandler(_mockSampleRepository.Object, _localizer.Object);
        }
        public static IEnumerable<object[]> GetSampleQuery()
        {
            yield return new object[] {
                new GetSampleQuery(new Guid("ff2cd9e2-503a-4c73-85fc-ef1140dacab3"))
            };
        }
        [Theory]
        [MemberData(nameof(GetSampleQuery))]
        public async Task GetSampleQueryHandler_Throws_Exception_When_Sample_Not_Found_Exception(GetSampleQuery query)
        {
            _mockSampleRepository.Setup(q => q.GetAsync(x => x.Id == query.Id, It.IsAny<TrackingBehaviour>()))
                .ReturnsAsync(Sample.Default());

            await Assert.ThrowsAsync<SampleBusinessException>(async () => await _handler.Handle(query, cancellationToken: CancellationToken.None));
        }
        [Theory]
        [MemberData(nameof(GetSampleQuery))]
        public async Task GetSampleQueryHandler_Should_ReturnResponse_When_Success(GetSampleQuery query)
        {
            var sample = GetGeneratedActiveSample();

            _mockSampleRepository.Setup(q => q.GetAsync(x => x.Id == query.Id, It.IsAny<TrackingBehaviour>()))
                .ReturnsAsync(sample);

            var response = await _handler.Handle(query, cancellationToken: CancellationToken.None);
            Assert.IsType<GetSampleQueryResponse>(response);
        }
    }
}
