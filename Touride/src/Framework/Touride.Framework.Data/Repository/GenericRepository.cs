using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using Touride.Framework.Abstractions.Application.Models;
using Touride.Framework.Abstractions.Data.Entities;
using Touride.Framework.Abstractions.Data.Enums;
using Touride.Framework.Data.Abstractions;
using Touride.Framework.Data.Configuration;
using Touride.Framework.Data.Entities;
using Touride.Framework.Data.Extensions;
using Touride.Framework.Utilities;

namespace Touride.Framework.Data.Repository
{
    public class GenericRepository<TEntity, TDto> : IGenericRepository<TEntity, TDto>
        where TDto : class, IDto, new()
        where TEntity : class, IEntity, new()
    {
        private readonly ILogger<GenericRepository<TEntity, TDto>> _logger;
        private readonly IRepository<TEntity> _repository;
        private readonly UnitOfWorkOptions _unitOfWorkOptions;
        private readonly IMapper _mapper;

        public GenericRepository(
            ILogger<GenericRepository<TEntity, TDto>> logger,
            IRepository<TEntity> repository,
            IMapper mapper,
            IOptions<UnitOfWorkOptions> options)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _unitOfWorkOptions = options.Value;
        }

        #region[CRUD_OPERATIONS]

        public virtual Result<TDto> Insert(TEntity entity, InsertStrategy insertStrategy = InsertStrategy.InsertAll)
        {
            var insert = _repository.Insert(entity);

            if (insert != null)
            {
                return new SuccessResult<TDto>(_mapper.Map<TDto>(insert));
            }

            return new NoContentResult<TDto>();
        }

        public Result<TDto[]> Insert(params TEntity[] entities)
        {
            _repository.Insert(entities);

            return new SuccessResult<TDto[]>(_mapper.Map<TDto[]>(entities));
        }

        public Result<IEnumerable<TDto>> Insert(IEnumerable<TEntity> entities)
        {
            _repository.Insert(entities);

            return new SuccessResult<IEnumerable<TDto>>(_mapper.Map<IEnumerable<TDto>>(entities));
        }

        public async Task<Result<TDto>> InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await _repository.InsertAsync(entity, cancellationToken);

            if (res != null)
            {
                return new SuccessResult<TDto>(_mapper.Map<TDto>(res));
            }

            return new NoContentResult<TDto>();

        }

        public async Task<Result<TDto[]>> InsertAsync(params TEntity[] entities)
        {
            await _repository.InsertAsync(entities);

            return new SuccessResult<TDto[]>(_mapper.Map<TDto[]>(entities));
        }

        public async Task<Result<IEnumerable<TDto>>> InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _repository.InsertAsync(entities, cancellationToken);

            return new SuccessResult<IEnumerable<TDto>>(_mapper.Map<IEnumerable<TDto>>(entities));
        }

        public Result<TDto> Update(TEntity entity, UpdateStrategy updateStrategy = UpdateStrategy.UpdateAll)
        {
            _repository.Update(entity);

            return new SuccessResult<TDto>(_mapper.Map<TDto>(entity));
        }

        public Result<TDto[]> Update(params TEntity[] entities)
        {
            _repository.Update(entities);

            return new SuccessResult<TDto[]>(_mapper.Map<TDto[]>(entities));

        }

        public Result<IEnumerable<TDto>> Update(IEnumerable<TEntity> entities)
        {
            _repository.Update(entities);

            return new SuccessResult<IEnumerable<TDto>>(_mapper.Map<IEnumerable<TDto>>(entities));
        }

        public Result<bool> Delete(Expression<Func<TEntity, bool>> predicate = null)
        {
            var entity = _repository.Find(predicate);

            if (entity != null)
            {
                Delete(entity);

                return new SuccessResult<bool>(true);
            }

            return new NoContentResult<bool>();
        }

        public Result<bool> Delete(TEntity entity, DeleteStrategy deleteStrategy = DeleteStrategy.MainIfRequiredAddChilds)
        {
            _repository.Delete(entity);

            return new SuccessResult<bool>(true);
        }

        public Result<bool> Delete(params TEntity[] entities)
        {
            _repository.Delete(entities);

            return new SuccessResult<bool>(true);
        }

        public Result<bool> Delete(IEnumerable<TEntity> entities)
        {
            _repository.Delete(entities);

            return new SuccessResult<bool>(true);
        }

        #endregion[END_CRUD_OPERATIONS]

        #region[QUERY_OPERATIONS]

        public Result<List<TDto>> GetAll()
        {
            Expression<Func<TEntity, bool>> predicate = null;

            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            var result = _repository.GetAll(predicate: predicate);

            if (result != null)
            {
                return new SuccessResult<List<TDto>>(_mapper.Map<List<TDto>>(result));
            }

            return new NoContentResult<List<TDto>>(Messages.InvalidResult);
        }
        public Result<List<TDto>> GetAll(bool isDeleted = false)
        {
            Expression<Func<TEntity, bool>> predicate = null;

            predicate = isDeleted ?
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "true", predicate) :
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            //p=>p.isDeleted == false

            var result = _repository.GetAll(predicate: predicate);

            if (result != null)
            {
                return new SuccessResult<List<TDto>>(_mapper.Map<List<TDto>>(result));
            }

            return new NoContentResult<List<TDto>>(Messages.InvalidResult);
        }
        public Result<List<TDto>> GetAll(
            bool isDeleted = false,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault)
        {

            predicate = isDeleted ?
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "true", predicate) :
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            var result = _repository.GetAll(predicate: predicate, orderBy: orderBy, include: include);

            if (result != null)
            {
                return new SuccessResult<List<TDto>>(_mapper.Map<List<TDto>>(result));
            }

            return new NoContentResult<List<TDto>>(Messages.InvalidResult);
        }
        public Result<PagedResponse<TDto>> GetAllPaginator(PagedRequest pagedRequest)
        {
            Expression<Func<TEntity, bool>> predicate = null;

            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            if (pagedRequest.SorguParametreleri.Count != 0)
            {
                foreach (var item in pagedRequest.SorguParametreleri)
                {
                    if (item.Parametre == "like")
                    {
                        predicate = LikePredicate.strToFunc<TEntity>(item.Name, item.Parametre, item.Value, predicate);
                    }
                    else
                    {
                        predicate = LinqExpressionProvider.strToFunc<TEntity>(item.Name, item.Parametre, item.Value, predicate);
                    }

                }
            }

            foreach (var item in pagedRequest.SorguParametreleri)
            {
                predicate = LinqExpressionProvider.strToFunc<TEntity>(item.Name, item.Parametre, item.Value, predicate);
            }

            var result = _repository.GetAll(predicate: predicate).ToPagedList(pageNumber: pagedRequest.Paginator.Page.Value,
                pageSize: pagedRequest.Paginator.PageSize.Value);

            if (result != null)
            {

                var sorting = OrderBy.CustumOrderBy(result.Items, PascalCaseConvert.ToPascalCase(pagedRequest.Sorting.Column),
                    pagedRequest.Sorting.Direction == "desc" ? true : false);

                PagedResponse<TDto> res = new()
                {
                    Items = _mapper.Map<List<TDto>>(sorting),
                    Total = result.TotalCount
                };

                return new SuccessResult<PagedResponse<TDto>>(res);
            }

            return new NoContentResult<PagedResponse<TDto>>(Messages.InvalidResult);
        }
        public Result<PagedResponse<TDto>> GetAllPaginator(
            PagedRequest pagedRequest,
            bool isDeleted = false)
        {
            Expression<Func<TEntity, bool>> predicate = null;

            predicate = isDeleted ?
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "true", predicate) :
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            if (pagedRequest.SorguParametreleri.Count != 0)
            {
                foreach (var item in pagedRequest.SorguParametreleri)
                {
                    if (item.Parametre == "like")
                    {
                        predicate = LikePredicate.strToFunc<TEntity>(item.Name, item.Parametre, item.Value, predicate);
                    }
                    else
                    {
                        predicate = LinqExpressionProvider.strToFunc<TEntity>(item.Name, item.Parametre, item.Value, predicate);
                    }

                }
            }

            foreach (var item in pagedRequest.SorguParametreleri)
            {
                predicate = LinqExpressionProvider.strToFunc<TEntity>(item.Name, item.Parametre, item.Value, predicate);
            }

            var result = _repository.GetAll(predicate: predicate).ToPagedList(pageNumber: pagedRequest.Paginator.Page.Value,
                pageSize: pagedRequest.Paginator.PageSize.Value);

            if (result != null)
            {

                var sorting = OrderBy.CustumOrderBy(result.Items, PascalCaseConvert.ToPascalCase(pagedRequest.Sorting.Column),
                    pagedRequest.Sorting.Direction == "desc" ? true : false);

                PagedResponse<TDto> res = new()
                {
                    Items = _mapper.Map<List<TDto>>(sorting),
                    Total = result.TotalCount
                };

                return new SuccessResult<PagedResponse<TDto>>(res);
            }

            return new NoContentResult<PagedResponse<TDto>>(Messages.InvalidResult);
        }
        public Result<PagedResponse<TDto>> GetAllPaginator(
            PagedRequest pagedRequest,
            bool isDeleted = false,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault)
        {

            predicate = isDeleted ?
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "true", predicate) :
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            if (pagedRequest.SorguParametreleri.Count != 0)
            {
                foreach (var item in pagedRequest.SorguParametreleri)
                {
                    if (item.Parametre == "like")
                    {
                        predicate = LikePredicate.strToFunc<TEntity>(item.Name, item.Parametre, item.Value, predicate);
                    }
                    else
                    {
                        predicate = LinqExpressionProvider.strToFunc<TEntity>(item.Name, item.Parametre, item.Value, predicate);
                    }

                }
            }

            // p=>p.isDeleted == false && p.adi=="ahmet" && p.soyadi == "kafadar"

            var result = _repository.GetAll(predicate: predicate, orderBy: orderBy, include: include).ToPagedList(pageNumber: pagedRequest.Paginator.Page.Value,
                pageSize: pagedRequest.Paginator.PageSize.Value);

            if (result != null)
            {

                var sorting = OrderBy.CustumOrderBy(result.Items, PascalCaseConvert.ToPascalCase(pagedRequest.Sorting.Column),
                    pagedRequest.Sorting.Direction == "desc" ? true : false);

                PagedResponse<TDto> res = new()
                {
                    Items = _mapper.Map<List<TDto>>(sorting),
                    Total = result.TotalCount
                };

                return new SuccessResult<PagedResponse<TDto>>(res);
            }

            return new NoContentResult<PagedResponse<TDto>>(Messages.InvalidResult);
        }

        public async Task<Result<List<TDto>>> GetAllAsync()
        {
            Expression<Func<TEntity, bool>> predicate = null;

            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            var result = await _repository.GetAllAsync(predicate: predicate);

            if (result != null)
            {
                return new SuccessResult<List<TDto>>(_mapper.Map<List<TDto>>(result));
            }

            return new NoContentResult<List<TDto>>(Messages.InvalidResult);
        }
        public async Task<Result<List<TDto>>> GetAllAsync(bool isDeleted = false)
        {
            Expression<Func<TEntity, bool>> predicate = null;

            predicate = isDeleted ?
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "true", predicate) :
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            var result = await _repository.GetAllAsync(predicate: predicate);

            if (result != null)
            {
                return new SuccessResult<List<TDto>>(_mapper.Map<List<TDto>>(result));
            }

            return new NoContentResult<List<TDto>>(Messages.InvalidResult);
        }
        public async Task<Result<List<TDto>>> GetAllAsync(
            bool isDeleted = false,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault)
        {

            predicate = isDeleted ?
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "true", predicate) :
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            var result = await _repository.GetAllAsync(predicate: predicate, orderBy: orderBy, include: include);

            if (result != null)
            {
                return new SuccessResult<List<TDto>>(_mapper.Map<List<TDto>>(result));
            }

            return new NoContentResult<List<TDto>>(Messages.InvalidResult);
        }
        public async Task<Result<PagedResponse<TDto>>> GetAllPaginatorAsync(PagedRequest pagedRequest)
        {
            Expression<Func<TEntity, bool>> predicate = null;

            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            if (pagedRequest.SorguParametreleri.Count != 0)
            {
                foreach (var item in pagedRequest.SorguParametreleri)
                {
                    if (item.Parametre == "like")
                    {
                        predicate = LikePredicate.strToFunc<TEntity>(item.Name, item.Parametre, item.Value, predicate);
                    }
                    else
                    {
                        predicate = LinqExpressionProvider.strToFunc<TEntity>(item.Name, item.Parametre, item.Value, predicate);
                    }

                }
            }

            var result = _repository.GetAll(predicate: predicate).ToPagedList(pageNumber: pagedRequest.Paginator.Page.Value,
                pageSize: pagedRequest.Paginator.PageSize.Value);

            if (result != null)
            {

                var sorting = OrderBy.CustumOrderBy(result.Items, PascalCaseConvert.ToPascalCase(pagedRequest.Sorting.Column),
                    pagedRequest.Sorting.Direction == "desc" ? true : false);

                PagedResponse<TDto> res = new()
                {
                    Items = _mapper.Map<List<TDto>>(sorting),
                    Total = result.TotalCount
                };

                return new SuccessResult<PagedResponse<TDto>>(res);
            }

            return new NoContentResult<PagedResponse<TDto>>(Messages.InvalidResult);
        }
        public async Task<Result<PagedResponse<TDto>>> GetAllPaginatorAsync(
            PagedRequest pagedRequest,
            bool isDeleted = false)
        {
            Expression<Func<TEntity, bool>> predicate = null;

            predicate = isDeleted ?
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "true", predicate) :
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            if (pagedRequest.SorguParametreleri.Count != 0)
            {
                foreach (var item in pagedRequest.SorguParametreleri)
                {
                    if (item.Parametre == "like")
                    {
                        predicate = LikePredicate.strToFunc<TEntity>(item.Name, item.Parametre, item.Value, predicate);
                    }
                    else
                    {
                        predicate = LinqExpressionProvider.strToFunc<TEntity>(item.Name, item.Parametre, item.Value, predicate);
                    }

                }
            }

            var result = _repository.GetAll(predicate: predicate).ToPagedList(pageNumber: pagedRequest.Paginator.Page.Value,
                pageSize: pagedRequest.Paginator.PageSize.Value);

            if (result != null)
            {

                var sorting = OrderBy.CustumOrderBy(result.Items, PascalCaseConvert.ToPascalCase(pagedRequest.Sorting.Column),
                    pagedRequest.Sorting.Direction == "desc" ? true : false);

                PagedResponse<TDto> res = new()
                {
                    Items = _mapper.Map<List<TDto>>(sorting),
                    Total = result.TotalCount
                };

                return new SuccessResult<PagedResponse<TDto>>(res);
            }

            return new NoContentResult<PagedResponse<TDto>>(Messages.InvalidResult);
        }
        public async Task<Result<PagedResponse<TDto>>> GetAllPaginatorAsync(
            PagedRequest pagedRequest,
            bool isDeleted = false,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault)
        {

            predicate = isDeleted ?
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "true", predicate) :
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            if (pagedRequest.SorguParametreleri != null)
            {
                if (pagedRequest.SorguParametreleri.Count != 0)
                {
                    foreach (var item in pagedRequest.SorguParametreleri)
                    {
                        if (item.Parametre == "like")
                        {
                            predicate = LikePredicate.strToFunc<TEntity>(item.Name, item.Parametre, item.Value, predicate);
                        }
                        else
                        {
                            predicate = LinqExpressionProvider.strToFunc<TEntity>(item.Name, item.Parametre, item.Value, predicate);
                        }

                    }
                }
            }

            var result = _repository.GetAll(predicate: predicate, orderBy: orderBy, include: include)
                                            .ToPagedList(pageNumber: pagedRequest.Paginator.Page.Value,
                                                            pageSize: pagedRequest.Paginator.PageSize.Value);


            if (result != null)
            {

                //var sorting = OrderBy.CustumOrderBy(result.Items, PascalCaseConvert.ToPascalCase(pagedRequest.Sorting.Column),
                //pagedRequest.Sorting.Direction == "desc" ? true : false);


                PagedResponse<TDto> res = new()
                {
                    Items = _mapper.Map<List<TDto>>(result.Items),
                    Total = result.TotalCount
                };

                return new SuccessResult<PagedResponse<TDto>>(res);



            }

            return new NoContentResult<PagedResponse<TDto>>(Messages.InvalidResult);
        }
        public async Task<Result<List<TDto>>> GetAllFilterAsync(
            List<SorguParametre> sorguParametreleri,
            bool isDeleted = false,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault)
        {

            predicate = isDeleted ?
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "true", predicate) :
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            foreach (var item in sorguParametreleri)
            {
                predicate = LinqExpressionProvider.strToFunc<TEntity>(item.Name, item.Parametre, item.Value, predicate);
            }


            var result = await _repository.GetAllAsync(predicate: predicate, orderBy: orderBy, include: include);

            if (result != null)
            {
                return new SuccessResult<List<TDto>>(_mapper.Map<List<TDto>>(result));
            }

            return new NoContentResult<List<TDto>>(Messages.InvalidResult);
        }

        public async Task<Result<List<TDto>>> GetAllFilterAsync(
            List<SorguParametre> sorguParametreleri,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault)
        {

            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            foreach (var item in sorguParametreleri)
            {
                predicate = LinqExpressionProvider.strToFunc<TEntity>(item.Name, item.Parametre, item.Value, predicate);
            }

            var result = await _repository.GetAllAsync(predicate: predicate, orderBy: orderBy, include: include);

            if (result != null)
            {
                return new SuccessResult<List<TDto>>(_mapper.Map<List<TDto>>(result));
            }

            return new NoContentResult<List<TDto>>(Messages.InvalidResult);
        }
        public Result<TDto> GetFirstOrDefault(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault)
        {

            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            var result = _repository.GetFirstOrDefault(predicate: predicate, orderBy: orderBy, include: include, tracking: TrackingBehaviour.AsNoTracking);

            if (result != null)
            {
                return new SuccessResult<TDto>(_mapper.Map<TDto>(result));
            }

            return new NoContentResult<TDto>(Messages.InvalidResult);
        }
        public Result<TDto> GetFirstOrDefault(
            bool isDeleted = false,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault)
        {

            predicate = isDeleted ?
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "true", predicate) :
                LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            var result = _repository.GetFirstOrDefault(predicate: predicate, orderBy: orderBy, include: include, tracking: TrackingBehaviour.AsNoTracking);

            if (result != null)
            {
                return new SuccessResult<TDto>(_mapper.Map<TDto>(result));
            }

            return new NoContentResult<TDto>(Messages.InvalidResult);
        }



        #endregion[END_QUERY_OPERATIONS]

        #region[AGGRIGATIONS]

        public Result<int> Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            var result = _repository.Count(predicate: predicate);

            if (result != 0)
            {
                return new SuccessResult<int>(result);
            }

            return new NoContentResult<int>(Messages.InvalidResult);

        }

        public async Task<Result<int>> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            var result = await _repository.CountAsync(predicate: predicate);

            if (result != 0)
            {
                return new SuccessResult<int>(result);
            }

            return new NoContentResult<int>(Messages.InvalidResult);
        }

        public Result<long> LongCount(Expression<Func<TEntity, bool>> predicate = null)
        {
            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            var result = _repository.LongCount(predicate: predicate);

            if (result != 0)
            {
                return new SuccessResult<long>(result);
            }

            return new NoContentResult<long>(Messages.InvalidResult);
        }

        public async Task<Result<long>> LongCountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            var result = await _repository.LongCountAsync(predicate: predicate);

            if (result != 0)
            {
                return new SuccessResult<long>(result);
            }

            return new NoContentResult<long>(Messages.InvalidResult);
        }

        public Result<T> Max<T>(Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, T>> selector = null)
        {
            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            var result = _repository.Max<T>(predicate: predicate);

            if (result != null)
            {
                return new SuccessResult<T>(result);
            }

            return new NoContentResult<T>(Messages.InvalidResult);
        }

        public async Task<Result<T>> MaxAsync<T>(Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, T>> selector = null)
        {
            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            var result = await _repository.MaxAsync<T>(predicate: predicate);

            if (result != null)
            {
                return new SuccessResult<T>(result);
            }

            return new NoContentResult<T>(Messages.InvalidResult);
        }

        public Result<T> Min<T>(Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, T>> selector = null)
        {
            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            var result = _repository.Min<T>(predicate: predicate);

            if (result != null)
            {
                return new SuccessResult<T>(result);
            }

            return new NoContentResult<T>(Messages.InvalidResult);
        }

        public async Task<Result<T>> MinAsync<T>(Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, T>> selector = null)
        {
            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            var result = await _repository.MinAsync<T>(predicate: predicate);

            if (result != null)
            {
                return new SuccessResult<T>(result);
            }

            return new NoContentResult<T>(Messages.InvalidResult);
        }

        public Result<decimal> Average(Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, decimal>> selector = null)
        {
            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            var result = _repository.Average(predicate: predicate, selector: selector);

            if (result != 0)
            {
                return new SuccessResult<decimal>(result);
            }

            return new NoContentResult<decimal>(Messages.InvalidResult);
        }

        public async Task<Result<decimal>> AverageAsync(Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, decimal>> selector = null)
        {
            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            var result = await _repository.AverageAsync(predicate: predicate, selector: selector);

            if (result != 0)
            {
                return new SuccessResult<decimal>(result);
            }

            return new NoContentResult<decimal>(Messages.InvalidResult);
        }

        public Result<decimal> Sum(Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, decimal>> selector = null)
        {
            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            var result = _repository.Sum(predicate: predicate, selector: selector);

            if (result != 0)
            {
                return new SuccessResult<decimal>(result);
            }

            return new NoContentResult<decimal>(Messages.InvalidResult);
        }

        public async Task<Result<decimal>> SumAsync(Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, decimal>> selector = null)
        {
            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);

            var result = await _repository.SumAsync(predicate: predicate, selector: selector);

            if (result != 0)
            {
                return new SuccessResult<decimal>(result);
            }

            return new NoContentResult<decimal>(Messages.InvalidResult);
        }

        #endregion[END_AGGRIGATIONS]

        #region [UNITOFWORK_OPERATIONS]
        public int SaveChanges()
        {
            return _repository.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _repository.SaveChangesAsync();
        }
        #endregion [UNITOFWORK_OPERATIONS]


    }
}
