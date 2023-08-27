namespace WebApi.Commons
{
    public class ServerPath
    {
        public readonly string _Server_Path;
        private const string DEVELOPMENT_MODE = "https://localhost:7161/reactjs";
        private const string PRODUCTION_MODE = "https://www.tozan.co.jp/reactjs";

        public ServerPath(IWebHostEnvironment hostingEnvironment)
        {
            _Server_Path = Configure(hostingEnvironment);
        }

        public string Configure(IWebHostEnvironment env)
        {
            var serverPath = string.Empty;
            if (env.IsEnvironment("Development"))
            {
                // code to be executed in development environment 
                serverPath = DEVELOPMENT_MODE;
            }
            else if (env.IsStaging())
            {
                // code to be executed in staging environment 
                serverPath = PRODUCTION_MODE;
            }
            else
            {
                // env.IsProduction()
                // code to be executed in production environment 
                serverPath = PRODUCTION_MODE;
            }

            return serverPath;
        }
    }
}
