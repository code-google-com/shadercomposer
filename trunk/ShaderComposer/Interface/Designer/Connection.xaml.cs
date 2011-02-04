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
using ShaderComposer.Interface.Designer.Canvas;
using System.ComponentModel;
using ShaderComposer.Interface.Designer.Variables;

namespace ShaderComposer.Interface.Designer
{
    /// <summary>
    /// Interaction logic for Connection.xaml
    /// </summary>
    public partial class Connection : UserControl
    {
        public Connection()
        {
            InitializeComponent();

            PreviewWindow.path = path;
            PreviewWindow.parent = this;
        }

        // DesignArea that this connection belongs to
        public DesignArea DesignArea { get; set; }

        // Event listeners for the DynamicCanvas.Left and DynamicCanvas.Top properties on the connected nodes
        // these ensure that the connection curve is updated whenever the parent nodes move.
        private static DependencyPropertyDescriptor dpdLeft = DependencyPropertyDescriptor.FromProperty(DynamicCanvas.LeftProperty, typeof(DynamicCanvas));
        private static DependencyPropertyDescriptor dpdTop = DependencyPropertyDescriptor.FromProperty(DynamicCanvas.TopProperty, typeof(DynamicCanvas));

        private EventHandler outputVariableHandler;
        private SizeChangedEventHandler outputVariableSizeHandler;
        private Variable outputVariable;
        public Variable OutputVariable
        {
            get
            {
                return outputVariable;
            }

            set
            {
                if (outputVariable != null)
                {
                    // Remove event listeners
                    dpdLeft.RemoveValueChanged(outputVariable.Node, outputVariableHandler);
                    dpdTop.RemoveValueChanged(outputVariable.Node, outputVariableHandler);
                    outputVariable.Node.SizeChanged -= outputVariableSizeHandler;

                    // Notify node
                    outputVariable.RemoveLink(this);
                }

                outputVariable = value;

                if (outputVariable != null)
                {
                    // Add event listeners
                    outputVariableHandler = new EventHandler(pointChangedHandler);
                    outputVariableSizeHandler = new SizeChangedEventHandler(pointChangedSizeHandler);
                    dpdLeft.AddValueChanged(outputVariable.Node, outputVariableHandler);
                    dpdTop.AddValueChanged(outputVariable.Node, outputVariableHandler);
                    outputVariable.Node.SizeChanged += outputVariableSizeHandler;

                    // Notify connecting node
                    outputVariable.AddLink(this);

                    // Update curve position
                    if (DesignArea != null)
                    {
                        updateCurve();
                    }
                }
            }
        }

        private EventHandler inputVariableHandler;
        private SizeChangedEventHandler inputVariableSizeHandler;
        private Variable inputVariable;
        public Variable InputVariable
        {
            get
            {
                return inputVariable;
            }

            set
            {
                if (inputVariable != null)
                {
                    // Remove event listeners
                    dpdLeft.RemoveValueChanged(inputVariable.Node, inputVariableHandler);
                    dpdTop.RemoveValueChanged(inputVariable.Node, inputVariableHandler);
                    inputVariable.Node.SizeChanged -= inputVariableSizeHandler;

                    // Notify node
                    inputVariable.RemoveLink(this);
                }

                inputVariable = value;

                if (inputVariable != null)
                {
                    // Add event listeners
                    inputVariableHandler = new EventHandler(pointChangedHandler);
                    inputVariableSizeHandler = new SizeChangedEventHandler(pointChangedSizeHandler);
                    dpdLeft.AddValueChanged(inputVariable.Node, inputVariableHandler);
                    dpdTop.AddValueChanged(inputVariable.Node, inputVariableHandler);

                    inputVariable.AddLink(this);

                    // Update curve position
                    if (DesignArea != null)
                    {
                        updateCurve();
                    }
                }
            }
        }

        // Coordinates of start and end point of the connection
        // these coordinates are relative to the design canvas
        private Point StartPoint
        {
            get 
            {
                return Point.Add(outputVariable.TranslatePoint(outputVariable.LinkPoint, Parent as DynamicCanvas), new Vector(5, 0));
            }
        }

        private Point EndPoint
        {
            get
            {
                if (inputVariable == null)
                    return Mouse.GetPosition(DesignArea.Canvas);
                else
                    return Point.Add(inputVariable.TranslatePoint(inputVariable.LinkPoint, Parent as DynamicCanvas), new Vector(-5, 0));
            }
        }

        // Updates the position of the connection curve whenever the connecting nodes change their position
        public void pointChangedHandler(object sender, EventArgs e)
        {
            updateCurve();
        }

        public void pointChangedSizeHandler(object sender, SizeChangedEventArgs e)
        {
            updateCurve();
        }

        private void updateCurve()
        {
            // Update visibility of the curve start and end points
            start.RadiusX = (outputVariable == null ? 2 : 0);
            start.RadiusY = (outputVariable == null ? 2 : 0);

            end.RadiusX = (inputVariable == null ? 2 : 0);
            end.RadiusY = (inputVariable == null ? 2 : 0);

            // Update curve between start and end points
            if (outputVariable != null)
            {
                Point middlePoint1 = new Point(StartPoint.X * 0.5 + EndPoint.X * 0.5, StartPoint.Y);
                Point middlePoint2 = new Point(StartPoint.X * 0.5 + EndPoint.X * 0.5, EndPoint.Y);

                start.Center = StartPoint;
                line.StartPoint = StartPoint;
                bezier.Point1 = middlePoint1;
                bezier.Point2 = middlePoint2;
                bezier.Point3 = EndPoint;
                end.Center = EndPoint;

                // Update position of preview window
                PreviewWindow.Margin = new Thickness(StartPoint.X * 0.5 + EndPoint.X * 0.5 - 64.0, StartPoint.Y * 0.5 + EndPoint.Y * 0.5 - 64.0, 0, 0);

                DesignArea.UpdateLayout();
            }
        }

        // Start creating a new connection
        private MouseEventHandler creationMouseMoveListener;
        private MouseButtonEventHandler creationMouseUpListener;

        public static void startCreation(Variable outputVariable)
        {
            DesignArea designArea = outputVariable.DesignArea;

            Connection newConnection = new Connection();
            newConnection.DesignArea = designArea;
            newConnection.OutputVariable = outputVariable;

            designArea.AddConnection(newConnection);

            DynamicCanvas.SetZIndex(newConnection, designArea.getHighestZIndex());

            // Update curve position when the mouse is moved
            newConnection.creationMouseMoveListener = new MouseEventHandler((x, y) => dragCreation(newConnection, y));
            newConnection.creationMouseUpListener = new MouseButtonEventHandler((x, y) => dragCreation(newConnection, y));
            designArea.MouseMove += newConnection.creationMouseMoveListener;
            designArea.MouseUp += newConnection.creationMouseUpListener;
        }

        public void breakConnection()
        {
            InputVariable = null;

            // Update curve position when the mouse is moved
            creationMouseMoveListener = new MouseEventHandler((x, y) => dragCreation(this, y));
            creationMouseUpListener = new MouseButtonEventHandler((x, y) => dragCreation(this, y));
            DesignArea.MouseMove += creationMouseMoveListener;
            DesignArea.MouseUp += creationMouseUpListener;
        }

        public void removeConnection()
        {
            InputVariable = null;
            OutputVariable = null;

            if (DesignArea != null)
            {
                DesignArea.RemoveConnection(this);
            }
        }

        private static void dragCreation(Connection newConnection, MouseEventArgs e) {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Update the connection curve
                newConnection.updateCurve();

                newConnection.DesignArea.Canvas.UpdateLayout();
            }
            else
            {
                // Finalize creation
                endCreation(newConnection);
            }
        }

        private static void endCreation(Connection newConnection)
        {
            DesignArea designArea = newConnection.DesignArea;

            // Remove the mouse move and mouse up listeners
            designArea.MouseMove -= newConnection.creationMouseMoveListener;
            designArea.MouseUp -= newConnection.creationMouseUpListener;

            // Check if the mouse is currently intersecting with a input variable
            HitTestFilterCallback filterCallback = new HitTestFilterCallback(element => element == newConnection ? HitTestFilterBehavior.ContinueSkipSelfAndChildren : HitTestFilterBehavior.Continue);

            HitTestResultCallback resultCallback = new HitTestResultCallback(element => finalizeCreation(element, newConnection));

            VisualTreeHelper.HitTest(designArea.Canvas, filterCallback, resultCallback, new PointHitTestParameters(newConnection.EndPoint));
        }

        private static HitTestResultBehavior finalizeCreation(HitTestResult hitTestResult, Connection newConnection)
        {
            // Try to find the variable parent
            Variable inputVariable = null;

            if (hitTestResult.VisualHit is Ellipse)
            {
                Ellipse ellipse = hitTestResult.VisualHit as Ellipse;

                DependencyObject parent = VisualTreeHelper.GetParent(ellipse);

                while(!(parent is DesignArea))
                {
                    if (parent is Variable) 
                    {
                        Variable variable = parent as Variable;

                        if (variable.Type == Variable.VariableType.Input && variable.GetLinks().Count == 0)
                        {
                            inputVariable = variable;
                        }

                        break;
                    }

                    parent = VisualTreeHelper.GetParent(parent);
                }
            }

            if (inputVariable != null)
            {
                // Finalize the connection
                newConnection.InputVariable = inputVariable;
            }
            else
            {
                // Destroy the connection
                newConnection.OutputVariable = null;

                newConnection.DesignArea.RemoveConnection(newConnection);
            }

            return HitTestResultBehavior.Stop;
        }

        private void MouseEnter(object sender, MouseEventArgs e)
        {
            updatePreviewWindow();
        }

        private void MouseLeave(object sender, MouseEventArgs e)
        {
            updatePreviewWindow();
        }

        private void MouseUp(object sender, MouseButtonEventArgs e)
        {
            PreviewWindow.Pinned = !PreviewWindow.Pinned;

            updatePreviewWindow();
        }

        private void updatePreviewWindow()
        {
            bool mouseOverPath = Mouse.DirectlyOver == path;
            bool mouseOverPreview = (Mouse.DirectlyOver == PreviewWindow.PreviewGrid || Mouse.DirectlyOver == PreviewWindow.PreviewBorder || Mouse.DirectlyOver == PreviewWindow.PreviewImage || Mouse.DirectlyOver == PreviewWindow.PinImage);

            if ((mouseOverPath || mouseOverPreview) && InputVariable != null)
                PreviewWindow.Visibility = Visibility.Visible;
            else
                PreviewWindow.Visibility = Visibility.Collapsed;

            if (PreviewWindow.Pinned)
                PreviewWindow.Visibility = Visibility.Visible;

            PreviewWindow.Opacity = PreviewWindow.Pinned ? 1.0 : 0.6;
        }
    }
}
