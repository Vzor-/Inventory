﻿using System;
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
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PartDelDialog : Window
    {
        public bool isDelete = false;
        public PartDelDialog(string EnglishName, string InternalID)
        {
            InitializeComponent();
            promptText.Text += "\"" + EnglishName + "\" ID: " + InternalID + "?";
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            isDelete = true;
            this.Close();
        }
    }
}
