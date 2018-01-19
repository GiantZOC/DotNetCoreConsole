using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApplication3.Data;

namespace ConsoleApp1
{
	class Program
	{
		static void Main(string[] args) {
			Console.OutputEncoding = Encoding.UTF8;
			Console.WriteLine("Starting");
			//Get Settings and setup Services
			IServiceCollection serviceCollection = new ServiceCollection();
			ConfigureServices(serviceCollection);

			//Initialize the sample class
			SampleClass sample = new SampleClass(serviceCollection);
			Task.Run(async () => {
				//Do your stuff
				await sample.DoStuff();
			}).GetAwaiter().GetResult();
			Console.WriteLine("Complete");
		}

		private static void ConfigureServices(IServiceCollection serviceCollection) {

			string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			if (String.IsNullOrWhiteSpace(environment))
				throw new ArgumentNullException("Environment not found in ASPNETCORE_ENVIRONMENT");

			Console.WriteLine("Environment: {0}", environment);

			//Get settings from the json file
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true);
			if (environment == "Development") {

				builder
					.AddJsonFile(
						Path.Combine(AppContext.BaseDirectory, string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar), $"appsettings.{environment}.json"),
						optional: true
					);
			}
			else {
				builder
					.AddJsonFile($"appsettings.{environment}.json", optional: false);
			}

			var config = builder.Build();

			//bind json setting to object

			serviceCollection.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

			serviceCollection.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			ILoggerFactory loggerFactory = new Microsoft.Extensions.Logging.LoggerFactory();
			loggerFactory.AddConsole();
			loggerFactory.AddDebug();
			//loggerFactory.AddEventLog();
			serviceCollection.AddSingleton<ILogger>(loggerFactory.CreateLogger<Program>());
		}

		public class SampleClass
		{
			private readonly ILogger _logger;
			private readonly UserManager<IdentityUser> _userManager;
			public SampleClass(IServiceCollection serviceCollection) {
				IServiceProvider services = serviceCollection.BuildServiceProvider();
				_userManager = services.GetRequiredService<UserManager<IdentityUser>>();
				_logger = services.GetRequiredService<ILogger>();
			}

			public async Task DoStuff() {
				try {
					//DoStuff
					await LongRunningOperation();
				}
				catch (Exception ex) {
					_logger.LogError(ex.ToString());
				}
			}

			private static async Task<string> LongRunningOperation() {
				WebClient webClient = new WebClient();
				return await webClient.DownloadStringTaskAsync(new Uri("http://icanhazip.com"));

			}

		}
	}
}
