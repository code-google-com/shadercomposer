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
using ShaderComposer.Libraries;
using ShaderComposer.FileManagers;
using ShaderComposer.Interface.FilesTab;

namespace ShaderComposer.Interface.Toolbars
{
    /// <summary>
    /// Interaction logic for NodesToolbar.xaml
    /// </summary>
    public partial class NodesToolbar : ToolBar
    {
        public NodesToolbar()
        {
            InitializeComponent();

            // Register for library added events
            LibraryManager.Instance.LibraryAdded += new LibraryAddedHandler(LibraryAdded);
        }

        //
        private Dictionary<MenuItem, Type> nodeTypes = new Dictionary<MenuItem, Type>();

        void LibraryAdded(object sender, ILibrary library)
        {
            Menu libraryMenu = new Menu();
            ToolBar.SetOverflowMode(libraryMenu, OverflowMode.AsNeeded);
            libraryMenu.Background = new SolidColorBrush(Color.FromRgb((byte)0xBC, (byte)0xC7, (byte)0xD8));
            Items.Add(libraryMenu);

            MenuItem libraryMenuItem = new MenuItem();
            libraryMenuItem.Header = library.GetName();
            libraryMenu.Items.Add(libraryMenuItem);

            foreach (Type node in library.GetNodeTypes())
            {
                MenuItem libraryNodeItem = new MenuItem();
                libraryNodeItem.Header = node.Name;

                nodeTypes[libraryNodeItem] = node;
                libraryNodeItem.Click += new RoutedEventHandler(libraryNodeItem_Click);
                
                libraryMenuItem.Items.Add(libraryNodeItem);
            }
        }

        // Add a new node to the graph
        void libraryNodeItem_Click(object sender, RoutedEventArgs e)
        {
            Type node = nodeTypes[sender as MenuItem];

            if (FilesManager.Instance.ActiveFile != null)
            {
                FilesManager.Instance.ActiveFile.ActiveState.AddNewNode(node, new Point(0, 0));
            }
        }
    }
}
