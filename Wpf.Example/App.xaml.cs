using DA.SharedDeskPlanner.Model;
using DA.SharedDeskPlanner.Model.Contracts;
using DA.Wpf.Framework;
using DA.Wpf.Framework.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Windows;

namespace Wpf.Example
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly ObservableCollection<IRibbonTabControl> tabControls = [];
		private IServiceProvider? serviceProvider;
		public App()
		{
		}

		private async Task LoadAndInitPluginsAsync(ServiceCollection services)
		{
			var cfg = GetConfiguration();
			services.AddSingleton<IConfiguration>(cfg);
			string? connString = cfg.GetConnectionString("default");
			if (connString == null)
				throw new NullReferenceException(nameof(connString));
		
			var ctx = new SharedDeskPlannerContext(connString);
			List<IRibbonTabControl> ctrls = [];
			//await Task.CompletedTask;

			// 1. Pfad zum bin-Verzeichnis ermitteln
			string binPath = AppContext.BaseDirectory;
			// 2. Alle .dll Dateien finden
			var dllFiles = Directory.GetFiles(binPath, "*.dll");
			var typesWithAttribute = new List<Type>();
			foreach (var file in dllFiles)
			{
				try
				{
					// 3. Assembly laden (Default Context nutzen)
					var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);

					// 4. Typen extrahieren, die das Attribut haben
					var matches = assembly.GetTypes()
						.Where(t => t.GetCustomAttribute<RibbonTabControlAttribute>() != null);

					typesWithAttribute.AddRange(matches);
				}
				catch (BadImageFormatException)
				{
					// Ignoriere Dateien, die keine validen .NET Assemblies sind (z.B. native DLLs)
				}
				catch (Exception)
				{
					throw;
				}
			}
			typesWithAttribute.ForEach(t =>
			{
				if (Activator.CreateInstance(t, ctx) is IRibbonTabControl ctrl)
					ctrls.Add(ctrl);
			});

			tabControls.Clear();
			for (int i = 0; i < ctrls.Count; i++)
			{
				IRibbonTabControl ctrl = ctrls[i];
				if (ctrl == null)
					throw new NullReferenceException(nameof(ctrl));
				await ctrl.OnInitAsync(services);
				tabControls.Add(ctrl);
			}

		}

		private IConfiguration GetConfiguration()
		{
			var cfgBuilder = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.local.json", false, true);
			return cfgBuilder.Build();
		}
		private void ConfigureServices(IServiceCollection services)
		{
			services.AddScoped<ISharedDeskPlannerContext, SharedDeskPlannerContext>();
			services.AddTransient<MainWindow>();
		}

		protected override async void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var services = new ServiceCollection();
			ConfigureServices(services);
			await LoadAndInitPluginsAsync(services);
			serviceProvider = services.BuildServiceProvider();

			var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
			mainWindow.SetTabControls(tabControls);
			mainWindow.Show();
		}
	}

}
