using Avalonia;
using Avalonia.Media.Imaging;
using ECAD_Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace ECAD_Library.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        public ObservableCollection<PalleteItem> PaletteItems { get; } = new()
    {
            new PalleteItem
        {
            Name = "Resistor",
            Icon = LoadBitmap("ECAD_Library.Pictures.resistor.png"),
            ConnectionPoints = new List<Point>
            {
                new Point(0, 25),  // слева по центру
                new Point(40, 25), // справа по центру
            }
        },
        new PalleteItem
        {
            Name = "dc",
            Icon = LoadBitmap("ECAD_Library.Pictures.dc.png"),
            ConnectionPoints = new List<Point>
            {
                new Point(25, 0), // сверху по центру
                new Point(25, 50) // снизу по центру
            }
        },
    };

        private static Bitmap LoadBitmap(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream == null)
                    throw new ArgumentException($"Resource '{resourcePath}' not found.");

                return new Bitmap(stream);
            }
        }
    }
}
