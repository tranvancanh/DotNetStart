
#define TEST   //環境設定

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

                        var databaseName = string.Empty;
#if TEST
            databaseName = "tozandb_test"; //TEST環境
#elif UAT
            databaseName = "tozandb_1stUAT"; //UAT環境
#elif DEV
            databaseName = "tozandb_Dev"; //開発環境
#else
                        databaseName = "tozandb"; //本番環境
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
