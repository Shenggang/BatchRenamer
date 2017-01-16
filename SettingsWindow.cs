using System.Windows;
using System.Windows.Controls;
using BatchRenamer.RenameTools;

namespace BatchRenamer
{
    public partial class SettingsWindow:Window
    {
        private ExtensionMapperList maps;

        private void CreateExtensionMaps()
        {
            foreach( ExtensionMapper em in maps.getList())
            {
                createMapper(em);
            }
        }

        public ExtensionMapperList ExtensionMapperList
        {
            get { return maps; }
        }

        private void createMapper(ExtensionMapper em)
        {
            Canvas canvas = createMapperComponent(maps.getList().IndexOf(em));
            extensionCanvas.Children.Add(canvas);

            TextBox sourceBox = new TextBox();
            Label sourceLabel = new Label();
            sourceLabel.Content = "Src. Ext.";
            sourceBox.Width = 100;
            sourceBox.TextChanged += SourceBox_TextChanged;

            TextBox resultBox = new TextBox();
            Label resultLabel = new Label();
            resultLabel.Content = " →  Rst. Ext.";
            resultBox.Width = 60;
            resultBox.TextChanged += ResultBox_TextChanged;

            AddComponentsToCanvas(canvas, sourceBox, sourceLabel);
            AddComponentsToCanvas(canvas, resultBox, resultLabel);
            Canvas.SetLeft(sourceLabel, 5);
            Canvas.SetLeft(sourceBox, 65);
            Canvas.SetLeft(resultLabel, 170);
            Canvas.SetLeft(resultBox, 260);
            initialiseTextBox(sourceBox, em.InputExtensions);
            initialiseTextBox(resultBox, em.OutputExtension);
            EnlargeCanvas(extensionCanvas, 40);
        }

        private void SourceBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int index = extensionCanvas.Children.IndexOf((UIElement)tb.Parent);
            ExtensionMapper mapper = maps.getList()[index];
            mapper.InputExtensions = tb.Text;
        }

        private void ResultBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int index = extensionCanvas.Children.IndexOf((UIElement)tb.Parent);
            ExtensionMapper mapper = maps.getList()[index];
            mapper.OutputExtension = tb.Text;
        }

        private void AddComponentsToCanvas(Canvas canvas, TextBox tb, Label l)
        {
            canvas.Children.Add(tb);
            canvas.Children.Add(l);
            Canvas.SetTop(tb, 5);
        }

        private Canvas createMapperComponent(int index)
        {
            Canvas canvas = new Canvas();
            Border border = addInBorder(360, 35);
            canvas.Width = 360;
            canvas.Height = 40;
            Canvas.SetTop(canvas, 5 + index * 40);
            Canvas.SetTop(border, 0);
            canvas.Children.Add(border);
            return canvas;
        }

        private void AddExtensionMap(object sender, RoutedEventArgs e)
        {
            ExtensionMapper em = new ExtensionMapper();
            maps.add(em);
            createMapper(em);
        }

        private void RemoveExtensionMap(object sender, RoutedEventArgs e)
        {
            maps.removeTheLast();
            extensionCanvas.Children.RemoveAt(extensionCanvas.Children.Count - 1);
            EnlargeCanvas(extensionCanvas, -40);
        }
    }
}
