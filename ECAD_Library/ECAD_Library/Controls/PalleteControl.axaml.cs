using Avalonia.Controls;
using Avalonia.Input;
using ECAD_Library.Models;

namespace ECAD_Library.Controls
{
    public partial class PaletteControl : UserControl
    {
        public PaletteControl()
        {
            InitializeComponent();
        }

        private void OnDragStart(object? sender, PointerPressedEventArgs e)
        {
            if (sender is Image image && image.DataContext is PalleteItem item)
            {
                var data = new DataObject();
                data.Set("PaletteItem", item); // Используем уникальный идентификатор

                DragDrop.DoDragDrop(e, data, DragDropEffects.Copy);
            }
        }
    }
}
