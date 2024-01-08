namespace Touride.Framework.Abstractions.Data.AuditProperties
{
    public interface IHasFullAudit : IHasCreateDate, IHasUpdateDate, IHasCreatedBy, IHasUpdatedBy, IHasCreatedAt, IHasUpdatedAt, IHasCreatedByUserCode, IHasUpdatedByUserCode
    {
    }
}
