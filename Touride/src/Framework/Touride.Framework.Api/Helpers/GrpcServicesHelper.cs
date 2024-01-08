using System.Reflection;

namespace Touride.Framework.Api.Helpers
{
    public static class GrpcServicesHelper
    {
        public static Dictionary<string, string> GetGrpcServices(string assemblyName)
        {
            if (!string.IsNullOrEmpty(assemblyName))
            {
                Assembly assembly = Assembly.Load(assemblyName);
                List<Type> ts = assembly.GetTypes().ToList();

                var result = new Dictionary<string, string>();
                foreach (var item in ts.Where(u => u.CustomAttributes.Any(a => a.AttributeType.Name == "GrpcServiceAttribute")))
                {
                    result.Add(item.Name, item.Namespace);
                }
                return result;
            }
            return new Dictionary<string, string>();
        }

        public static Dictionary<Type, Type[]> GetClassName(string assemblyName)
        {
            if (!String.IsNullOrEmpty(assemblyName))
            {
                Assembly assembly = Assembly.Load(assemblyName);
                List<Type> ts = assembly.GetTypes().ToList();

                var result = new Dictionary<Type, Type[]>();
                foreach (var item in ts.Where(s => !s.IsInterface))
                {
                    var interfaceType = item.GetInterfaces();
                    result.Add(item, interfaceType);
                }
                return result;
            }
            return new Dictionary<Type, Type[]>();
        }
    }
}
