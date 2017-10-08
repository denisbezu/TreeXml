using System;
using Microsoft.Win32;
using TreeWPF.Interfaces;

namespace TreeWPF.Helpers
{
    public class OpenSaveDialog : IOpenSaveDialog
    {
        public void OpenFile(Action<string> onOpenAction)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "Xml documents (.xml)|*.xml";
            if (openFileDialog.ShowDialog() == true)
            {
                onOpenAction(openFileDialog.FileName);
            }
        }

        public void SaveFile(Action<string> onSaveAction)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.FileName = "Tree";
            saveFileDialog.Filter = "Xml documents (.xml)|*.xml"; 
            if (saveFileDialog.ShowDialog() == true)
            {
                onSaveAction(saveFileDialog.FileName);
            }
        }
    }
}