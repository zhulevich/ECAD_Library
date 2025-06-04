using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.Media;
using System.Collections.ObjectModel;

namespace ECAD_Library.Controls
{
    public class ComponentProperty
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public string Unit { get; set; }
    }

    public partial class ComponentPropertiesDialog : Window
    {
        public static readonly StyledProperty<string> ComponentNameProperty =
            AvaloniaProperty.Register<ComponentPropertiesDialog, string>(nameof(ComponentName));

        public string ComponentName
        {
            get => GetValue(ComponentNameProperty);
            set => SetValue(ComponentNameProperty, value);
        }

        public static readonly StyledProperty<ObservableCollection<ComponentProperty>> PropertiesProperty =
            AvaloniaProperty.Register<ComponentPropertiesDialog, ObservableCollection<ComponentProperty>>(nameof(Properties));

        public ObservableCollection<ComponentProperty> Properties
        {
            get => GetValue(PropertiesProperty);
            set => SetValue(PropertiesProperty, value);
        }

        public ComponentPropertiesDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Width = 400;
            Height = 300;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MinWidth = 300;
            MinHeight = 200;

            var mainStackPanel = new StackPanel
            {
                Margin = new Thickness(20),
                Spacing = 10
            };

            var nameTextBlock = new TextBlock
            {
                FontSize = 16,
                FontWeight = FontWeight.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };
            nameTextBlock.Bind(TextBlock.TextProperty, this.GetObservable(ComponentNameProperty));

            mainStackPanel.Children.Add(nameTextBlock);

            var propertiesItemsControl = new ItemsControl();
            propertiesItemsControl.Bind(ItemsControl.ItemsSourceProperty, this.GetObservable(PropertiesProperty));

            propertiesItemsControl.ItemTemplate = new FuncDataTemplate<ComponentProperty>((prop, _) =>
            {
                var grid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = GridLength.Auto },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width = GridLength.Auto }
                    },
                    Margin = new Thickness(0, 5)
                };

                var nameText = new TextBlock
                {
                    Text = prop.Name,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 10, 0)
                };
                Grid.SetColumn(nameText, 0);

                var valueBox = new TextBox
                {
                    Text = prop.Value?.ToString(),
                    VerticalAlignment = VerticalAlignment.Center
                };
                valueBox.PropertyChanged += (s, e) =>
                {
                    if (e.Property == TextBox.TextProperty)
                    {
                        prop.Value = valueBox.Text;
                    }
                };
                Grid.SetColumn(valueBox, 1);

                var unitText = new TextBlock
                {
                    Text = prop.Unit,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10, 0, 0, 0)
                };
                Grid.SetColumn(unitText, 2);

                grid.Children.Add(nameText);
                grid.Children.Add(valueBox);
                grid.Children.Add(unitText);

                return grid;
            });

            mainStackPanel.Children.Add(propertiesItemsControl);

            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Spacing = 10,
                Margin = new Thickness(0, 20, 0, 0)
            };

            var okButton = new Button
            {
                Content = "OK",
                MinWidth = 80
            };
            okButton.Click += (s, e) => Close(true);

            var cancelButton = new Button
            {
                Content = "Cancel",
                MinWidth = 80
            };
            cancelButton.Click += (s, e) => Close(false);

            buttonPanel.Children.Add(okButton);
            buttonPanel.Children.Add(cancelButton);
            mainStackPanel.Children.Add(buttonPanel);

            Content = mainStackPanel;
        }
    }
}