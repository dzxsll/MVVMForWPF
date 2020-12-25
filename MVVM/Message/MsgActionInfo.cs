using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Message
{
    internal class MsgActionInfo
    {
        public object RegInstance { get; internal set; }

        public string MsgName { get; internal set; }

        public string Group { get; internal set; }

        public Action Action { get; internal set; }

        public void Execute()
        {
            Action?.Invoke();
        }
    }
}