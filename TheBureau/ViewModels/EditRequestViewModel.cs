﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using TheBureau.Models.DataManipulating;
using TheBureau.Repositories;
using static System.Int32;

namespace TheBureau.ViewModels
{
    public class EditRequestViewModel : ViewModelBase
    {
        private RequestRepository _requestRepository = new RequestRepository();
        private BrigadeRepository _brigadeRepository = new();
        int _selectedBrigadeId;
        private int _requestStatus;
        ObservableCollection<Brigade> _brigades;
        private RelayCommand _updateRequest;
        private Request _requestForEdit;
        public bool sendEmail;

        public bool SendEmail
        {
            get => sendEmail;
            set { sendEmail = value; OnPropertyChanged("SendEmail"); }
        }

        public EditRequestViewModel(Request request)
        {
            Brigades = new ObservableCollection<Brigade>(_brigadeRepository.GetAll());
            Brigades.Add(new Brigade{id=0});
            RequestForEdit = request;
        }
        public Request RequestForEdit
        {
            get => _requestForEdit;
            set
            {
                _requestForEdit = value;
                OnPropertyChanged("RequestForEdit");
            }
        }
        //todo command parameter

        public ICommand UpdateRequestCommand => _updateRequest ??= new RelayCommand(UpdateRequest);

        private void UpdateRequest(object o)
        {
            bool isStatusChanged = false;
            var request = _requestRepository.Get(RequestForEdit.id);
            if (Parse(RequestStatus) == 1 || Parse(RequestStatus) == 2 ||Parse(RequestStatus)== 3)
            {
                if (Parse(RequestStatus) != request.status) isStatusChanged = true;
                request.status = Parse(RequestStatus);
            }
            if (SelectedBrigadeId != null)
            {
                if (SelectedBrigadeId == 0) request.brigadeId = null;
                else 
                    request.brigadeId = SelectedBrigadeId;
            }
            _requestRepository.Update(request);
            _requestRepository.Save();

            if (isStatusChanged && SendEmail)
            {
                Notifications.SendRequestStatusChanged(request);
            }
            OnPropertyChanged("Requests");
        }

        public int SelectedBrigadeId
        {
            get => _selectedBrigadeId;
            set
            {
                _selectedBrigadeId = value;
                OnPropertyChanged("SelectedBrigadeId");
            }
        }
        
        public ObservableCollection<Brigade> Brigades 
        { 
            get => _brigades; 
            set 
            { 
                _brigades = value; 
                OnPropertyChanged("Brigades"); 
            } 
        }
        
        public string RequestStatus
        {
            get => _requestStatus.ToString();
            set
            {
                if (value.Contains("Готово"))
                    _requestStatus = 3; 
                else if (value.Contains("В процессе"))
                    _requestStatus = 2;
                else _requestStatus = 1;
                OnPropertyChanged("RequestStatus");
            }
        }
        
    }
}