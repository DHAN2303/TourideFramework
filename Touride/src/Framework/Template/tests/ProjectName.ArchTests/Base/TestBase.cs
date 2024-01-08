using NetArchTest.Rules;
using NUnit.Framework;
using ProjectName.Application.Samples.Commands.CreateSample;
using ProjectName.Domain.AggregatesModel.Samples;
using ProjectName.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProjectName.ArchTests.Base
{
    public abstract class TestBase
    {
        protected static Assembly ApplicationAssembly => typeof(CreateSampleCommand).Assembly;

        protected static Assembly DomainAssembly => typeof(Sample).Assembly;

        protected static Assembly InfrastructureAssembly => typeof(SampleRepository).Assembly;

        protected static void AssertAreImmutable(IEnumerable<Type> types)
        {
            IList<Type> failingTypes = new List<Type>();
            foreach (var type in types)
            {
                if (type.GetFields().Any(x => !x.IsInitOnly) || type.GetProperties().Any(x => x.CanWrite))
                {
                    failingTypes.Add(type);
                    break;
                }
            }

            AssertFailingTypes(failingTypes);
        }

        protected static void AssertFailingTypes(IEnumerable<Type> types)
        {
            Assert.That(types, Is.Null.Or.Empty);
        }

        protected static void AssertArchTestResult(TestResult result)
        {
            AssertFailingTypes(result.FailingTypes);
        }
    }
}
