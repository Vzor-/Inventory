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
    public partial class PartAddDialog : Window
    {
        public string EnglishName, InternalID;
        public bool isClean = false, IsActive;
        public PartAddDialog()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            EnglishName = EnglishNameField.Text;
            InternalID = InternalIDField.Text;
            IsActive = (bool)IsActiveCheckbox.IsChecked;
            if ((EnglishName != "") && (InternalID != "")) isClean = true;
            this.Close();
        }
    }
}
