using DevExtreme.AspNet.Data.ResponseModel;
using ProjectName.Abstraction.Dtos;
using ProjectName.Abstraction.Models.TestModels;
using Touride.Framework.Abstractions.Application.Models;

namespace ProjectName.Abstraction.Services
{
    public interface IUserService
    {
        Task<Result<UserDto>> Insert(CreateTestModel createTestModel);
        Task<Result<List<UserDto>>> GetAll();
        Task<Result<List<LoadResult>>> GetAllDevExtreme();


    }
}
