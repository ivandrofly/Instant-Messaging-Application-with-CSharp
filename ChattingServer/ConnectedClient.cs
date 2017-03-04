﻿using ChattingInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingServer
{
    public class ConnectedClient
    {
        public IClient Connection { get; set; } // Call back contract
        public string UserName { get; set; }
    }
}
