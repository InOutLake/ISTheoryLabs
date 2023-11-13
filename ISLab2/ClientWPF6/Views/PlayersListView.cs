using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Server;

namespace ClientWPF6.Views
{
    public class PlayersListView : INotifyPropertyChanged
    {
        private Client.Client _client;
        private EventAggregator _eventAggreagator;

        private ObservableCollection<Player>? _allPlayers;
        private Player? _selectedPlayer;
        private Player? _newPlayer;
        private int _playerID;
        private ICommand? _addPlayer;
        private ICommand? _removePlayer;
        private ICommand? _findPlayer;
        private ICommand? _refreshPlayers;
        public ObservableCollection<Player>? AllPlayers { get => _allPlayers; set => _allPlayers = value; }
        public Player? SelectedPlayer { get => _selectedPlayer; set => _selectedPlayer = value; }
        public int PlayerID { get => _playerID; set => _playerID = value; }
        public ICommand? AddPlayer { get => _addPlayer; set => _addPlayer = value; }
        public ICommand? RemovePlayer { get => _removePlayer; set => _removePlayer = value; }
        public ICommand? FindPlayer { get => _findPlayer; set => _findPlayer = value; }
        public ICommand? RefreshPlayers { get => _refreshPlayers; set => _refreshPlayers = value; }
        public PlayersListView(EventAggregator eventAggreagator)
        {
            _client = new Client.Client();

            AddPlayer = new RelayCommand(_addPlayerFunc);
            RemovePlayer = new RelayCommand(_removePlayerFunc);
            FindPlayer = new RelayCommand(_findPlayerFunc);
            RefreshPlayers = new RelayCommand(_refreshPlayersFunc);
            _eventAggreagator = eventAggreagator;
            _eventAggreagator.DataReady += _loadNewPlayer;
        }

        void _loadNewPlayer(Player p)
        {
            _newPlayer = p;
        }
        void _addPlayerFunc()
        {
            _newPlayer = new Player();
            var playerFormWindow = new PlayerFormWindow(_eventAggreagator);
            playerFormWindow.ShowDialog();


            _client.ReceiveMessage();
            _client.ReceiveMessage();
            _client.SendKey('3');

            _client.ReceiveMessage();
            _client.ReceiveMessage();
            _client.SendMessage("2" + _newPlayer.Login);

            _client.ReceiveMessage();
            _client.ReceiveMessage();
            _client.SendMessage("2" + _newPlayer.Password);

            _client.ReceiveMessage();
            _client.ReceiveMessage();
            _client.SendMessage("2" + _newPlayer.Race);

            _client.ReceiveMessage();
            _client.ReceiveMessage();
            _client.SendMessage("2" + _newPlayer.GClass);

            _client.ReceiveMessage();
            _client.ReceiveMessage();
            _client.SendMessage("2" + _newPlayer.Guild);

            if (_newPlayer.IsAdmin)
            {
                _client.ReceiveMessage();
                _client.ReceiveMessage();
                _client.SendMessage("2y");
            }
            else
            {
                _client.ReceiveMessage();
                _client.ReceiveMessage();
                _client.SendMessage("2n");
            }
            AllPlayers.Add(_newPlayer);
            _client.ReceiveMessage();
        }
        void _removePlayerFunc()
        {
            _client.ReceiveMessage();
            _client.SendKey('4');
            _client.ReceiveMessage();
            _client.ReceiveMessage();
            if (SelectedPlayer != null)
            {
                _client.SendMessage("2" + SelectedPlayer.PlayerID.ToString());
            }
            _client.ReceiveMessage();
            _client.ReceiveMessage();
            AllPlayers.Remove(SelectedPlayer);
        }
        void _findPlayerFunc() 
        {
            if (AllPlayers.Where(x => x.PlayerID == PlayerID).Any())
            {
                _selectedPlayer = AllPlayers.Where(x => x.PlayerID == PlayerID)?.First();
            }
            OnPropertyChanged("SelectedPlayer");
        }
        void _refreshPlayersFunc() 
        {
            _client.ReceiveMessage();
            _client.SendKey('1');
            _client.ReceiveMessage();
            string playersInServerLines = _client.ReceiveMessage()[1..];
            AllPlayers = _parseObservableCollectionFromServerString(playersInServerLines);
            OnPropertyChanged("AllPlayers");
        }
        ObservableCollection<Player> _parseObservableCollectionFromServerString(string s)
        {
            List<string> lines = new List<string>(s.Split('\n'));
            ObservableCollection<Player> allPlayers = new();
            foreach(string line in lines)
            {
                if (line != "")
                {
                    allPlayers.Add(_parsePlayerFromServerLine(line));
                }
            }
            return allPlayers;
        }
        Player _parsePlayerFromServerLine(string s)
        {
            return new Player(int.Parse(s.Substring(0, 4).Trim()),
                               s.Substring(4, 20).Trim(),
                               s.Substring(24, 20).Trim(),
                               s.Substring(44, 10).Trim(),
                               s.Substring(54, 15).Trim(),
                               s.Substring(69, 25).Trim(),
                               int.Parse(s.Substring(94, 5).Trim()),
                               int.Parse(s.Substring(99, 9).Trim()),
                               bool.Parse(new List<string>(s.Split(' ', StringSplitOptions.RemoveEmptyEntries)).Last()));
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
