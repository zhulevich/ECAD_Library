using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using ECAD_Library.Controls;
using ECAD_Library.Models;
using Xunit;
using System.Collections.Generic;
using Avalonia.Interactivity;

namespace ECAD_Tests
{
    public class CanvasControlTests
    {
        [Fact]
        public void OnDrop_ShouldAddCanvasItem()
        {
            // Arrange
            var canvas = new CanvasControl();
            var paletteItem = new PalleteItem
            {
                Name = "TestComponent",
                Icon = null,
                ConnectionPoints = new List<Point> { new Point(10, 10) }
            };
            var data = new DataObject();
            data.Set("PaletteItem", paletteItem);

            var position = new Point(100, 100);
            var dragEventArgs = new DragEventArgs
            {
                Data = data,
                GetPosition = _ => position
            };

            // Act
            

            // Asser
            Assert.Null(dragEventArgs);
            
        }

        [Fact]
        public void HandleConnectionClick_StartsAndCompletesConnection()
        {
            // Arrange
            var canvas = new CanvasControl();
            var canvasItem1 = new CanvasItem { Name = "Item1", ParentCanvas = canvas };
            var canvasItem2 = new CanvasItem { Name = "Item2", ParentCanvas = canvas };

            var button1 = new Button();
            var button2 = new Button();

            canvas.Children.Add(canvasItem1);
            canvas.Children.Add(canvasItem2);
            Canvas.SetLeft(canvasItem1, 0);
            Canvas.SetTop(canvasItem1, 0);
            Canvas.SetLeft(canvasItem2, 100);
            Canvas.SetTop(canvasItem2, 100);

            var localPoint1 = new Point(10, 10);
            var localPoint2 = new Point(20, 20);

            // Act
            canvas.HandleConnectionClick(button1, localPoint1, canvasItem1); 
            canvas.HandleConnectionClick(button2, localPoint2, canvasItem2); 

            // Assert
            Assert.Null(button1); 
        }
    }


    public class DragEventArgs : RoutedEventArgs
    {
        public IDataObject Data { get; set; }
        public Func<Control, Point> GetPosition { get; set; }
    }
}