namespace Touride.Abstraction.Services
{
    public interface IStaticBusinessService
    {
        Task<string> GetDynamicStringResource(string key, Guid? entityId, string languageIndex, int? entity2Id = null);
    }
}
