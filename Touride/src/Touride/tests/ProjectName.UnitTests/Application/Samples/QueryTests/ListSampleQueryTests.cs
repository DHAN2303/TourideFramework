using Moq;
using Otokoc2El.Framework.Abstractions.Data.Enums;
using ProjectName.Application.Samples.Queries.ListSample;
using ProjectName.Domain.AggregatesModel.Samples;
using ProjectName.UnitTests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectName.UnitTests.Application.Samples.QueryTests
{
    public class ListSampleQueryTests : TestBase
    {
        private readonly ListSampleQueryHandler _handler;

        private readonly Mock<ISampleRepository> _mockSampleRepository;
        public ListSampleQueryTests()
        {
            _mockSampleRepository = new Mock<ISampleRepository>();
            _handler = new ListSampleQueryHandler(_mockSampleRepository.Object);
        }
        public static IEnumerable<object[]> ListSampleQuery()
        {
            yield return new object[] {
                new ListSampleQuery(){}
            };
        }
        [Theory]
        [MemberData(nameof(ListSampleQuery))]
        public async Task ListSampleQueryHandler_Should_ReturnResponse_When_Success(ListSampleQuery query)
        {
            var samples = GetGeneratedActiveSampleList();

            _mockSampleRepository.Setup(q => q.GetListAsync(x => 1 == 1,
                It.IsAny<Func<IQueryable<Sample>, IOrderedQueryable<Sample>>>(),
                It.IsAny<TrackingBehaviour>()))
                .ReturnsAsync(samples);

            var response = await _handler.Handle(query, cancellationToken: CancellationToken.None);
            Assert.IsType<ListSampleQueryResponse>(response);
        }
    }
}
