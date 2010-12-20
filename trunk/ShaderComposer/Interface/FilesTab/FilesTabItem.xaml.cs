using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ShaderComposer.FileManagement;
using ShaderComposer.Interface.FileViewing;

namespace ShaderComposer.Interface.FilesTab
{
    /// <summary>
    /// Interaction logic for FilesTabItem.xaml
    /// </summary>
    public partial class FilesTabItem : TabItem
    {
        public FilesTabItem()
        {
            InitializeComponent();
        }

        //
        public File File { get; private set; }

        public void Initialize(File file)
        {
            File = file;
            File.Saved += new FileManagement.EventHandler(Saved);
            File.Changed += new FileManagement.EventHandler(Changed);

            Header = File.FileName;

            FileView fileView = new FileView();
            fileView.Initialize(file);

            Content = fileView;
        }

        // 
        void Saved(object sender, EventArgs e)
        {
            Header = File.FileName;
        }

        void Changed(object sender, EventArgs e)
        {
            Header = File.FileName + "*";
        }

        //
        private void Close(object sender, RoutedEventArgs e)
        {
            File.Close();
        }
    }
}
