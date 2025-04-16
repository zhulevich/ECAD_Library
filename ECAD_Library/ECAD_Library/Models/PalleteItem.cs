using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;

namespace ECAD_Library.Models
{
    public class PalleteItem
    {
        public string Name { get; set; }
        public Bitmap Icon { get; set; }
        public List<Point> ConnectionPoints { get; set; } = new();
    }
}
