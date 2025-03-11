using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using ECAD_Library.Models;
using Avalonia.Media.Imaging;
using System.Reflection;
using System;

namespace ECAD_Library.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        public ObservableCollection<PalleteItem> PaletteItems { get; } = new()
    {
        new PalleteItem{Name = "Resistor",Icon = LoadBitmap("ECAD_Library.Pictures.resistor.png")},
        new PalleteItem{Name = "dc",Icon = LoadBitmap("ECAD_Library.Pictures.dc.png")},
        new PalleteItem{Name = "diode",Icon = LoadBitmap("ECAD_Library.Pictures.diode.png")},
        new PalleteItem{Name = "transistor",Icon = LoadBitmap("ECAD_Library.Pictures.transistor.png")},
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
