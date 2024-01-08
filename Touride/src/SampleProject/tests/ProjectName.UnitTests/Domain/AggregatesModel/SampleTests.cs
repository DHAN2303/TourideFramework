using ProjectName.Domain.AggregatesModel.Samples;
using ProjectName.Domain.Exceptions;
using System;
using Xunit;

namespace ProjectName.UnitTests.Domain.AggregatesModel
{
    public class SampleTests
    {
        private readonly Sample _sample;

        #region Constructor
        public SampleTests()
        {
            _sample = Sample.Map(
                id: new Guid("130ddbd8-7f1d-4474-94fb-91428aa845ab"),
                name: "Hakan",
                creationTime: new DateTime(2022, 04, 12),
                isDeleted: false,
                isActive: true
            );
        }
        #endregion

        #region CreateNew Method Tests
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CreateNew_Should_ThrowException_When_NameIsNullOrEmpty(string name)
        {
            Assert.Throws<SampleDomainException>(() => new Sample(name: name));

        }
        [Fact]
        public void CreateNew_Should_Be_Successful()
        {
            var name = "Hakan";
            var sample = new Sample(name: name);

            Assert.True(sample.IsActive);
            Assert.Equal(sample.Name, name);
        }
        #endregion

        #region Map Method Tests

        [Fact]
        public void Map_Should_Be_Successful()
        {
            var id = new Guid("130ddbd8-7f1d-4474-94fb-91428aa845ab");
            var name = "Hakan";
            var creationTime = new DateTime(2022, 04, 12);
            var isDeleted = false;
            var isActive = true;
            var creatorId = new Guid("92290455-50fc-4c33-82f2-cec39971f1aa");
            var deletionTime = new DateTime(2022, 04, 12);
            var lastModificationTime = new DateTime(2022, 04, 12);
            var lastModifierId = new Guid("6beca0ff-b076-45e8-87af-f3e0d3b87c66");

            var sample = Sample.Map(
                id: id,
                name: name,
                creationTime: creationTime,
                isDeleted: isDeleted,
                isActive: isActive
            );

            Assert.Equal(id, sample.Id);
            Assert.Equal(name, sample.Name);
            Assert.Equal(creationTime, sample.CreationTime);
            Assert.Equal(isDeleted, sample.IsDeleted);
            Assert.Equal(isActive, sample.IsActive);
            Assert.True(sample.Exists());
        }
        #endregion

        #region Default Method Tests
        [Fact]
        public void Default_Should_Be_Successful()
        {
            var sample = Sample.Default();
            Assert.False(sample.Exists());
        }
        #endregion

        #region Set Name Tests
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SetName_Should_Throw_Exception_When_When_NameIsNullOrEmpty(string name)
        {
            Assert.Throws<SampleDomainException>(() => _sample.SetName(name));
        }

        [Fact]
        public void SetName_Should_Be_Successful()
        {
            var name = "Cem";

            _sample.SetName(name);

            Assert.Equal(_sample.Name, name);
        }
        #endregion

    }
}
