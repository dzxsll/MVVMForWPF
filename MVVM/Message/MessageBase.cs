﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Message
{
    public class MessageBase
    {
        public object Sender { get; private set; }

        public MessageBase(object sender = null)
        {
            Sender = sender;
        }
    }
}