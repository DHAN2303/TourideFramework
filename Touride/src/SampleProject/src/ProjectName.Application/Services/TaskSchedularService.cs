using Microsoft.Extensions.Configuration;
using ProjectName.Abstraction.Services;
using Touride.Framework.Abstractions.TaskScheduling;

namespace ProjectName.Application.Services
{
    public class TaskSchedularService : ITaskSchedularService
    {
        private readonly ITaskSchedulingEngine _taskSchedulingEngine;
        private readonly IConfiguration _configuration;
        public TaskSchedularService(ITaskSchedulingEngine taskSchedulingEngine, IConfiguration configuration)
        {
            _taskSchedulingEngine = taskSchedulingEngine;
            _configuration = configuration;
        }


        public void Execute()
        {
            var sendContractProcessMail = _taskSchedulingEngine.AddRecurringJob<IDataCollectorService>(x => x.GetandSave(),
             _configuration["CronJobTiming:Shorting"], "default");
        }
    }
}
