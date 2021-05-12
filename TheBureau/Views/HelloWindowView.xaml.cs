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

namespace TheBureau.Views
{
    public partial class HelloWindowView : Window
    {
        public HelloWindowView()
        {
            InitializeComponent();
            AuthFrame.Content = new HelloPageView();
        }

        private void ClientButton_Click(object sender, RoutedEventArgs e)
        {
            AuthFrame.Content = new HelloPageView();
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            AuthFrame.Content = new AuthenticationPageView();
        }

        private void ShutdownButton_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}