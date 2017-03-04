using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChattingInterfaces
{
    public interface IClient
    {
        [OperationContract]
        void PlaceHolder();

        [OperationContract]
        void GetMessage(string message, string senderUserName);

        [OperationContract]
        void GetUpdate(int value, string userName);
    }
}
