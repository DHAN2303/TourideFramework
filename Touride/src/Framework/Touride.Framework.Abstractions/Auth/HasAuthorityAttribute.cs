using Touride.Framework.Abstractions.Ioc;

namespace Touride.Framework.Abstractions.Auth
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class HasAuthorityAttribute : InterceptionAttribute, IAuthorityAttribute
    {
        public string[] Action { get; set; }
        public HasAuthorityAttribute(string[] Action) =>
            (this.Action) = (Action);


    }
}
