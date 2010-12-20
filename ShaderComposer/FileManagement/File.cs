using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using ShaderComposer.FileManagers;
using ShaderComposer.Interface.FileViewing;

namespace ShaderComposer.FileManagement
{
    public delegate void EventHandler(object sender, EventArgs e);

    public class File
    {
        public string FilePath { get; private set; }

        public string FileName
        {
            get
            {
                return Path.GetFileName(FilePath);
            }
        }

        public bool IsChanged { get; private set; }

        // Changed event
        public event EventHandler Changed;

        protected virtual void OnChanged(EventArgs e)
        {
            if (Changed != null)
                Changed(this, e);
        }

        // Saved event
        public event EventHandler Saved;

        protected virtual void OnSaved(EventArgs e)
        {
            if (Saved != null)
                Saved(this, e);
        }

        public static File New(string fileName)
        {
            File newFile = new File();
            newFile.FilePath = fileName;
            newFile.IsChanged = true;

            return newFile;
        }

        public static File Open(string fileName)
        {
            File file = new File();
            file.FilePath = fileName;
            file.IsChanged = false;

            return file;
        }

        // Save and SaveAs methods
        public void Save()
        {
            IsChanged = false;
            OnSaved(EventArgs.Empty);
        }

        public void SaveAs(string fileName)
        {
            FilePath = fileName;
            IsChanged = false;
            OnSaved(EventArgs.Empty);
        }

        // Close methods
        public void Close()
        {
            FilesManager.Instance.Close(this);
        }

        // Constructor
        private File()
        {
            // Create the initial file state
            RootState = ActiveState = new FileState(this);

        }

        // First file state
        public FileState RootState;

        // Currently active state
        private FileState activeState;
        public FileState ActiveState
        {
            get
            {
                return activeState;
            }

            set
            {
                if (activeState != null)
                {
                    activeState.stop();
                }

                activeState = value;

                activeState.start();
            }
        }

        // File Viewer
        private FileView fileView;
        public FileView FileView
        {
            get
            {
                return fileView;
            }

            set
            {
                fileView = value;

                FileView.VisualTrail.AddStateNode(ActiveState);
            }
        }


        // Add a new state
        public void NewState()
        {
            ActiveState = new FileState(ActiveState);

            FileView.VisualTrail.AddStateNode(ActiveState);
        }
    }
}
