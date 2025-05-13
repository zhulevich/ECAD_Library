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
using Avalonia.VisualTree;
using System.Collections.ObjectModel;

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

        private async void CanvasItem_DoubleTapped(object? sender, TappedEventArgs e)
        {
            e.Handled = true;

            if (string.IsNullOrEmpty(Name))
                return;

            var dialog = new ComponentPropertiesDialog
            {
                ComponentName = Name
            };


            switch (Name)
            {
                case "Резистор":
                    dialog.Properties = new ObservableCollection<ComponentProperty>
            {
                new ComponentProperty { Name = "Сопротивление", Value = "100", Unit = "Ом" },
                new ComponentProperty { Name = "Мощность", Value = "0.25", Unit = "Вт" },
                new ComponentProperty { Name = "Точность", Value = "5", Unit = "%" },
                new ComponentProperty { Name = "Темп. коэффициент", Value = "100", Unit = "ppm/°C" },
                new ComponentProperty { Name = "Материал", Value = "Углеродный", Unit = "" }
            };
                    break;

                case "Диод":
                    dialog.Properties = new ObservableCollection<ComponentProperty>
            {
                new ComponentProperty { Name = "Тип", Value = "Выпрямительный", Unit = "" },
                new ComponentProperty { Name = "Макс. обратное напряжение", Value = "50", Unit = "В" },
                new ComponentProperty { Name = "Прямой ток", Value = "1", Unit = "А" },
                new ComponentProperty { Name = "Падение напряжения", Value = "0.7", Unit = "В" },
                new ComponentProperty { Name = "Корпус", Value = "DO-41", Unit = "" }
            };
                    break;

                case "Источник тока":
                    dialog.Properties = new ObservableCollection<ComponentProperty>
            {
                new ComponentProperty { Name = "Напряжение", Value = "5", Unit = "В" },
                new ComponentProperty { Name = "Мощность", Value = "10", Unit = "Вт" },
                new ComponentProperty { Name = "Точность", Value = "2", Unit = "%" },
                new ComponentProperty { Name = "Тип", Value = "Линейный", Unit = "" }
            };
                    break;

                case "Транзистор":
                    dialog.Properties = new ObservableCollection<ComponentProperty>
            {
                new ComponentProperty { Name = "Тип", Value = "NPN", Unit = "" },
                new ComponentProperty { Name = "Макс. ток коллектора", Value = "0.5", Unit = "А" },
                new ComponentProperty { Name = "Напряжение К-Э", Value = "30", Unit = "В" },
                new ComponentProperty { Name = "Коэф. усиления", Value = "100", Unit = "" },
                new ComponentProperty { Name = "Корпус", Value = "TO-92", Unit = "" }
            };
                    break;

                case "Капаситор":
                    dialog.Properties = new ObservableCollection<ComponentProperty>
            {
                new ComponentProperty { Name = "Ёмкость", Value = "100", Unit = "мкФ" },
                new ComponentProperty { Name = "Напряжение", Value = "16", Unit = "В" },
                new ComponentProperty { Name = "Тип", Value = "Электролитический", Unit = "" },
                new ComponentProperty { Name = "Допуск", Value = "20", Unit = "%" },
                new ComponentProperty { Name = "Темп. диапазон", Value = "-40..+85", Unit = "°C" }
            };
                    break;

                case "Катушка":
                    dialog.Properties = new ObservableCollection<ComponentProperty>
            {
                new ComponentProperty { Name = "Индуктивность", Value = "100", Unit = "мкГн" },
                new ComponentProperty { Name = "Ток насыщения", Value = "1.5", Unit = "А" },
                new ComponentProperty { Name = "Сопротивление", Value = "0.1", Unit = "Ом" },
                new ComponentProperty { Name = "Тип", Value = "Дроссель", Unit = "" }
            };
                    break;

                


                case "Переключатель":
                    dialog.Properties = new ObservableCollection<ComponentProperty>
            {
                new ComponentProperty { Name = "Тип", Value = "Тактовый", Unit = "" },
                new ComponentProperty { Name = "Контакты", Value = "1НО", Unit = "" },
                new ComponentProperty { Name = "Напряжение", Value = "50", Unit = "В" },
                new ComponentProperty { Name = "Ток", Value = "0.5", Unit = "А" }
            };
                    break;


                default:
                    dialog.Properties = new ObservableCollection<ComponentProperty>
            {
                new ComponentProperty { Name = "Тип компонента", Value = Name, Unit = "" },
                new ComponentProperty { Name = "Примечание", Value = "Свойства не определены", Unit = "" }
            };
                    break;
            }

            var result = await dialog.ShowDialog<bool>(this.GetVisualRoot() as Window);

            
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

