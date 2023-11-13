using System;
using Server;

namespace ClientWPF6
{
    public class EventAggregator
    {
        public event Action<Player> DataReady;
        public void SendData(Player p)
        {
            DataReady?.Invoke(p);
        }
    }
}
