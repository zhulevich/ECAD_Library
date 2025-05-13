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
                case "��������":
                    dialog.Properties = new ObservableCollection<ComponentProperty>
            {
                new ComponentProperty { Name = "�������������", Value = "100", Unit = "��" },
                new ComponentProperty { Name = "��������", Value = "0.25", Unit = "��" },
                new ComponentProperty { Name = "��������", Value = "5", Unit = "%" },
                new ComponentProperty { Name = "����. �����������", Value = "100", Unit = "ppm/�C" },
                new ComponentProperty { Name = "��������", Value = "����������", Unit = "" }
            };
                    break;

                case "����":
                    dialog.Properties = new ObservableCollection<ComponentProperty>
            {
                new ComponentProperty { Name = "���", Value = "��������������", Unit = "" },
                new ComponentProperty { Name = "����. �������� ����������", Value = "50", Unit = "�" },
                new ComponentProperty { Name = "������ ���", Value = "1", Unit = "�" },
                new ComponentProperty { Name = "������� ����������", Value = "0.7", Unit = "�" },
                new ComponentProperty { Name = "������", Value = "DO-41", Unit = "" }
            };
                    break;

                case "�������� ����":
                    dialog.Properties = new ObservableCollection<ComponentProperty>
            {
                new ComponentProperty { Name = "����������", Value = "5", Unit = "�" },
                new ComponentProperty { Name = "��������", Value = "10", Unit = "��" },
                new ComponentProperty { Name = "��������", Value = "2", Unit = "%" },
                new ComponentProperty { Name = "���", Value = "��������", Unit = "" }
            };
                    break;

                case "����������":
                    dialog.Properties = new ObservableCollection<ComponentProperty>
            {
                new ComponentProperty { Name = "���", Value = "NPN", Unit = "" },
                new ComponentProperty { Name = "����. ��� ����������", Value = "0.5", Unit = "�" },
                new ComponentProperty { Name = "���������� �-�", Value = "30", Unit = "�" },
                new ComponentProperty { Name = "����. ��������", Value = "100", Unit = "" },
                new ComponentProperty { Name = "������", Value = "TO-92", Unit = "" }
            };
                    break;

                case "���������":
                    dialog.Properties = new ObservableCollection<ComponentProperty>
            {
                new ComponentProperty { Name = "�������", Value = "100", Unit = "���" },
                new ComponentProperty { Name = "����������", Value = "16", Unit = "�" },
                new ComponentProperty { Name = "���", Value = "�����������������", Unit = "" },
                new ComponentProperty { Name = "������", Value = "20", Unit = "%" },
                new ComponentProperty { Name = "����. ��������", Value = "-40..+85", Unit = "�C" }
            };
                    break;

                case "�������":
                    dialog.Properties = new ObservableCollection<ComponentProperty>
            {
                new ComponentProperty { Name = "�������������", Value = "100", Unit = "����" },
                new ComponentProperty { Name = "��� ���������", Value = "1.5", Unit = "�" },
                new ComponentProperty { Name = "�������������", Value = "0.1", Unit = "��" },
                new ComponentProperty { Name = "���", Value = "��������", Unit = "" }
            };
                    break;

                


                case "�������������":
                    dialog.Properties = new ObservableCollection<ComponentProperty>
            {
                new ComponentProperty { Name = "���", Value = "��������", Unit = "" },
                new ComponentProperty { Name = "��������", Value = "1��", Unit = "" },
                new ComponentProperty { Name = "����������", Value = "50", Unit = "�" },
                new ComponentProperty { Name = "���", Value = "0.5", Unit = "�" }
            };
                    break;


                default:
                    dialog.Properties = new ObservableCollection<ComponentProperty>
            {
                new ComponentProperty { Name = "��� ����������", Value = Name, Unit = "" },
                new ComponentProperty { Name = "����������", Value = "�������� �� ����������", Unit = "" }
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

