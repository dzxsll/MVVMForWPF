using MVVM.Message;
using MVVM.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MVVM.ViewModel
{
    public class ViewModelBase : NotifyObject
    {
        private IMessageManager _msgManager;
        private Dispatcher _UIDispatcher;

        public IMessageManager MsgManager
        {
            get
            {
                if (_msgManager == null)
                    _msgManager = MessageManager.Default;
                return _msgManager;
            }

            set { _msgManager = value; }
        }

        public Dispatcher UIDispatcher
        {
            get
            {
                if (_UIDispatcher == null)
                    _UIDispatcher = DispatcherHelper.UIDispatcher;
                return _UIDispatcher;
            }
            set
            {
                _UIDispatcher = value;
            }
        }
    }
}