using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MVVM.Threading
{
    public class DispatcherHelper
    {
        public static Dispatcher UIDispatcher { get; set; }
    }
}