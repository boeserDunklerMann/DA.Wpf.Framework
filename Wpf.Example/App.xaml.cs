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
	/// <ChangeLog>
	/// <Create Datum="??.??.2026" Entwickler="DA" />
	/// <Change Datum="26.06.2026" Entwickler="DA">Added many DI stuff</Change>	
	/// </ChangeLog>
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly ObservableCollection<IRibbonTabControl> tabControls = [];
		private IServiceProvider? serviceProvider;

		private async Task LoadAndInitPluginsAsync(ServiceCollection services)
		{
			var cfg = GetConfiguration();
			services.AddSingleton(cfg);
			string? connString = cfg.GetConnectionString("default");
			if (connString == null)
				throw new NullReferenceException(nameof(connString));

			services.AddScoped<ISharedDeskPlannerContext>(provider =>
			{
				return new SharedDeskPlannerContext(connString);
			});

			services.AddSingleton<IDialogService, DialogService>();

			List<IRibbonTabControl> ctrls = [];

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
				// t im DI Container registrieren
				services.AddTransient(t);
				//ctrls.Add(ctrl);
			});
			serviceProvider = services.BuildServiceProvider();
			typesWithAttribute.ForEach(t =>
			{
				// Der Provider weiß jetzt, dass er für 't' auch IDialogService mitliefern muss!
				if (serviceProvider.GetRequiredService(t) is IRibbonTabControl ctrl)
				{
					ctrls.Add(ctrl);
				}
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
			services.AddTransient<MainWindow>();
			services.AddSingleton<IDialogService, DialogService>();
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