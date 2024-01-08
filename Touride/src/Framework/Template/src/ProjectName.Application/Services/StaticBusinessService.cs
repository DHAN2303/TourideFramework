using ProjectName.Abstraction.Services;
using Touride.Framework.Statics.Services;

namespace ProjectName.Application.Services
{
    public class StaticBusinessService : IStaticBusinessService
    {
        private readonly IStaticService<string> _staticService;
        public StaticBusinessService(IStaticService<string> staticService)
        {
            _staticService = staticService;
        }
        public async Task<string> GetDynamicStringResource(string key, Guid? entityId, string languageIndex, int? entity2Id = null)
        {
            return await _staticService.GetDynamicResourceAsync(key, entityId, languageIndex, entity2Id);
        }
    }
}
