using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Module
{
    public class LogService
    {
        public static void Info(string msg)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = baseDir + "Log";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string logPath = baseDir + "Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            try
            {
                using (StreamWriter sw = File.AppendText(logPath))
                {
                    sw.WriteLine("消息：" + msg);
                    sw.WriteLine("时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    sw.WriteLine("**************************************************");
                    sw.WriteLine();
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (IOException e)
            {
                using (StreamWriter sw = File.AppendText(logPath))
                {
                    sw.WriteLine("异常：" + e.Message);
                    sw.WriteLine("时间：" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"));
                    sw.WriteLine("**************************************************");
                    sw.WriteLine();
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
        }
    }
}