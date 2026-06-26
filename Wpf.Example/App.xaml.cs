using DA.SharedDeskPlanner.Model;
using DA.SharedDeskPlanner.Model.Contracts;
using DA.Wpf.Framework;
using DA.Wpf.Framework.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Asn1.Cms;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;

namespace Wpf.Example
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly List<IRibbonTabControl> tabControls = [];
		private readonly IServiceProvider serviceProvider;
		public App()
		{
#pragma warning disable 4014
			var services = new ServiceCollection();
			ConfigureServices(services);
			LoadAndInitPluginsAsync(services);
			serviceProvider = services.BuildServiceProvider();
#pragma warning restore 4014
		}

		private async Task LoadAndInitPluginsAsync(ServiceCollection services)
		{
			//var ctrls =  Common.GetRibbonTabControlsAsync().Result;
			var ctx = new SharedDeskPlannerContext();
			List<IRibbonTabControl> ctrls = [];
			await Task.CompletedTask;

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
				ctrl.OnInit(services);
				tabControls.Add(ctrl);
			}

		}
		private void ConfigureServices(IServiceCollection services)
		{
			services.AddScoped<ISharedDeskPlannerContext, SharedDeskPlannerContext>();
			services.AddTransient<MainWindow>();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
			mainWindow.SetTabControls(tabControls);
			mainWindow.Show();
		}
	}

}
