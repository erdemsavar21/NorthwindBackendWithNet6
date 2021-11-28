namespace Northwind.DataAccess.Settings
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; set; }
    }
}