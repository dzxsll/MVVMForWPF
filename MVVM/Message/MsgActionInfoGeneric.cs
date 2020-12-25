using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Message
{
    internal class MsgActionInfo<T> : MsgActionInfo
    {
        public new Action<T> Action { get; internal set; }

        public new void Execute()
        {
            Execute(default(T));
        }

        public void Execute(T args)
        {
            Action?.Invoke(args);
        }
    }
}