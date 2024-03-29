﻿using System;
using System.Windows;
using System.Windows.Input;
using TheBureau.Repositories;
using TheBureau.Views;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        readonly RequestRepository _requestRepository = new();

        private int _selectedIndex;
        private string _mainTopText;
        private object _content;
        private int _countRed;

        private WindowState _windowState;

        private ICommand _openSettingsCommand;
        private ICommand _logOutCommand;
        private ICommand _closeWindowCommand;
        private ICommand _minimizeWindowCommand;
        private ICommand _maximizeWindowCommand;

        public ICommand OpenSettingsCommand
        {
            get
            {
                return _openSettingsCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        SettingsWindow sw = new SettingsWindow();
                        if (sw.ShowDialog() == true)
                        {
                        }
                    }
                    catch (Exception)
                    {
                        InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при открытии окна настроек");
                        infoWindow.ShowDialog();
                    }
                });
            }
        }
        public ICommand LogOutCommand
        {
            get
            {
                return _logOutCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        Application.Current.Properties["User"] = null;
                        var helloWindow = new HelloWindowView();
                        helloWindow.Show();
                        Application.Current.Windows[0]?.Close();
                    }
                    catch (Exception)
                    {
                        InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при выходе из аккаунта");
                        infoWindow.ShowDialog();
                    }
                });
            }
        }

        #region Resize

        public WindowState  WindowState
        {
            get => _windowState;
            set { _windowState = value; OnPropertyChanged("WindowState"); }
        }
 
        public ICommand CloseWindowCommand =>
            _closeWindowCommand ??= new RelayCommand(obj => { Application.Current.Shutdown(); });

        public ICommand MinimizeWindowCommand =>
            _minimizeWindowCommand ??= new RelayCommand(obj => { WindowState = WindowState.Minimized; });

        public ICommand MaximizeWindowCommand => _maximizeWindowCommand ??= new RelayCommand(obj =>
            { WindowState = WindowState != WindowState.Maximized ? WindowState.Maximized : WindowState.Normal; });

        #endregion
        public object Content
        {
            get => _content;
            set
            {
                _content = value;
                CountRed = _requestRepository.GetRedRequestsCount();
                OnPropertyChanged("Content");
            }
        }

        public string MainTopText
        {
            get => _mainTopText;
            set { _mainTopText = value; OnPropertyChanged("MainTopText"); }
        }
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                SetPage(_selectedIndex);
                OnPropertyChanged("SelectedIndex");
            }
        }
        public int CountRed
        {
            get => _countRed;
            set { _countRed = value; OnPropertyChanged("CountRed"); }
        }

        public MainWindowViewModel()
        {            
            Content = new StatisticsView();
            WindowState = WindowState.Normal;
            CountRed = _requestRepository.GetRedRequestsCount();
            SelectedIndex = 1;
        }

        private void SetPage(int index)
        {
            switch (index)
            {
                case 0:
                    Content = new StatisticsView();
                    MainTopText = "БЮРО МОНТАЖНИКА";
                    break;
                case 1:
                    Content = new RequestView();
                    MainTopText = "ЗАЯВКИ";
                    break;
                case 2:
                    Content = new BrigadeView();
                    MainTopText = "БРИГАДЫ";
                    break;
                case 3:
                    Content = new ClientView();
                    MainTopText = "КЛИЕНТЫ";
                    break;
                case 4:
                    Content = new EmployeeView();
                    MainTopText = "РАБОТНИКИ";
                    break;
                case 5:
                    Content = new StorageView();
                    MainTopText = "СКЛАД";
                    break;
                default:
                    Content = new StatisticsView();
                    MainTopText = "БЮРО МОНТАЖНИКА";
                    break;                    
            }
        }
    }
}