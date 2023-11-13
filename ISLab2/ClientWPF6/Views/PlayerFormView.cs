using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Server;

namespace ClientWPF6.Views
{
    public class PlayerFormView: INotifyPropertyChanged
    {
        Player _player;
        EventAggregator _eventAggregator;

        string _login;
        string _password;
        string _race;
        string _gClass;
        string _guild;
        int _level;
        int _balance;
        bool _isAdmin;
        ICommand _confirm;

        public string Login { get => _login; set => _login = value; }
        public string Password { get => _password; set => _password = value; }
        public string Race { get => _race; set => _race = value; }
        public string GClass { get => _gClass; set => _gClass = value; }
        public string Guild { get => _guild; set => _guild = value; }
        public int Level { get => _level; set => _level = value; }
        public int Balance { get => _balance; set => _balance = value; }
        public bool IsAdmin { get => _isAdmin; set => _isAdmin = value; }
        public ICommand Confirm { get => _confirm; set => _confirm = value; }

        public PlayerFormView(EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            Confirm = new RelayCommand(_confirmFunc);
        }

        void _confirmFunc()
        {
            _player = new Player(_login, _password, _race, _gClass, _guild, _level, _balance, _isAdmin);
            _eventAggregator.SendData(_player);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
