using System;
using System.IO;
using System.Windows;

namespace BatchRenamer
{
    partial class MainWindow : Window
    {
        private void DoRename(object sender, RoutedEventArgs e)
        {
            if (fileList.Count == 0)
            {
                popNoFileIncludedWarning();
                return;
            }
            if (componentList.Count == 0)
            {
                popNoRuleSetWarning();
                return;
            }
            BatchPrepForRenaming();
            for (int i = 0; i< fileList.Count; i++)
            {
                MoveFile(i);   
            }
            listBox.Items.Refresh();
            MessageBox.Show("All Done unless notified otherwise.");
        }

        private void popNoFileIncludedWarning()
        {
            string message = "Please include file before renaming.";
            string caption = "No file included!";
            MessageBox.Show(message, caption, MessageBoxButton.OK);
        }

        private void popNoRuleSetWarning()
        {
            string message = "Please set rename rules!";
            string caption = "Rename rules not set!";
            MessageBox.Show(message, caption, MessageBoxButton.OK);
        }

        private void BatchPrepForRenaming()
        {
            componentList.InitialistRequiredFields(fileList.Count);
        }

        private void MoveFile(int i)
        {
            string oldName = fileList[i];
            string[] fileInfo = trisectDirectory(oldName);
            string newName = composeNameByIndex(fileInfo, i);
            try
            {
                File.Move(oldName, newName);
            }
            catch (IOException)
            {
                MessageBox.Show("File name duplicated.");
                return;
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Does not have the permission to move file: " + fileList[i]);
                return;
            }
            fileList[i] = newName;
        }

        private string composeNameByIndex(string[] fileInfo, int index)
        {
            string directory = fileInfo[0];
            string fileName = fileInfo[1];
            string extension = fileInfo[2];
            if (boxSelected[0])
            {
                fileName = componentList.ComposeNewName(index);
            }
            if (boxSelected[1])
            {
                extension = extensionMaps.getResultExtensoin(extension);
            }
            return directory + fileName + "." + extension;
        }

        private string[] trisectDirectory(string fullAddress)
        {
            //returns an array containing the parent directory, file name and its extension.
            string[] info = new string[3];
            int[] indexs = new int[2];
            indexs[0] = fullAddress.LastIndexOf('\\');
            indexs[1] = fullAddress.LastIndexOf('.');
            info[0] = fullAddress.Substring(0, indexs[0] + 1);
            if (indexs[1] != -1)
            {
                info[1] = fullAddress.Substring(indexs[0] + 1, indexs[1] - indexs[0] - 1);
                info[2] = fullAddress.Substring(indexs[1] + 1);
            } else
            {
                info[1] = fullAddress.Substring(indexs[0] + 1);
                info[2] = "";
            }
            return info;
        }
    }
}
