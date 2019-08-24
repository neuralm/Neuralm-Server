namespace Neuralm.Application.Configurations
{
    /// <summary>
    /// The database configuration class which holds the connection string.
    /// </summary>
    public class DbConfiguration
    {
        public string ConnectionString { get; set; }
        public bool UseLazyLoading { get; set; }
        public string DbProvider { get; set; }
    }
}
