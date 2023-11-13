using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Server;

namespace ClientWPF.Views
{
    public class PlayersListView
    {
        public List<Player>;
        public int PlayerID;
        
        public ICommand AddPlayer;
        public ICommand RemovePlayer;
        public ICommand FindPlayer;
        public ICommand RefreshPlayers;
    }
}
