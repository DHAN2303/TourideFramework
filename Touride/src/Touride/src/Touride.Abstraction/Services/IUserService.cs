using DevExtreme.AspNet.Data.ResponseModel;
using Touride.Abstraction.Dtos;
using Touride.Abstraction.Models.TestModels;
using Touride.Framework.Abstractions.Application.Models;

namespace Touride.Abstraction.Services
{
    public interface IUserService
    {
        Task<Result<UserDto>> Insert(CreateTestModel createTestModel);
        Task<Result<List<UserDto>>> GetAll();
        Task<Result<List<LoadResult>>> GetAllDevExtreme();


    }
}
