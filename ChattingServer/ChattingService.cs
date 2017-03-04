using ChattingInterfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChattingServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class ChattingService : IChattingService
    {
        // Thread safe dictionary. Will manage all user connected to the server.
        public ConcurrentDictionary<string, ConnectedClient> _connectedClients = new ConcurrentDictionary<string, ConnectedClient>();

        public int Login(string userName, string pwd)
        {
            foreach (var client in _connectedClients)
            {
                // Someone already logged in with same user name.
                if (client.Key.ToLower() == userName.ToLower())
                {
                    return 1;
                }
            }

            // ther the channel of current connected user's channel.
            var establishedUserConnectino = OperationContext.Current.GetCallbackChannel<IClient>();
            var newClient = new ConnectedClient();
            newClient.Connection = establishedUserConnectino;
            newClient.UserName = userName;
            _connectedClients.TryAdd(userName, newClient);
            UpdateHelper(0, newClient.UserName);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Client login: {newClient.UserName} at {DateTime.Now}");
            Console.ResetColor();


            // Login successfully
            return 0;
        }

        public void Logout()
        {
            ConnectedClient connectedClient = GetConnectedClient();
            if (connectedClient == null)
            {
                return;
            }
            _connectedClients.TryRemove(connectedClient.UserName, out ConnectedClient removedClient);

            UpdateHelper(1, connectedClient.UserName);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Client logoff: {removedClient.UserName} at {DateTime.Now}");
            Console.ResetColor();
        }

        public ConnectedClient GetConnectedClient()
        {
            // ther the channel of current connected user's channel.
            var establishedUserConnectino = OperationContext.Current.GetCallbackChannel<IClient>();
            foreach (var client in _connectedClients)
            {
                if (client.Value.Connection == establishedUserConnectino)
                {
                    return client.Value;
                }
            }
            return null;
        }

        public void NotifyAll(string message, string userName)
        {
            foreach (var client in _connectedClients)
            {
                if (client.Key != userName.ToLower())
                {
                    client.Value.Connection.GetMessage(message, userName);
                }
            }
        }

        public void TestConnectin(string value)
        {
            Console.WriteLine(value);
        }

        /// <summary>
        /// Notify all the clients when a user login/logoff.
        /// </summary>
        /// <param name="value">1 if user logoff and 0 if login.</param>
        /// <param name="userName"></param>
        private void UpdateHelper(int value, string userName)
        {
            foreach (var client in _connectedClients)
            {
                if (client.Value.UserName.ToLower() != userName.ToLower())
                {
                    client.Value.Connection.GetUpdate(value, userName);
                }
            }
        }

        public IList<string> GetConnectedUsers()
        {
            var connectedUser = new List<string>();
            foreach (var client in _connectedClients)
            {
                connectedUser.Add(client.Value.UserName);
            }
            return connectedUser;
        }
    }
}


// NOTE: MUST RUNN AS ADMIN IN ORDER TO HOST THE SERVER.