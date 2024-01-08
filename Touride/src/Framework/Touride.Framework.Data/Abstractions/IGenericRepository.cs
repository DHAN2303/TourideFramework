using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Touride.Framework.Abstractions.Application.Models;
using Touride.Framework.Abstractions.Data.Entities;
using Touride.Framework.Abstractions.Data.Enums;
using Touride.Framework.Data.Entities;

namespace Touride.Framework.Data.Abstractions
{
    public interface IGenericRepository<TEntity, TDto>
                        where TEntity : class, IEntity, new()
                        where TDto : class, IDto, new()
    {
        #region[CRUD_OPERATIONS]
        /// <summary>
        /// Veri tabanına kayıt işlemi.
        /// </summary>
        /// <param name="entity">Kayıt edilmek istenilen entity.</param>
        /// <param name="insertStrategy">Kayıt işlemi sırasında izlenecek strateji.</param>
        /// <returns>Kayıt işleminden sonra kaydedilen entitynin son hali.</returns>
        Result<TDto> Insert(TEntity entity, InsertStrategy insertStrategy = InsertStrategy.InsertAll);

        /// <summary>
        /// Veri tabanı kayıt işlemi.
        /// </summary>
        /// <param name="entities">Kayıt edilmek istenen entity arrayi.</param>
        Result<TDto[]> Insert(params TEntity[] entities);

        /// <summary>
        /// Veri tabanı kayıt işlemi.
        /// </summary>
        /// <param name="entities">Kayıt edilmek istenen entity listesi.</param>
        Result<IEnumerable<TDto>> Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// Veri tabanına kayıt işlemi.
        /// </summary>
        /// <param name="entity">Kayıt edilmek istenilen entity.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Kayıt işleminden sonra kaydedilen entitynin son hali.</returns>
        Task<Result<TDto>> InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Veri tabanı kayıt işlemi.
        /// </summary>
        /// <param name="entities">Kayıt edilmek istenen entity arrayi.</param>
        /// <returns>Async operasyon</returns>
        Task<Result<TDto[]>> InsertAsync(params TEntity[] entities);

        Task<Result<IEnumerable<TDto>>> InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken));

        Result<TDto> Update(TEntity entity, UpdateStrategy updateStrategy = UpdateStrategy.UpdateAll);

        Result<TDto[]> Update(params TEntity[] entities);

        Result<IEnumerable<TDto>> Update(IEnumerable<TEntity> entities);

        Result<bool> Delete(Expression<Func<TEntity, bool>> predicate = null);

        Result<bool> Delete(TEntity entity, DeleteStrategy deleteStrategy = DeleteStrategy.MainIfRequiredAddChilds);

        Result<bool> Delete(params TEntity[] entities);

        Result<bool> Delete(IEnumerable<TEntity> entities);

        #endregion[END_CRUD_OPERATIONS]

        #region[QUERY_OPERATIONS]

        Result<List<TDto>> GetAll();
        Result<List<TDto>> GetAll(bool isDeleted = true);
        Result<List<TDto>> GetAll(
            bool isDeleted = true,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault);
        Task<Result<List<TDto>>> GetAllAsync();
        Task<Result<List<TDto>>> GetAllAsync(bool isDeleted = true);
        Task<Result<List<TDto>>> GetAllAsync(
            bool isDeleted = true,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault);

        Result<PagedResponse<TDto>> GetAllPaginator(PagedRequest pagedRequest, bool isDeleted = true);
        Result<PagedResponse<TDto>> GetAllPaginator(PagedRequest pagedRequest);
        Result<PagedResponse<TDto>> GetAllPaginator(
            PagedRequest pagedRequest, bool isDeleted = true, Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault);

        Task<Result<List<TDto>>> GetAllFilterAsync(
            List<SorguParametre> sorguParametreleri,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault);
        Task<Result<List<TDto>>> GetAllFilterAsync(
            List<SorguParametre> sorguParametreleri,
            bool isDeleted = true,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault);

        Task<Result<PagedResponse<TDto>>> GetAllPaginatorAsync(PagedRequest pagedRequest);
        Task<Result<PagedResponse<TDto>>> GetAllPaginatorAsync(PagedRequest pagedRequest, bool isDeleted = true);
        Task<Result<PagedResponse<TDto>>> GetAllPaginatorAsync(
            PagedRequest pagedRequest,
            bool isDeleted = true,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault);

        Result<TDto> GetFirstOrDefault(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault);

        Result<TDto> GetFirstOrDefault(
            bool isDeleted = true,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault);

        #endregion[END_QUERY_OPERATIONS]

        #region[AGGRIGATIONS]

        Result<int> Count(Expression<Func<TEntity, bool>> predicate = null);

        Task<Result<int>> CountAsync(Expression<Func<TEntity, bool>> predicate = null);

        Result<long> LongCount(Expression<Func<TEntity, bool>> predicate = null);

        Task<Result<long>> LongCountAsync(Expression<Func<TEntity, bool>> predicate = null);

        Result<T> Max<T>(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, T>> selector = null);

        Task<Result<T>> MaxAsync<T>(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, T>> selector = null);

        Result<T> Min<T>(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, T>> selector = null);

        Task<Result<T>> MinAsync<T>(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, T>> selector = null);

        Result<decimal> Average(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, decimal>> selector = null);

        Task<Result<decimal>> AverageAsync(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, decimal>> selector = null);

        Result<decimal> Sum(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, decimal>> selector = null);

        Task<Result<decimal>> SumAsync(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, decimal>> selector = null);

        #endregion[END_AGGRIGATIONS]           

        #region[UNITOFWORK_OPERATIONS]
        int SaveChanges();
        Task<int> SaveChangesAsync();
        #endregion[UNITOFWORK_OPERATIONS]

    }
}
