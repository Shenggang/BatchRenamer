using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using BatchRenamer.RenameTools;

namespace BatchRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<String> fileList = new List<string>();
        private List<NameComponent> componentList = new List<NameComponent>();
        private ExtensionMapperList extensionMaps = new ExtensionMapperList();
        private bool[] boxSelected = { false, false };

        private SettingsWindow settings;

        public MainWindow()
        {
            InitializeComponent();
            MakeSettings();
        }

        private void MakeSettings()
        {
            listBox.SelectionMode = SelectionMode.Extended;
        }

        private void AddFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.FileName = "";
            dialog.DefaultExt = ".*";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                AddInNames(dialog.FileNames);
            }
            listBox.ItemsSource = fileList;
            listBox.Items.Refresh();
        }

        private void AddInNames(string[] filename)
        {
            bool[] hasDuplicate = new bool[filename.Length];
            for (int i = 0; i < filename.Length; i++)
            {
                hasDuplicate[i] = false;
                if (fileList != null)
                {
                    hasDuplicate[i] = fileList.Contains(filename[i]);
                }
            }
            for (int i = 0; i < filename.Length; i++)
            {
                if (!hasDuplicate[i])
                {
                    fileList.Add(filename[i]);
                }
            }
        }

        private void OpenRenameRules(object sender, RoutedEventArgs e)
        {
            settings = new SettingsWindow(componentList, extensionMaps, boxSelected);
            settings.Show();
            settings.Closed += getSettings;
        }

        private void getSettings(object sender, EventArgs e)
        {
            SettingsWindow settings = (SettingsWindow)sender;
            componentList = settings.ComponentList;
            extensionMaps = settings.ExtensionMapperList;
            boxSelected = settings.BoxSelected;
        }

        private void MoveUp(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                int[] indices = getIndices(listBox.SelectedItems);
                int bound = 0;
                for (int i = 0; i<indices.Length; i++)
                {
                    int index = indices[i];
                    if (indices[i] > bound)
                    {
                        string s = fileList[index - 1];
                        fileList[index - 1] = fileList[index];
                        fileList[index] = s;
                        index--;
                    }
                    bound = index + 1;
                }
                listBox.Items.Refresh();
            }
        }

        private void MoveDown(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                int[] indices = getIndices(listBox.SelectedItems);
                int bound = listBox.Items.Count - 1;
                for (int i = indices.Length - 1; i >= 0; i--)
                {
                    int index = indices[i];
                    if (indices[i] < bound)
                    {
                        string s = fileList[index + 1];
                        fileList[index + 1] = fileList[index];
                        fileList[index] = s;
                        index++;
                    }
                    bound = index - 1;
                }
                listBox.Items.Refresh();
            }
        }

        private int[] getIndices(IList list)
        {
            int[] indices = new int[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                indices[i] = fileList.FindIndex(x => x.Equals(list[i]));
            }
            return bubbleSort(indices);
        }

        private int[] bubbleSort(int[] list)
        {
            int length = list.Length;
            for (int i = 0; i < length; i++)
            {
                for (int j = i; j < length; j++)
                {
                    if (list[i] > list[j])
                    {
                        int t = list[i];
                        list[i] = list[j];
                        list[j] = t;
                    }
                }
            }
            return list;
        }

        private void AccendSort(object sender, RoutedEventArgs e)
        {
            fileList.Sort();
            listBox.Items.Refresh();
        }

        private void DecendSort(object sender, RoutedEventArgs e)
        {
            fileList.Sort();
            fileList.Reverse();
            listBox.Items.Refresh();
        }

        private void DeleteItems(object sender, RoutedEventArgs e)
        {
            while (listBox.SelectedIndex != -1)
            {
                System.Diagnostics.Debug.WriteLine(listBox.SelectedIndex);
                fileList.RemoveAt(listBox.SelectedIndex);
                listBox.Items.Refresh();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (settings != null)
            {
                if (settings.IsLoaded)
                {
                    settings.Close();
                }
            }
        }
    }
}
