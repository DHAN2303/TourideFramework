using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Touride.Framework.Abstractions.TaskScheduling;

namespace Touride.Framework.TaskScheduling.Hangfire
{
    public class JobInitializationHelper
    {
        public static void InitializeJobs(Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                InitializeJobs(assembly);
            }
        }

        private static void InitializeJobs(Assembly assembly)
        {
            IEnumerable<Type> types = GetJobInitializerTypes(assembly);
            foreach (Type type in types)
            {
                IJobInitializer jobInitializer = Activator.CreateInstance(type) as IJobInitializer;
                jobInitializer.Initilize();
            }
        }


        private static IEnumerable<Type> GetJobInitializerTypes(Assembly assembly)
        {
            return assembly.GetTypes().Where(x => x != typeof(IJobInitializer) && typeof(IJobInitializer).IsAssignableFrom(x) && !x.IsAbstract);
        }
    }
}
