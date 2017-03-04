using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChattingInterfaces
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IChattingService" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(IClient))]
    public interface IChattingService
    {
        [OperationContract]
        int Login(string userName, string pwd);

        [OperationContract]
        void TestConnectin(string value);

        [OperationContract]
        void NotifyAll(string message, string userName); // username is to prevent sending message to your self.

        [OperationContract]
        void Logout();

        [OperationContract]
        IList<string> GetConnectedUsers();
    }
}
