namespace Touride.Framework.Abstractions.Data.Entities
{
    /// <summary>
    /// Long Id ye sahip olan entityler için kullanılır.
    /// </summary>
    public interface IHasId : IEntity
    {
        Guid Id { get; set; }
    }
}
