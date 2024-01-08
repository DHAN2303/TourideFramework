namespace Touride.Framework.Abstractions.Data.Entities
{
    /// <summary>
    /// Lookup entityleri için base class
    /// </summary>
    public abstract class LookUpEntityBase : ILookUpEntity
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }
}
