﻿#pragma checksum "..\..\..\..\..\Interface\Designer\DesignArea.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2295149649C1D04DA5472DD3E9EC4DE9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ShaderComposer.Interface.Designer;
using ShaderComposer.Interface.Designer.Canvas;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ShaderComposer.Interface.Designer {
    
    
    /// <summary>
    /// DesignArea
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class DesignArea : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\..\..\..\Interface\Designer\DesignArea.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer ScrollViewer;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\..\Interface\Designer\DesignArea.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Viewbox Viewbox;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\..\Interface\Designer\DesignArea.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ShaderComposer.Interface.Designer.Canvas.DynamicCanvas Canvas;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\..\Interface\Designer\DesignArea.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle CanvasBackground;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ShaderComposer;component/interface/designer/designarea.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Interface\Designer\DesignArea.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ScrollViewer = ((System.Windows.Controls.ScrollViewer)(target));
            
            #line 13 "..\..\..\..\..\Interface\Designer\DesignArea.xaml"
            this.ScrollViewer.SizeChanged += new System.Windows.SizeChangedEventHandler(this.ScrollViewer_SizeChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Viewbox = ((System.Windows.Controls.Viewbox)(target));
            
            #line 14 "..\..\..\..\..\Interface\Designer\DesignArea.xaml"
            this.Viewbox.SizeChanged += new System.Windows.SizeChangedEventHandler(this.Viewbox_SizeChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Canvas = ((ShaderComposer.Interface.Designer.Canvas.DynamicCanvas)(target));
            return;
            case 4:
            this.CanvasBackground = ((System.Windows.Shapes.Rectangle)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

