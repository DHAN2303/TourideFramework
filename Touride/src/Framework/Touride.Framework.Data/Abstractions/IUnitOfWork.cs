using Touride.Framework.Abstractions.Data.AuditLog;

namespace Touride.Framework.Data.Abstractions
{
    /// <summary>
    /// Veri tabanı işlemlerinin tek seferde yapılabilmesini olanak sağlar.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Belirli bir tipteki entity için repository üretir.<seealso cref="IRepository{TEntity}"/>
        /// </summary>
        /// <typeparam name="TEntity">Repositorysi istenilen entity</typeparam>
        /// <returns>Belirtilen tipteki entity için repository</returns>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        /// <summary>
        /// Değişikliklerin kaydedilmesi için kullanılır.
        /// </summary>
        /// <param name="auditBehaviour"><seealso cref="Touride.Abstractions.Data.AuditLog.AuditBehaviour"/>Değişikliklerin kaydı sırasında audit loglarının oluşup oluşmamasını belirtmek
        /// için kullanılır.</param>
        /// <returns>Etkilenen kayıt sayısı</returns>
        int SaveChanges(AuditBehaviour auditBehaviour = AuditBehaviour.Enabled);
        /// <summary>
        /// Değişikliklerin kaydedilmesi için kullanılır.
        /// </summary>
        /// <param name="auditBehaviour"><seealso cref="Touride.Abstractions.Data.AuditLog.AuditBehaviour"/>Değişikliklerin kaydı sırasında audit loglarının oluşup oluşmamasını belirtmek
        /// için kullanılır.</param>
        /// <returns>Etkilenen kayıt sayısı</returns>
        Task<int> SaveChangesAsync(AuditBehaviour auditBehaviour = AuditBehaviour.Enabled, CancellationToken cancellationToken = new CancellationToken());
        /// <summary>
        /// Belirtilen modele uygun database oluşturulması için kullanılır.
        /// </summary>
        /// <returns>İşlem başarı durumu</returns>
        public bool CreateDatabase();
        /// <summary>
        /// Var olan databasein silinmesi için kullanılır.
        /// </summary>
        /// <returns></returns>
        public bool DeleteDatabase();
        /// <summary>
        /// Connection açmak için kullanılır. Otomatik connection açamayan Database ler için.(Örn:Sqlite)
        /// </summary>
        public void OpenConnection();
        /// <summary>
        /// Belirtilen modele uygun database oluşturulması için kullanılır.
        /// </summary>
        /// <returns>İşlem başarı durumu</returns>
        public Task<bool> CreateDatabaseAsync();
        /// <summary>
        /// Var olan databasein silinmesi için kullanılır.
        /// </summary>
        /// <returns></returns>
        public Task<bool> DeleteDatabaseAsync();
        /// <summary>
        /// Connection açmak için kullanılır. Otomatik connection açamayan Database ler için.(Örn:Sqlite)
        /// </summary>
        public Task OpenConnectionAsync();
        /// <summary>
        /// Auto migratşon
        /// </summary>
        public void AutoMigration();
    }
}
