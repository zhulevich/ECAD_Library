using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Xaml.Interactions.DragAndDrop;
using ECAD_Library.Models;

namespace ECAD_Library.Utilities.Behaviors
{
    public class CanvasDropHandler : DropHandlerBase
    {
        public override bool Validate(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
        {
            return e.Data.Contains("PaletteItem");
        }

        public override void Drop(object? sender, DragEventArgs e, object? sourceContext, object? targetContext)
        {
            if (sender is Canvas canvas && e.Data.Get("PaletteItem") is PalleteItem item)
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
}
