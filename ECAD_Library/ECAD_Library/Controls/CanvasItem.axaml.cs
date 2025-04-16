using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using System.Collections.Generic;
using System;
using System.Reactive.Linq;

namespace ECAD_Library.Controls
{
    public partial class CanvasItem : UserControl
    {
        public static readonly StyledProperty<Bitmap?> IconProperty =
            AvaloniaProperty.Register<CanvasItem, Bitmap?>(nameof(Icon));

        public static readonly StyledProperty<string?> NameProperty =
            AvaloniaProperty.Register<CanvasItem, string?>(nameof(Name));

        public Bitmap? Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public string? Name
        {
            get => GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }
        public static readonly StyledProperty<List<Point>> ConnectionPointsProperty =
    AvaloniaProperty.Register<CanvasItem, List<Point>>(nameof(ConnectionPoints));

        public List<Point> ConnectionPoints
        {
            get => GetValue(ConnectionPointsProperty);
            set => SetValue(ConnectionPointsProperty, value);
        }
        private Canvas _connectionCanvas;
        public CanvasItem()
        {
            var grid = new Grid();

            var image = new Image
            {
                Width = 50,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            image.Bind(Image.SourceProperty, this.GetObservable(IconProperty));
            grid.Children.Add(image);

            _connectionCanvas = new Canvas
            {
                Width = 50,
                Height = 50,
                IsHitTestVisible = true // если кнопки не нужны для кликов
            };

            grid.Children.Add(_connectionCanvas);
            Content = grid;
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (ConnectionPoints != null)
            {
                foreach (var point in ConnectionPoints)
                {
                    var btn = new Button
                    {
                        Width = 5,
                        Height = 5,
                        Background = Brushes.Blue,
                        BorderThickness = new Thickness(1),
                        IsHitTestVisible = true
                    };
                    Canvas.SetLeft(btn, point.X );
                    Canvas.SetTop(btn, point.Y );
                    _connectionCanvas.Children.Add(btn);
                }
            }
        }
    }
}

