/// <summary>
/// 東山DBサーバに接続するための文字列を取得する。
/// </summary>
namespace Tozan.Server.ConnectionString
{
    public class GetConnectString
    {
        private GetConnectString() { }

        private static GetConnectString _instance = null;

        private static readonly object _lock = new object();

        public static GetConnectString GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new GetConnectString();

                        var databaseName = "tozandb_test";
#if DEBUG
                        databaseName = "tozandb";
#endif
                        // install library Microsoft.Extensions.Configuration and Microsoft.Extensions.Configuration.Json and Microsoft.AspNetCore.Mvc.NewtonsoftJson
                        var builder = new ConfigurationBuilder()
                                            .SetBasePath(Directory.GetCurrentDirectory())
                                            .AddJsonFile("appsettings.json", optional: false);
                        var configuration = builder.Build();
                        var connectionString = configuration.GetSection("connectionString").GetValue<string>(databaseName);
                        _instance.ConnectionString = connectionString;
                    }
                }
            }
            return _instance;
        }

        public string ConnectionString { get; set; }
    }
}
