using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using System.Collections.Generic;
using System;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.Input;

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
        public CanvasControl? ParentCanvas { get; set; }
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
                IsHitTestVisible = true 
            };
            this.DoubleTapped += CanvasItem_DoubleTapped;
            grid.Children.Add(_connectionCanvas);
            Content = grid;
        }

        private void CanvasItem_DoubleTapped(object? sender, TappedEventArgs e)
        {
            e.Handled = true;

            var dialog = new Window
            {
                Title = $"Свойства: {Name ?? "Элемент"}",
                Width = 400,
                Height = 300,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            var stackPanel = new StackPanel
            {
                Margin = new Thickness(20)
            };

            stackPanel.Children.Add(new TextBlock
            {
                Text = $"Вы дважды нажали на элемент: {Name ?? "Без имени"}"
            });

            stackPanel.Children.Add(new Button
            {
                Content = "Закрыть",
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 0),
                Command = new RelayCommand(() => dialog.Close())
            });

            dialog.Content = stackPanel;

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
                        IsHitTestVisible = true,
                        Focusable = true
                    };
                    btn.Click += (_, e) =>
                    {
                        ParentCanvas.HandleConnectionClick(btn, point, this);
                        e.Handled = true;
                    };
                    btn.Click += (_, e) =>
                    {
                        ParentCanvas.HandleConnectionClick(btn, point, this);
                        e.Handled = true;
                    };
                    Canvas.SetLeft(btn, point.X );
                    Canvas.SetTop(btn, point.Y );
                    _connectionCanvas.Children.Add(btn);
                }
            }
        }
    }
}

