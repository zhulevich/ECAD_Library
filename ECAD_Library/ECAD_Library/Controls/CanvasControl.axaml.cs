using Avalonia;
using Avalonia.Controls;
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
        private void OnDrop(object? sender, DragEventArgs e)
        {
            if (sender is Canvas canvas && e.Data.Contains("PaletteItem"))
            {
                if (e.Data.Get("PaletteItem") is PalleteItem item)
                {
                    var image = new Image
                    {
                        Source = item.Icon,
                        Width = 50,
                        Height = 50
                    };

                    var position = e.GetPosition(canvas);
                    Canvas.SetLeft(image, position.X);
                    Canvas.SetTop(image, position.Y);

                    canvas.Children.Add(image);
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
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                _isPanning = true;
                _lastMousePosition = e.GetPosition(this);
            }
        }

        private void OnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (_isPanning)
            {
                var currentPosition = e.GetPosition(this);
                var delta = currentPosition - _lastMousePosition;

                _translateTransform.X += delta.X;
                _translateTransform.Y += delta.Y;

                _lastMousePosition = currentPosition;
            }
        }

        private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            _isPanning = false;
        }
    }
}
