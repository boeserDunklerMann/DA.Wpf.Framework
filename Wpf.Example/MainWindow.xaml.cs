using DA.Wpf.Framework;
using DA.Wpf.Framework.Attributes;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;

namespace Wpf.Example
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly ObservableCollection<IRibbonTabControl> _tabControls = [];

		public MainWindow()
		{

			InitializeComponent();
			rbnMain.Items.Clear();
		}

		public void SetTabControls(IEnumerable<IRibbonTabControl> tabControls)
		{
			// SORTIERUNG: Das ist der entscheidende Fix!
			// Wir sortieren alle Tabs basierend auf ihrer Position, bevor wir sie verarbeiten.
			var sortedTabs = tabControls.OrderBy(c => c.GetAttribute().Position).ToList();

			_tabControls.Clear();
			sortedTabs.ForEach(_tabControls.Add);
			rbnMain.Items.Clear();
			sortedTabs.ForEach(t =>
			{
				RibbonTabControlAttribute attr = t.GetAttribute();
				RibbonTab tab = new();

				// --- HIER DIE GRAFIK UND DEN TEXT DYNAMISCH IM CODE ZUSAMMENBAUEN ---

				// 1. StackPanel für die horizontale Anordnung (Icon links, Text rechts)
				var headerStack = new StackPanel { Orientation = Orientation.Horizontal };

				// 2. Wenn das Plugin eine Geometrie liefert, bauen wir das Icon
				if (!string.IsNullOrEmpty(t.IconGeometry))
				{
					try
					{
						// Versucht den String (z.B. "M10,20 L30,40...") in ein WPF Geometry-Objekt zu parsen
						var geometry = Geometry.Parse(t.IconGeometry);

						// Path-Element erstellen und konfigurieren
						var path = new System.Windows.Shapes.Path
						{
							Data = geometry,
							Stroke = System.Windows.Media.Brushes.DimGray, // Deine Wunschfarbe für die Kontur
							StrokeThickness = 1.5,
							StrokeStartLineCap = PenLineCap.Round,
							StrokeEndLineCap = PenLineCap.Round,
							Stretch = Stretch.Uniform
						};

						// Viewbox verpackt den Pfad, damit er sauber skaliert (z.B. 16x16 Pixel)
						var viewbox = new Viewbox
						{
							Width = 16,
							Height = 16,
							Margin = new Thickness(0, 0, 6, 0), // Kleiner Abstand nach rechts zum Text
							Child = path
						};

						headerStack.Children.Add(viewbox);
					}
					catch (Exception)
					{
						// Falls ein Plugin-Entwickler mal einen fehlerhaften XAML-String liefert,
						// fangen wir das ab, damit die App nicht abstürzt.
					}
				}

				// 3. Den Textblock für die Beschriftung hinzufügen
				var textBlock = new TextBlock
				{
					Text = attr.Heading,
					VerticalAlignment = VerticalAlignment.Center
				};
				headerStack.Children.Add(textBlock);

				// 4. Das gesamte StackPanel als Header setzen
				tab.Header = headerStack;

				rbnMain.Items.Add(tab);
			});
		}

		private void rbnMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Ribbon? ribbon = sender as Ribbon;
			if (ribbon != null && _tabControls.Count > 0)
			{
				grdMain.Children.Clear();
				var ctrl = _tabControls[ribbon.SelectedIndex] as UIElement;
				var ictrl = ctrl as IRibbonTabControl;
				if (ictrl != null)
				{
					ictrl.OnStart();
					grdMain.Children.Add(ctrl);
				}
				else
					throw new NullReferenceException(nameof(ictrl));
			}
		}
	}
}