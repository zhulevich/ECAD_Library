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
        private Control? _selectedElement;

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
            var point = e.GetCurrentPoint(this);

            // Если нажата левая кнопка мыши и панорамирование не активно
            if (point.Properties.IsLeftButtonPressed && !_isPanning)
            {
                // Проверяем, был ли клик по какому-либо элементу
                var hitTestResult = this.InputHitTest(point.Position);
                if (hitTestResult is Image image)
                {
                    // Выбираем объект
                    _selectedElement = image;
                    _lastMousePosition = point.Position;
                }
            }
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                _isPanning = true;
                _lastMousePosition = e.GetPosition(this);
            }
        }

        private void OnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (_selectedElement != null)
            {
                // Логика перемещения объекта
                var currentPosition = e.GetPosition(this);

                // Получаем текущие координаты объекта
                double left = Canvas.GetLeft(_selectedElement);
                double top = Canvas.GetTop(_selectedElement);

                // Обновляем координаты объекта
                Canvas.SetLeft(_selectedElement, left + (currentPosition.X - _lastMousePosition.X));
                Canvas.SetTop(_selectedElement, top + (currentPosition.Y - _lastMousePosition.Y));

                // Обновляем последнюю позицию мыши
                _lastMousePosition = currentPosition;
            }
            else if (_isPanning)
            {
                // Логика панорамирования остается той же
                var currentPosition = e.GetPosition(this);
                var delta = currentPosition - _lastMousePosition;

                _translateTransform.X += delta.X;
                _translateTransform.Y += delta.Y;

                _lastMousePosition = currentPosition;
            }
            
        }

        private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
{
    // Отключаем панорамирование
    _isPanning = false;

    // Снимаем выбор с объекта
    _selectedElement = null;
}
    }
}
