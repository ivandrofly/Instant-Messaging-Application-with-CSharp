using ChattingInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChattingClient
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ClientCallback : IClient
    {
        public event EventHandler<ClientStatusOnServerEventArgs> ClientLoginLogoff;

        public void GetMessage(string message, string senderUserName)
        {
            // System.Windows.MessageBox.Show()
            ((MainWindow)Application.Current.MainWindow).TakeMessage(message, senderUserName);
        }

        public void GetUpdate(int value, string userName)
        {
            OnClientLoginLogoff(value == 0, userName);
        }

        protected void OnClientLoginLogoff(bool connected, string userName)
        {
            ClientLoginLogoff?.Invoke(this, new ClientStatusOnServerEventArgs(connected, userName));
        }

        public void PlaceHolder()
        {
            throw new NotImplementedException();
        }
    }
}
