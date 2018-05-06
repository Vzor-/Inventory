using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Inventory.Popups
{
    public partial class GeneralDialog : Window
    {
        public bool isClean = false;
        public GeneralDialog()
        {
            InitializeComponent();
        }

        public void setTitle(string title)
        {
            this.Title = title;
        }

        public TextBox addSimpleField(string text)
        {
            RowDefinition newRow = new RowDefinition();
            MainGrid.RowDefinitions.Add(newRow);

            DockPanel dock = new DockPanel();
            dock.Margin = new Thickness(5, 0, 5, 0);
            Grid.SetRow(dock, MainGrid.RowDefinitions.Count - 1);

            TextBlock newText = new TextBlock();
            newText.FontSize = 18;
            newText.Text = text;
            newText.Padding = new Thickness(0, 0, 50, 0);
            newText.Margin = new Thickness(5);
            dock.Children.Add(newText);

            TextBox newField = new TextBox();
            newField.FontSize = 18;
            newField.HorizontalAlignment = HorizontalAlignment.Stretch;
            newField.Margin = new Thickness(5);
            dock.Children.Add(newField);

            MainGrid.Children.Add(dock);

            return newField;
        }

        public void addReadOnlyField(string text, string value)
        {
            RowDefinition newRow = new RowDefinition();
            MainGrid.RowDefinitions.Add(newRow);

            DockPanel dock = new DockPanel();
            dock.Margin = new Thickness(5, 0, 5, 0);
            Grid.SetRow(dock, MainGrid.RowDefinitions.Count - 1);

            TextBlock newText = new TextBlock();
            newText.FontSize = 18;
            newText.Text = text;
            newText.Width = 150;
            newText.Margin = new Thickness(5);
            dock.Children.Add(newText);

            TextBlock newField = new TextBlock();
            newField.FontSize = 18;
            newField.Text = value;
            newField.HorizontalAlignment = HorizontalAlignment.Stretch;
            newField.Margin = new Thickness(5);
            dock.Children.Add(newField);

            MainGrid.Children.Add(dock);
        }

        public CheckBox addCheckbox(string text, bool isChecked)
        {
            RowDefinition newRow = new RowDefinition();
            MainGrid.RowDefinitions.Add(newRow);

            CheckBox cb = new CheckBox();
            cb.Content = text;
            cb.IsChecked = isChecked;
            cb.Margin = new Thickness(5, 0, 0, 0);

            Grid.SetRow(cb, MainGrid.RowDefinitions.Count - 1);

            MainGrid.Children.Add(cb);

            return cb;
        }

        public Button addSimpleButtons(string text)
        {
            RowDefinition newRow = new RowDefinition();
            MainGrid.RowDefinitions.Add(newRow);

            StackPanel stacker = new StackPanel();
            stacker.Height = 40;
            stacker.Orientation = Orientation.Horizontal;
            stacker.HorizontalAlignment = HorizontalAlignment.Right;
            stacker.Margin = new Thickness(5, 0, 5, 0);
            Grid.SetRow(stacker, MainGrid.RowDefinitions.Count - 1);

            Button button = new Button();
            button.Content = text;
            button.FontSize = 18;
            button.Margin = new Thickness(5);
            button.Padding = new Thickness(5, 0, 5, 0);
            stacker.Children.Add(button);

            Button cancel = new Button();
            cancel.Content = "Cancel";
            cancel.FontSize = 18;
            cancel.Margin = new Thickness(5);
            cancel.Padding = new Thickness(5, 0, 5, 0);
            cancel.IsCancel = true;
            stacker.Children.Add(cancel);

            MainGrid.Children.Add(stacker);

            return button;
        }
    }
}
