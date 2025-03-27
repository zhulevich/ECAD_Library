using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;

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

        public CanvasItem()
        {
            var stackPanel = new StackPanel { Orientation = Orientation.Vertical };
            var image = new Image
            {
                Width = 50,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var text = new TextBlock
            {
                FontSize = 10,
                Width = 60,
                Height = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            image.Bind(Image.SourceProperty, this.GetObservable(IconProperty));
            text.Bind(TextBlock.TextProperty, this.GetObservable(NameProperty));

            stackPanel.Children.Add(image);
            stackPanel.Children.Add(text);

            Content = stackPanel;


        }

    }
}

