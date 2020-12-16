namespace Entities.Enums
{
    public class ConfigurationEnum
    {
        public static string BaseConn = "Server=tcp:databasesqlpocserver.database.windows.net,1433;Initial Catalog=Database-Sql-Poc;Persist Security Info=False;User ID=CloudAdmin;Password=Colombia1*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public static string BaseConnAudit = "Server=tcp:app-covid-dev.windows.net,1433;Initial Catalog=BetsOnlineDB_Audit;Persist Security Info=False;User ID=CloudAdmin;Password=Colombia3030*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    }
}