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
using ShaderComposer.FileManagers;

namespace ShaderComposer.Interface.Toolbars
{
    /// <summary>
    /// Interaction logic for StandardToolbar.xaml
    /// </summary>
    public partial class StandardToolbar : ToolBar
    {
        public StandardToolbar()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (FilesManager.Instance.ActiveFile != null)
            {
                FilesManager.Instance.ActiveFile.ActiveState.Build();
                FilesManager.Instance.ActiveFile.ActiveState.BuildXML();
            }
        }

    }
}
