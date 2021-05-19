﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TheBureau.Models.DataManipulating;
using TheBureau.Repositories;
using TheBureau.Views;

namespace TheBureau.ViewModels
{
    public class ClientWindowViewModel : ViewModelBase
    {
        private RequestRepository _requestRepository;
        private ClientRepository _clientRepository;
        private AddressRepository _addressRepository;
        private ToolRepository _toolRepository = new ToolRepository();
        private RequestEquipmentRepository _requestEquipmentRepository;
        
        private ObservableCollection<Request> _requests;
        private RelayCommand sendRequestCommand;
        private ICommand logOutCommand;
        
        private string _findRequestText;
        private string _firstname;
        private string _surname;
        private string _patronymic;
        private decimal _contactNumber;
        private string _email;
        private string _city;
        private string _street;
        private int _house;
        private int _corpus;
        private int _flat;
        private string _statusCost;
        private string _emailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        private string _contactNumberPattern = @"^((\+7|7|8)+([0-9]){10})$";
        private int _rpQuantity;
        private int _rsQuantity;
        private int _kpQuantity;
        private int _ksQuantity;
        private int _vpQuantity;
        private bool _isRough;
        private bool _isClean;
        private string _comment;
        private DateTime _mountingDate;
        
        private ICommand closeWindowCommand;
        private ICommand minimizeWindowCommand;
        private WindowState _windowState;


        public ICommand CloseWindowCommand
        {
            get
            {
                return closeWindowCommand = new RelayCommand(obj =>
                {
                    Application.Current.Shutdown();
                });
            }
        }
        public WindowState  WindowState
        {
            get { return _windowState; }
            set
            {
                _windowState = value;
                OnPropertyChanged("WindowState");
            }
        }
        public ICommand MinimizeWindowCommand
        {
            get
            {
                return minimizeWindowCommand = new RelayCommand(obj =>
                {
                    WindowState = WindowState.Minimized;
                });
            }
        }
        

        public DateTime MountingDate
        {
            get => _mountingDate;
            set
            {
                _mountingDate = value;
                OnPropertyChanged("MountingDate");
            }
        }

        public string Comment
        {
            get => _comment;
            set { _comment = value; OnPropertyChanged("Comment"); }
        }

        public int RpQuantity
        {
            get => _rpQuantity;
            set { _rpQuantity = value; OnPropertyChanged("RpQuantity");}
        }

        public int RsQuantity
        {
            get => _rsQuantity;
            set { _rsQuantity = value; OnPropertyChanged("RsQuantity");}
        }

        public int KpQuantity
        {
            get => _kpQuantity;
            set { _kpQuantity = value; OnPropertyChanged("KpQuantity");}
        }

        public int KsQuantity
        {
            get => _ksQuantity;
            set {  _ksQuantity = value; OnPropertyChanged("KsQuantity");}
        }

        public int VpQuantity
        {
            get => _vpQuantity;
            set { _vpQuantity = value; OnPropertyChanged("VpQuantity");}
        }

        public bool IsRough
        {
            get => _isRough;
            set { _isRough = value; OnPropertyChanged("IsRough");}
        }

        public bool IsClean
        {
            get => _isClean;
            set { _isClean = value; OnPropertyChanged("IsClean");}
        }

        public int Stage
        {
            get
            {
                if (IsRough)
                {
                    return IsClean ? 3 : 1;
                    //Черновая + чистовая - 3, черновая - 1
                }
                if (IsClean) return 2; //только чистовая
                return 0;
            }
        }

        private string statusName;

        public RelayCommand SendRequestCommand
        {
            get
            {
                return sendRequestCommand ??= new RelayCommand(obj =>
                {
                    Address address;
                    Client client;
                    
                    //Если адрес есть в базе, он не дублируется (берется существующий)
                    if(!_addressRepository.IsDuplicateAddress(City, Street, Int32.Parse(House), Corpus, Int32.Parse(Flat)))
                    {
                        address = _addressRepository.FindAddress(City, Street, Int32.Parse(House), Corpus, Int32.Parse(Flat));
                    }
                    else
                    {
                        address = new Address{country = "Беларусь",city=City, street = Street, house = Int32.Parse(House), 
                            corpus = Corpus, flat = Int32.Parse(Flat)};
                        _addressRepository.Add(address);
                        _addressRepository.Save();
                    }
                    //Если клиент есть в базе, он не дублируется (берется существующий)
                    if (!_clientRepository.IsDuplicateClient(Surname, Firstname, Patronymic, Email, ContactNumber))
                    {
                        client = _clientRepository.FindClient(Surname, Firstname, Patronymic, Email, ContactNumber);
                    }
                    else
                    {
                        client = new Client{firstname = Firstname, patronymic = Patronymic, surname = Surname, 
                            email = Email, contactNumber = Decimal.Parse(ContactNumber)};
                        _clientRepository.Add(client);
                        _clientRepository.Save();
                    }
                    
                    var request = new Request
                    {
                        clientId = client.id, addressId = address.id, stage=Stage, status = 1, mountingDate=MountingDate,
                        comment = Comment
                    };
                    _requestRepository.Add(request);
                    _requestRepository.Save();
                    //todo quantity default 0
                    
                    //Количество приборов каждого типа в оборудовании заявки
                    if (RpQuantity != 0) {
                        var requestEquipmentRP = new RequestEquipment  {  requestId = request.id, equipmentId = "RP", quantity = RpQuantity };
                        _requestEquipmentRepository.Add(requestEquipmentRP);
                    }
                    if (RsQuantity != 0)
                    {
                        var requestEquipmentRS = new RequestEquipment { requestId = request.id, equipmentId = "RS", quantity = RsQuantity};
                        _requestEquipmentRepository.Add(requestEquipmentRS);

                    }
                    if (KpQuantity != 0)
                    {
                        var requestEquipmentKP = new RequestEquipment { requestId = request.id, equipmentId = "HP", quantity = KpQuantity };
                        _requestEquipmentRepository.Add(requestEquipmentKP);
                    }

                    if (KsQuantity != 0)
                    {
                        var requestEquipmentKS = new RequestEquipment  { requestId = request.id, equipmentId = "HS", quantity = KsQuantity };
                        _requestEquipmentRepository.Add(requestEquipmentKS);
                    }

                    if (VpQuantity != 0)
                    {
                        var requestEquipmentVP = new RequestEquipment { requestId = request.id, equipmentId = "VP", quantity = VpQuantity  };
                        _requestEquipmentRepository.Add(requestEquipmentVP);
                    }
                    _requestEquipmentRepository.Save();
                    Update();

                    //Получение сформированной записи из БД, отправка уведолмения на почту клиента
                    var requestForNotification = _requestRepository.Get(request.id);
                    var tools = _toolRepository.GetByStage(requestForNotification.stage);
                    var accessories = _requestEquipmentRepository.GetAccessories(requestForNotification.RequestEquipments);
                    Notifications.SendRequestAccept(requestForNotification, tools, accessories);

                    OnPropertyChanged("SendRequestCommand");
                });
            }
        }
        
        #region  fields
        public string Firstname
        {
            get => _firstname;
            set
            {
                if (value.Length is >= 2 and <= 20)
                {
                    _firstname = value;
                    statusName = String.Empty;
                }
                else
                    statusName = "Имя должно быть от 2 до 20 символов";
                OnPropertyChanged("Firstname");
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                if (value.Length is >= 2 and <= 20)
                {
                    _surname = value;
                    statusName = String.Empty;
                }
                else
                    statusName = "Фамилия должна быть от 2 до 20 символов";
                OnPropertyChanged("Surname");
            }
        }

        public string Patronymic
        {
            get => _patronymic;
            set
            {
                if (value.Length is >= 2 and <= 20)
                {
                    _patronymic = value;
                    statusName = String.Empty;
                }
                else
                    statusName = "Отчество должно быть от 2 до 20 символов";
                OnPropertyChanged("Patronymic");
            }
        }

        public string ContactNumber
        {
           
            get => _contactNumber.ToString();
            set
            {
                if (Decimal.TryParse(value, out _contactNumber) && value.Length == 12
                    && Decimal.Parse(value) >= 0)
                {
                   _contactNumber = Decimal.Parse(value);
                   _statusCost = String.Empty;
                }
                else
                {
                    _statusCost = "Номер должен быть длиной 12 и содержать лишь цифры";
                }
                OnPropertyChanged("ContactNumber"); 
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        public string City
        {
            get => _city;
            set => _city = value;
        }

        public string Street
        {
            get => _street;
            set => _street = value;
        }

        public string House
        {
            get => _house.ToString();
            set => _house = int.Parse(value);
        }
        
        public string Corpus
        {
            get => _corpus.ToString();
            set => _corpus = int.Parse(value);
        }
        
        public string Flat
        {
            get => _flat.ToString();
            set => _flat = int.Parse(value);
        }
        #endregion

        public ObservableCollection<Request> Requests
        {
            get => _requests;
            set { _requests = value; OnPropertyChanged("Requests"); }
        }

        public string FindRequestText
        {
            get => _findRequestText;
            set
            {
                _findRequestText = value;
                SetClientsRequests();
                OnPropertyChanged("FindRequestText");
            }
        }
        void SetClientsRequests()
        {
            Requests = new ObservableCollection<Request>(_requestRepository.GetRequestsBySurnameOrEmail(FindRequestText));
        }

        public ClientWindowViewModel()
        { 
            Update();
            MountingDate = DateTime.Today;
            WindowState = WindowState.Normal;
        }

        public ICommand LogOutCommand
        {
            get
            {
                return logOutCommand = new RelayCommand(obj =>
                {
                    Application.Current.Properties["User"] = null;
                    var helloWindow = new HelloWindowView();
                    helloWindow.Show();
                    Application.Current.Windows[0]?.Close();
                    OnPropertyChanged("LogOutCommand");
                });
            }
        }
        
        public void Update()
        {
            _requestRepository = new RequestRepository();
            _clientRepository = new ClientRepository();
            _addressRepository = new AddressRepository();
            _requestEquipmentRepository = new RequestEquipmentRepository();
        }

    }
}