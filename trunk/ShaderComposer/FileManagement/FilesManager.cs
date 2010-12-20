using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Win32;

using ShaderComposer.Interface.FilesTab;
using ShaderComposer.FileManagement;

namespace ShaderComposer.FileManagers
{
    public class FilesManager
    {
        // Singleton code
        private static FilesManager instance;

        public static FilesManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new FilesManager();

                return instance;
            }
        }

        private FilesManager()
        {
        }

        // Reference to the files tab collection
        public FilesTabCollection TabCollection { get; set; }
        
        // Reference to the currently active file
        public File ActiveFile {
            get
            {
                if (TabCollection.SelectedItem != null)
                {
                    return TabCollection.SelectedItem.File;
                } 
                else
                {
                    return null;
                }
            }
        }

        //
        public void New()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "untitled.scf";
            dialog.DefaultExt = ".scf";
            dialog.Filter = "Shader Composition file (.scf)|*.scf";

            if (dialog.ShowDialog() ?? false) {
                New(dialog.FileName);
            }
        }

        public void Open()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".scf";
            dialog.Filter = "Shader Composition file (.scf)|*.scf";
            dialog.Multiselect = true;

            if (dialog.ShowDialog() ?? false) {
                foreach (string fileName in dialog.FileNames) {
                    Open(fileName);
                }
            }
        }

        public void Save()
        {
            if (ActiveFile != null && ActiveFile.IsChanged)
            {
                ActiveFile.Save();
            }
        }

        public void SaveAs()
        {
            if (ActiveFile != null)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = ActiveFile.FilePath;
                dialog.DefaultExt = ".scf";
                dialog.Filter = "Shader Composition file (.scf)|*.scf";

                if (dialog.ShowDialog() ?? false) {
                    ActiveFile.SaveAs(dialog.FileName);
                }
            }
        }

        public void CloseAll()
        {
            File[] allFiles = TabCollection.Items.Select(x => x.File).ToArray();

            foreach (File file in allFiles)
            {
                Close(file);
            }
        }

        public void Close()
        {
            if (ActiveFile != null)
            {
                Close(ActiveFile);
            }
        }

        public void Close(File file)
        {
            if (file.IsChanged)
            {
                file.Save();
            }

            TabCollection.RemoveTab(file);
        }

        //
        private void New(string fileName)
        {
            File newFile = File.New(fileName);

            TabCollection.AddTab(newFile);
        }

        private void Open(string fileName)
        {
            File file = File.Open(fileName);

            TabCollection.AddTab(file);
        }

    }
}
