using System ;
using Microsoft.AspNetCore.Hosting ;
using Microsoft.Extensions.Hosting ;

namespace youtube_subs
{
    public class Program
    {
        public static void Main (string[] args)
        {
            CreateHostBuilder (args).Build ().Run () ;
        }

        public static IHostBuilder CreateHostBuilder (string[] args) =>
            Host.CreateDefaultBuilder     (args)
                .ConfigureWebHostDefaults (webBuilder =>
                 {
                     webBuilder.UseStartup<Startup> ().UseUrls ("http://0.0.0.0:" + (Environment.GetEnvironmentVariable ("PORT") ?? "80")) ;
                 });
    }
}
