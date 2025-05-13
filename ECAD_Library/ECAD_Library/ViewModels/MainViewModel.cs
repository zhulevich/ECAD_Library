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
            Name = "Резистор",
            Icon = LoadBitmap("ECAD_Library.Pictures.resistor.png"),
            ConnectionPoints = new List<Point>
            {
                new Point(2, 26),  // слева по центру
                new Point(43, 26), // справа по центру
            }
        },
            new PalleteItem
        {
            Name = "Батарея",
            Icon = LoadBitmap("ECAD_Library.Pictures.battery-48.png"),
            ConnectionPoints = new List<Point>
            {
                new Point(11, 5),  // слева по центру
                new Point(34, 5), // справа по центру
            }
        },
            new PalleteItem
        {
            Name = "Капаситор",
            Icon = LoadBitmap("ECAD_Library.Pictures.capacitor.png"),
            ConnectionPoints = new List<Point>
            {
                new Point(17, 43),  // слева по центру
                new Point(27, 43), // справа по центру
            }
        },
            new PalleteItem
        {
            Name = "Диод",
            Icon = LoadBitmap("ECAD_Library.Pictures.diode-64.png"),
            ConnectionPoints = new List<Point>
            {
                new Point(15, 45),  // слева по центру
                new Point(29, 45)
            }
        },
            new PalleteItem
        {
            Name = "Транзистор",
            Icon = LoadBitmap("ECAD_Library.Pictures.transistor-64.png"),
            ConnectionPoints = new List<Point>
            {
                new Point(13, 45),  // слева по центру
                new Point(22, 45),  // слева по центру
                new Point(32, 45)
            }
        },
        new PalleteItem
        {
            Name = "Источник тока",
            Icon = LoadBitmap("ECAD_Library.Pictures.dc.png"),
            ConnectionPoints = new List<Point>
            {
                new Point(31, 4), // сверху по центру
                new Point(31, 40) // снизу по центру
            }
        },
        new PalleteItem
        {
            Name = "Переключатель",
            Icon = LoadBitmap("ECAD_Library.Pictures.switch.png"),
            ConnectionPoints = new List<Point>
            {
                 new Point(0, 25),  // слева по центру
                new Point(45, 25), // справа по центру
            }
        },
        new PalleteItem
        {
            Name = "Катушка",
            Icon = LoadBitmap("ECAD_Library.Pictures.coil.png"),
            ConnectionPoints = new List<Point>
            {
                 new Point(0, 14),  // слева по центру
                new Point(45, 32), // справа по центру
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
