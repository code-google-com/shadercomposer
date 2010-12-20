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

namespace ShaderComposer.Interface.ShaderIO
{
    /// <summary>
    /// Interaction logic for ShaderOutput.xaml
    /// </summary>
    public partial class ShaderOutput : UserControl
    {
        public ShaderOutput()
        {
            InitializeComponent();
        }

        // Latest compiled source code
        private string sourceCode;
        public string SourceCode
        {
            get
            {
                return sourceCode;
            }

            set
            {
                sourceCode = value;

                TextEditor.Text = sourceCode;
            }
        }
        
        // Lates assembled source code
        // TODO
    }
}
