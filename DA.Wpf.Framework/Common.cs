using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace DA.Wpf.Framework
{
	/// <ChangeLog>
	/// <Create Datum="08.05.2026" Entwickler="DA" />
	/// </ChangeLog>
	public static class Common
	{
		public async static Task<List<IRibbonTabControl>> GetRibbonTabControlsAsync()
		{
			List<IRibbonTabControl> controls = [];
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
						.Where(t => t.GetCustomAttribute<Attributes.RibbonTabControlAttribute>() != null);

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
				if (Activator.CreateInstance(t) is IRibbonTabControl ctrl)
					controls.Add(ctrl);
			});
			return controls;
		}
	}
}