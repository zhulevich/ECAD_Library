using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using ECAD_Library.Models;

namespace ECAD_Library.Controls
{
    public partial class CanvasControl : Canvas
    {
        private Point _lastMousePosition;
        private bool _isPanning;
        private ScaleTransform _scaleTransform;
        private TranslateTransform _translateTransform;
        private TransformGroup _transformGroup;
        private Control? _selectedElement;
        private Button? _startConnector;
        private Polyline? _tempPolyline;
        private bool _isDrawingConnection = false;
        private CanvasItem? _startItem;


        public CanvasControl()
        {
            DragDrop.SetAllowDrop(this, true);
            AddHandler(DragDrop.DropEvent, OnDrop);

            _scaleTransform = new ScaleTransform(1, 1);
            _translateTransform = new TranslateTransform();
            _transformGroup = new TransformGroup();
            _transformGroup.Children.Add(_scaleTransform);
            _transformGroup.Children.Add(_translateTransform);
            RenderTransform = _transformGroup;

            PointerWheelChanged += OnWheelChanged;
            PointerPressed += OnPointerPressed;
            PointerMoved += OnPointerMoved;
            PointerReleased += OnPointerReleased;
        }

        public void HandleConnectionClick(Button btn, Point localPoint, CanvasItem item)
        {
            var itemLeft = Canvas.GetLeft(item);
            var itemTop = Canvas.GetTop(item);

            var globalPoint = new Point(itemLeft + localPoint.X, itemTop + localPoint.Y);

            if (!_isDrawingConnection)
            {
                StartConnectionFrom(btn, globalPoint);
                _startConnector = btn;
                _startItem = item;
            }
            else
            {
                if (_startConnector != btn) 
                {
                    TryCompleteConnection(btn, globalPoint);
                }

                _startConnector = null;
                _startItem = null;
                _isDrawingConnection = false;
            }
        }
        private void OnDrop(object? sender, DragEventArgs e)
        {
            if (sender is Canvas canvas && e.Data.Contains("PaletteItem"))
            {
                if (e.Data.Get("PaletteItem") is PalleteItem item)
                {
                    var canvasItem = new CanvasItem
                    {
                        Icon = item.Icon,
                        Name = item.Name,
                        ParentCanvas = this
                    };

                    canvasItem.ConnectionPoints = item.ConnectionPoints; 
                    var position = e.GetPosition(canvas);
                    Canvas.SetLeft(canvasItem, position.X);
                    Canvas.SetTop(canvasItem, position.Y);

                    canvas.Children.Add(canvasItem);
                }
            }
        }
        private void OnWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            const double zoomFactor = 1.1;
            var scale = e.Delta.Y > 0 ? zoomFactor : 1 / zoomFactor;

            var cursorPosition = e.GetPosition(this);
            var relativeX = cursorPosition.X / Bounds.Width;
            var relativeY = cursorPosition.Y / Bounds.Height;

            _scaleTransform.ScaleX *= scale;
            _scaleTransform.ScaleY *= scale;

            _translateTransform.X = (1 - scale) * Bounds.Width * relativeX + _translateTransform.X * scale;
            _translateTransform.Y = (1 - scale) * Bounds.Height * relativeY + _translateTransform.Y * scale;
        }

        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var point = e.GetCurrentPoint(this);

            if (point.Properties.IsLeftButtonPressed && !_isPanning)
            {

                var hitTestResult = this.InputHitTest(point.Position);
                if (hitTestResult is Control control)
                {

                    while (control is not null && control is not CanvasItem)
                    {
                        control = control.Parent as Control;
                    }

                    if (control is CanvasItem canvasItem)
                    {
                        _selectedElement = canvasItem;
                        _lastMousePosition = point.Position;
                    }
                }
            }

            if (point.Properties.IsLeftButtonPressed)
            {
                _isPanning = true;
                _lastMousePosition = e.GetPosition(this);
            }
        }


        private void OnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (_isDrawingConnection && _tempPolyline != null)
            {
                var start = _tempPolyline.Points[0];
                var current = e.GetPosition(this);

                _tempPolyline.Points[1] = new Point(current.X, start.Y);
                _tempPolyline.Points[2] = current;
            }
            if (_selectedElement != null)
            {

                var currentPosition = e.GetPosition(this);


                double left = Canvas.GetLeft(_selectedElement);
                double top = Canvas.GetTop(_selectedElement);

                Canvas.SetLeft(_selectedElement, left + (currentPosition.X - _lastMousePosition.X));
                Canvas.SetTop(_selectedElement, top + (currentPosition.Y - _lastMousePosition.Y));

                _lastMousePosition = currentPosition;
            }
            else if (_isPanning)
            {
                var currentPosition = e.GetPosition(this);
                var delta = currentPosition - _lastMousePosition;

                _translateTransform.X += delta.X;
                _translateTransform.Y += delta.Y;

                _lastMousePosition = currentPosition;
            }

        }

        private void TryCompleteConnection(Button btn, Point globalPoint)
        {
            if (_tempPolyline != null)
            {
                var start = _tempPolyline.Points[0];

                _tempPolyline.Points[1] = new Point(globalPoint.X, start.Y);
                _tempPolyline.Points[2] = globalPoint;

                _tempPolyline = null;
            }
        }
        private void StartConnectionFrom(Button btn, Point globalPoint)
        {
            _tempPolyline = new Polyline
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Points = new Points
        {
            globalPoint,
            globalPoint,
            globalPoint
        }
            };
            Children.Add(_tempPolyline);
            _isDrawingConnection = true;
        }
        private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {

            _isPanning = false;


            _selectedElement = null;
        }
    }
}
