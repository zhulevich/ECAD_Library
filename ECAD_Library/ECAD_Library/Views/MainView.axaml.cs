using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using ECAD_Library.Controls;
using ECAD_Library.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ECAD_Library.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (this.FindControl<CanvasItem>("DemoCanvasItem") is { } canvasItem)
            {
                canvasItem.Name = "Резистор";
                canvasItem.Icon = LoadBitmap("ECAD_Library.Pictures.resistor.png");
                canvasItem.ConnectionPoints = new List<Point>
        {
            new Point(2, 26),
            new Point(43, 26)
        };
            }
        }

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
