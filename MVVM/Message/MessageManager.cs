using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM.Message
{
    public class MessageManager : IMessageManager
    {
        private static MessageManager _default;

        /// <summary>
        /// This static instance is used for global messages
        /// </summary>
        public static MessageManager Default
        {
            get
            {
                if (_default == null)
                    _default = new MessageManager();
                return _default;
            }
        }

        /// <summary>
        /// Save the registered message actions
        /// </summary>
        private readonly List<MsgActionInfo> _messageList = new List<MsgActionInfo>();

        public void Clear()
        {
            _messageList.Clear();
        }

        public void Register(object regInstance, string msgName, Action action, string group = "")
        {
            _messageList.Add(
                new MsgActionInfo
                {
                    RegInstance = regInstance,
                    MsgName = msgName,
                    Action = action,
                    Group = group
                });
        }

        public void Register<T>(object regInstance, string msgName, Action<T> action, string group = "")
        {
            _messageList.Add(
               new MsgActionInfo<T>
               {
                   RegInstance = regInstance,
                   MsgName = msgName,
                   Action = action,
                   Group = group
               });
        }

        public void SendMsg(string msgName, Type targetType = null, string group = "")
        {
            var actions = GetMsgActionInfo(msgName, targetType, group);
            foreach (var item in actions)
            {
                item.Execute();
            }
        }

        public void SendMsg<T>(string msgName, T msgArgs, Type targetType = null, string group = "")
        {
            var actions = GetMsgActionInfo(msgName, targetType, group);
            foreach (var item in actions)
            {
                var msgAction = item as Action<T>;
                msgAction?.Invoke(msgArgs);
            }
        }

        private IEnumerable<MsgActionInfo> GetMsgActionInfo(string msgName, Type targetType, string group)
        {
            if (targetType == null)
            {
                return _messageList.Where(m =>
                            m.MsgName == msgName
                            && m.Group == group
                            );
            }
            else
                return _messageList.Where(m =>
                            m.MsgName == msgName
                            && m.Group == group
                            && m.RegInstance.GetType() == targetType
                            );
        }

        public void UnRegister(object regInstance)
        {
            var msgActions = _messageList.Where(m => m.RegInstance == regInstance).ToList();
            foreach (var item in msgActions)
            {
                _messageList.Remove(item);
            }
        }

        /// <summary>
        /// When window close,unregister messages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void WindowClse(object sender, EventArgs e)
        {
            UnRegister(sender);
            var win = sender as FrameworkElement;
            if (win != null)
                UnRegister(win.DataContext);
        }
    }
}