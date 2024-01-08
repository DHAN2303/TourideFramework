namespace Touride.Framework.Statics.Services
{
    public interface IStaticService<T>
    {
        //Task<T> GetAsync(Guid id);
        Task<string> GetDynamicResourceAsync(string key, Guid? entityId, string languageIndex, int? entity2Id = null);
        Task<T> GetAsync(Guid id);
    }
}
