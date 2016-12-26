using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        private void createMapper(ExtensionMapper em)
        {
            Canvas canvas = createMapperComponent(maps.getList().IndexOf(em));
            extensionCanvas.Children.Add(canvas);
            EnlargeCanvas(extensionCanvas, 40);
        }

        private Canvas createMapperComponent(int index)
        {
            Canvas canvas = new Canvas();
            Border border = new Border();
            canvas.Width = 320;
            canvas.Height = 30;
            Canvas.SetLeft(border, 5);
            Canvas.SetTop(canvas, 10 + index * 40);
            return canvas;
        }

        private void AddExtensionMap(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveExtensionMap(object sender, RoutedEventArgs e)
        {

        }
    }
}
