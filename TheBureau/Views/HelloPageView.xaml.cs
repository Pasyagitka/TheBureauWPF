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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TheBureau.Views
{
    /// <summary>
    /// Логика взаимодействия для HelloPage.xaml
    /// </summary>
    public partial class HelloPageView : Page
    {
        public HelloPageView()
        {
            InitializeComponent();
        }

        private void EnterAsClient_Click(object sender, RoutedEventArgs e)
        {
            var clientMainWindow = new ClientWindowView();
            //var clientMainWindow = new BrigadeMainWindowView();
            App.Current.Windows[0].Close();
            clientMainWindow.Show();
        }
    }
}