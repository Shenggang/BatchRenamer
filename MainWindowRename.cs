using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BatchRenamer.RenameTools;

namespace BatchRenamer
{
    partial class MainWindow : Window
    {
        private void DoRename(object sender, RoutedEventArgs e)
        {
            if (fileList.Count == 0)
            {
                string message = "Please include file before renaming.";
                string caption = "No file included!";
                MessageBox.Show(message, caption, MessageBoxButton.OK);
                return;
            }
            BatchPrepForRenaming();
        }

        private void BatchPrepForRenaming()
        {
            foreach( NameComponent c in componentList)
            {
                c.InitialiseNeededValues(fileList.Count);
            }
        }
    }
}
