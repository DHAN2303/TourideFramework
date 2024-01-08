using ProjectName.Domain.AggregatesModel.Samples;
using System;
using System.Collections.Generic;

namespace ProjectName.UnitTests.Base
{
    public class TestBase
    {
        public static List<Sample> GetGeneratedActiveSampleList()
        {
            return new List<Sample>
            {
                Sample.Map(
                id: new Guid("130ddbd8-7f1d-4474-94fb-91428aa845ab"),
                name: "Hakan",
                creationTime: new DateTime(2022, 04, 12),
                isDeleted: false,
                isActive: true
                )
            };
        }
        public static Sample GetGeneratedActiveSample()
        {
            return Sample.Map(
                id: new Guid("130ddbd8-7f1d-4474-94fb-91428aa845ab"),
                name: "Hakan",
                creationTime: new DateTime(2022, 04, 12),
                isDeleted: false,
                isActive: true
                );
        }
    }
}
