using MVVM.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM.ViewModel
{
    public class ViewModelManager
    {
        private static List<ViewModelInfo> _viewModelInfoList = new List<ViewModelInfo>();

        public static void Register<TView, TViewModel, TMsgRegister>(string token = "")
        {
            var vmInfo = new ViewModelInfo(
                typeof(TView),
                typeof(TViewModel),
                typeof(TMsgRegister),
                token);
            _viewModelInfoList.Add(vmInfo);
        }

        public static void Register<TView, TViewModel>(string token = "")
        {
            var vmInfo = new ViewModelInfo(
                typeof(TView),
                typeof(TViewModel),
                token: token);
            _viewModelInfoList.Add(vmInfo);
        }

        private static ViewModelInfo GetViewModelInfo(Type viewType, string token = "")
        {
            try
            {
                return _viewModelInfoList.
                    Where(p => p.ViewType == viewType
                            && p.Token == token).
                    First();
            }
            catch
            {
                return null;
            }
        }

        public static object GetViewModel<TView>(string token = "")
        {
            try
            {
                var vmType = GetViewModelInfo(typeof(TView), token).ViewModelType;
                return vmType.Assembly.CreateInstance(vmType.FullName);
            }
            catch
            {
                return null;
            }
        }

        public static void SetViewModel(FrameworkElement view, bool isGlobalMsg = false, string token = "")
        {
            var vmInfo = GetViewModelInfo(view.GetType(), token);
            if (vmInfo == null) return;

            var vm = vmInfo.GetViewModelInstance();
            //set ViewModels's UIdispatcher
            vm.UIDispatcher = view.Dispatcher;
            //set View's DataContext
            view.DataContext = vm;
            //register View's message
            var msgRegister = vmInfo.GetMsgRegisterInstance();
            if (msgRegister == null) return;

            msgRegister.RegInstance = view;
            if (isGlobalMsg)
            {
                var win = view as Window;
                if (win == null)
                {
                    throw new Exception("only can set a Window's message as global!");
                }

                vm.MsgManager = MessageManager.Default;
                win.Closed += MessageManager.Default.WindowClse;
            }
            else
            {
                vm.MsgManager = new MessageManager();
            }

            msgRegister.MsgManager = vm.MsgManager;
            msgRegister.Register();
        }
    }
}