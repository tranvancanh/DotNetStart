using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebApi.Middleware
{
    public static class MyJpif
    {
        //public static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        //{
        //    var builder = new ServiceCollection()
        //        .AddLogging()
        //        .AddMvc()
        //        .AddNewtonsoftJson()
        //        .Services.BuildServiceProvider();

        //    return builder
        //        .GetRequiredService<IOptions<MvcOptions>>()
        //        .Value
        //        .InputFormatters
        //        .OfType<NewtonsoftJsonPatchInputFormatter>()
        //        .First();
        //}
    }
}
