using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Linq.Expressions;
using Touride.Framework.Abstractions.Data.Enums;
using Touride.Framework.Data.Abstractions;
using Touride.Framework.Data.Configuration;
using Touride.Framework.Utilities;

namespace Touride.Framework.Data.Repository
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly UnitOfWork _dbContext;
        protected readonly DbSet<TEntity> _dbSet;
        private readonly UnitOfWorkOptions _unitOfWorkOptions;
        public BaseRepository(IUnitOfWork dbContext, IOptions<UnitOfWorkOptions> options)
        {
            _dbContext = dbContext == null ? throw new ArgumentNullException(nameof(dbContext)) : dbContext as UnitOfWork;
            _dbSet = _dbContext.Set<TEntity>();
            _unitOfWorkOptions = options.Value;
        }

        #region[CRUD_OPERATIONS]

        public virtual TEntity Insert(TEntity entity, InsertStrategy insertStrategy = InsertStrategy.InsertAll)
        {
            return _dbSet.Add(entity).Entity;
        }

        public void Insert(params TEntity[] entities)
        {
            _dbSet.AddRange(entities);
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await _dbSet.AddAsync(entity, cancellationToken);
            return res.Entity;
        }

        public async Task InsertAsync(params TEntity[] entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public virtual void Update(TEntity entity, UpdateStrategy updateStrategy = UpdateStrategy.UpdateAll)
        {
            _dbSet.Update(entity);
        }

        public void Update(params TEntity[] entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public abstract void Delete(object id);

        public virtual void Delete(TEntity entity, DeleteStrategy deleteStrategy = DeleteStrategy.MainIfRequiredAddChilds)
        {
            _dbSet.Remove(entity);
        }

        public void Delete(params TEntity[] entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        protected void TrackGraph(object rootEntity, Action<EntityEntryGraphNode> action)
        {
            _dbContext.ChangeTracker.TrackGraph(rootEntity, action);
        }

        #endregion[END_CRUD_OPERATIONS]

        #region[QUERY_OPERATIONS]

        public IQueryable<TEntity> GetAll()
        {
            Expression<Func<TEntity, bool>> predicate = null;
            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);
            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", "true", predicate);
            return _dbSet.Where(predicate);
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault)
        {
            IQueryable<TEntity> query = _dbSet;

            query = SetTracking(query, tracking);

            if (include != null)
            {
                query = include(query);
            }

            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            query = query.Where(predicate);

            return orderBy != null ? orderBy(query) : query;
        }


        public async Task<IList<TEntity>> GetAllAsync()
        {
            Expression<Func<TEntity, bool>> predicate = null;
            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", "false", predicate);
            predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", "true", predicate);
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault)
        {
            IQueryable<TEntity> query = _dbSet;

            query = SetTracking(query, tracking);

            if (include != null)
            {
                query = include(query);
            }

            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            query = query.Where(predicate);

            return orderBy != null ? await orderBy(query).ToListAsync() : await query.ToListAsync();
        }

        public TEntity Find(
            Expression<Func<TEntity, bool>> predicate,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true)
        {

            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            return _dbSet.Find(predicate);
        }

        public ValueTask<TEntity> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true)
        {
            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            return _dbSet.FindAsync(predicate);
        }

        public ValueTask<TEntity> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true)
        {
            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            return _dbSet.FindAsync(predicate, cancellationToken);
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault)
        {
            IQueryable<TEntity> query = _dbSet;

            query = SetTracking(query, tracking);

            if (include != null)
            {
                query = include(query);
            }

            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            query = query.Where(predicate);

            return orderBy != null ? orderBy(query).FirstOrDefault() : query.FirstOrDefault();
        }

        public TResult GetFirstOrDefault<TResult>(Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault)
        {
            IQueryable<TEntity> query = _dbSet;

            query = SetTracking(query, tracking);

            if (include != null)
            {
                query = include(query);
            }

            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            query = query.Where(predicate);

            return orderBy != null ? orderBy(query).Select(selector).FirstOrDefault() : query.Select(selector).FirstOrDefault();
        }

        public async Task<TResult> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault)
        {
            IQueryable<TEntity> query = _dbSet;

            query = SetTracking(query, tracking);

            if (include != null)
            {
                query = include(query);
            }

            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            query = query.Where(predicate);


            return orderBy != null ? await orderBy(query).Select(selector).FirstOrDefaultAsync() : await query.Select(selector).FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true,
            TrackingBehaviour tracking = TrackingBehaviour.ContextDefault)
        {
            IQueryable<TEntity> query = _dbSet;

            query = SetTracking(query, tracking);

            if (include != null)
            {
                query = include(query);
            }

            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            query = query.Where(predicate);

            return orderBy != null ? await orderBy(query).FirstOrDefaultAsync() : await query.FirstOrDefaultAsync();
        }

        public IQueryable<TEntity> FromSql(string sql, params object[] parameters)
        {
            return _dbSet.FromSqlRaw(sql, parameters);
        }

        public bool Exists(
            Expression<Func<TEntity, bool>> selector = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true)
        {
            if (isTheDeleteQueryActive)
                selector = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), selector);
            if (isTheActiveQueryActive)
                selector = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), selector);

            return selector == null ? _dbSet.Any() : _dbSet.Any(selector);
        }

        public async Task<bool> ExistsAsync(
            Expression<Func<TEntity, bool>> selector = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true)
        {
            if (isTheDeleteQueryActive)
                selector = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), selector);
            if (isTheActiveQueryActive)
                selector = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), selector);

            return selector == null ? await _dbSet.AnyAsync() : await _dbSet.AnyAsync(selector);
        }


        #endregion[END_QUERY_OPERATIONS]

        #region[AGGRIGATIONS]

        public int Count(
            Expression<Func<TEntity, bool>> predicate = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true)
        {
            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            return predicate == null ? _dbSet.Count() : _dbSet.Count(predicate);
        }

        public async Task<int> CountAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true
            )
        {
            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            return predicate == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(predicate);
        }

        public long LongCount(
            Expression<Func<TEntity, bool>> predicate = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true)
        {
            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            return predicate == null ? _dbSet.LongCount() : _dbSet.LongCount(predicate);
        }

        public async Task<long> LongCountAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true
            )
        {
            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            return predicate == null ? await _dbSet.LongCountAsync() : await _dbSet.LongCountAsync(predicate);
        }

        public T Max<T>(
            Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, T>> selector = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true)
        {

            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            return predicate == null ? _dbSet.Max(selector) : _dbSet.Where(predicate).Max(selector);
        }

        public async Task<T> MaxAsync<T>(
            Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, T>> selector = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true
            )
        {
            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            return predicate == null
                ? await _dbSet.MaxAsync(selector)
                : await _dbSet.Where(predicate).MaxAsync(selector);
        }

        public T Min<T>(
            Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, T>> selector = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true
            )
        {
            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            return predicate == null ? _dbSet.Min(selector) : _dbSet.Where(predicate).Min(selector);
        }

        public async Task<T> MinAsync<T>(
            Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, T>> selector = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true)
        {

            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            return predicate == null
                ? await _dbSet.MinAsync(selector)
                : await _dbSet.Where(predicate).MinAsync(selector);
        }

        public decimal Average(
            Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, decimal>> selector = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true)
        {

            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            return predicate == null ? _dbSet.Average(selector) : _dbSet.Where(predicate).Average(selector);
        }

        public async Task<decimal> AverageAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, decimal>> selector = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true)
        {
            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            if (predicate == null)
            {
                return await _dbSet.AverageAsync(selector);
            }

            return await _dbSet.Where(predicate).AverageAsync(selector);
        }

        public decimal Sum(
            Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, decimal>> selector = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true)
        {

            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            return predicate == null ? _dbSet.Sum(selector) : _dbSet.Where(predicate).Sum(selector);
        }

        public async Task<decimal> SumAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Expression<Func<TEntity, decimal>> selector = null,
            bool isDeleted = false,
            bool isActive = true,
            bool isTheDeleteQueryActive = true,
            bool isTheActiveQueryActive = true)
        {

            if (isTheDeleteQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultDeletePropertyName, "==", isDeleted.ToString(), predicate);
            if (isTheActiveQueryActive)
                predicate = LinqExpressionProvider.strToFunc<TEntity>(_unitOfWorkOptions.DefaultActivePropertyName, "==", isActive.ToString(), predicate);

            if (predicate == null)
            {
                return await _dbSet.SumAsync(selector);
            }

            return await _dbSet.Where(predicate).SumAsync(selector);
        }

        #endregion[END_AGGRIGATIONS]           

        #region[CONTEXT_OPERATIONS]

        public void Detach(TEntity entity)
        {
            var entry = GetEntityEntry(entity);
            if (entry != null)
            {
                _dbContext.Entry(entity).State = EntityState.Detached;
            }
        }

        #endregion[END_CONTEXT_OPERATIONS]


        #region[SQL_COMMAND_OPERATIONS]

        /// <summary>
        /// Raw Sql Calıştırmak için kullanılır.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns>Etkilenen Kayıt Sayısı</returns>
        public int ExecuteSqlRaw(string sql, params object[] parameters)
        {
            return _dbContext.Database.ExecuteSqlRaw(sql, parameters);
        }

        /// <summary>
        /// Raw Sql Calıştırmak için kullanılır.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns>Etkilenen Kayıt Sayısı</returns>
        public int ExecuteSqlRaw(string sql, IEnumerable<object> parameters)
        {
            return _dbContext.Database.ExecuteSqlRaw(sql, parameters);
        }

        /// <summary>
        /// Raw Sql Calıştırmak için kullanılır.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns>Etkilenen Kayıt Sayısı</returns>
        public async Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        /// <summary>
        /// Raw Sql Calıştırmak için kullanılır.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns>Etkilenen Kayıt Sayısı</returns>
        public async Task<int> ExecuteSqlRawAsync(string sql, IEnumerable<object> parameters)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        #endregion[SQL_COMMAND_OPERATIONS]

        #region [UNITOFWORK_OPERATIONS]
        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
        #endregion [UNITOFWORK_OPERATIONS]


        protected abstract IQueryable<TEntity> SetTracking(IQueryable<TEntity> query, TrackingBehaviour tracking);

        protected EntityEntry<TEntity> GetEntityEntry(TEntity entity)
        {
            return _dbContext.Entry(entity);
        }

        #region[IQUERYABLE_IMPLEMENTATION]
        public Type ElementType => SetTracking(_dbSet, TrackingBehaviour.ContextDefault).ElementType;

        public Expression Expression => SetTracking(_dbSet, TrackingBehaviour.ContextDefault).Expression;

        public IQueryProvider Provider => SetTracking(_dbSet, TrackingBehaviour.ContextDefault).Provider;

        public IEnumerator<TEntity> GetEnumerator()
        {
            return SetTracking(_dbSet, TrackingBehaviour.ContextDefault).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return SetTracking(_dbSet, TrackingBehaviour.ContextDefault).GetEnumerator();
        }
        #endregion[END_IQUERYABLE_IMPLEMENTATION]
    }
}
