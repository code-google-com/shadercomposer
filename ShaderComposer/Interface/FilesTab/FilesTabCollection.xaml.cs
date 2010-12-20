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

namespace ShaderComposer.Interface.FilesTab
{
    /// <summary>
    /// Interaction logic for FilesTabCollection.xaml
    /// </summary>
    public partial class FilesTabCollection : UserControl
    {
        public FilesTabCollection()
        {
            InitializeComponent();
        }

        //
        public IEnumerable<FilesTabItem> Items
        {
            get
            {
                return TabControl.Items.Cast<FilesTabItem>();
            }
        }

        public FilesTabItem SelectedItem
        {
            get
            {
                return TabControl.SelectedItem as FilesTabItem;
            }
        }

        //
        public FilesTabItem AddTab(File file)
        {
            FilesTabItem newTabItem = new FilesTabItem();
            newTabItem.Initialize(file);

            TabControl.Items.Add(newTabItem);

            TabControl.SelectedItem = newTabItem;

            return newTabItem;
        }

        public void RemoveTab(File file)
        {
            foreach (FilesTabItem filesTabItem in TabControl.Items.Cast<FilesTabItem>()) 
            {
                if (filesTabItem.File == file)
                {
                    TabControl.Items.Remove(filesTabItem);
                    break;
                }
            }
        }
    }
}
