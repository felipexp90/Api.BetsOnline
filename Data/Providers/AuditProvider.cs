using Audit.SqlServer.Providers;
using Entities.Enums;

namespace Data.Providers
{
    internal class AuditProvider : SqlDataProvider
    {
        public AuditProvider()
        {
            ConnectionString = ConfigurationEnum.BaseConnAudit;
            Schema = "dbo";
            TableName = "Event";
            IdColumnName = "EventId";
            JsonColumnName = "Data";
            LastUpdatedDateColumnName = "LastUpdatedDate";
        }
    }
}
