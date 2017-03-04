using ChattingInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

namespace ChattingClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IChattingService _server;
        private static DuplexChannelFactory<IChattingService> _channelFactory;

        public MainWindow()
        {
            InitializeComponent();
            var clientCallback = new ClientCallback();
            clientCallback.ClientLoginLogoff += ClientCallback_ClientLoginLogoff;
            _channelFactory = new DuplexChannelFactory<IChattingService>(clientCallback, "ChattingServiceEndPoint");
            _server = _channelFactory.CreateChannel();
        }

        private void ClientCallback_ClientLoginLogoff(object sender, ClientStatusOnServerEventArgs e)
        {
            // user connected
            if (e.Connected)
            {
                listBoxConnectedClients.Items.Add(e.UserName);
            }
            else
            {
                listBoxConnectedClients.Items.Remove(e.UserName);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // _server.TestConnectin("Tadaa!!!");
        }

        public void TakeMessage(string message, string userName)
        {
            textBoxMessage.Text += $"{userName}: {message}\n";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _server.NotifyAll(textBoxUserMessage.Text, textBoxUserName.Text);
            TakeMessage(textBoxUserMessage.Text, "You");
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            int returnValue = _server.Login(textBoxUserName.Text, "123");
            if (returnValue == 1)
            {
                MessageBox.Show("Already logged in!!!");
            }
            else if (returnValue == 0)
            {
                textBoxUserName.IsEnabled = false;
                Login.IsEnabled = false;
                LoadUsers();
            }
        }

        private void LoadUsers()
        {
            foreach (string userName in _server.GetConnectedUsers())
            {
                listBoxConnectedClients.Items.Add(userName);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _server.Logout();
        }
    }
}
