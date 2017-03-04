using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingClient
{
    public class ClientStatusOnServerEventArgs : EventArgs
    {
        public ClientStatusOnServerEventArgs(bool connected, string userName)
        {
            Connected = connected;
            UserName = userName;
        }

        public bool Connected { get; }
        public string UserName { get; }
    }
}
