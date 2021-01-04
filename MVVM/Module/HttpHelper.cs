using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Module
{
    public class HttpHelper
    {
        public class Req<T>
        {
            /// <summary>
            /// 传入数据
            /// </summary>
            public T Input { get; set; }

            /// <summary>
            /// 请求地址
            /// </summary>
            public string Url { get; set; }
        }

        public class Resp<T>
        {
            /// <summary>
            /// 错误信息
            /// </summary>
            public string ErrorMsg { get; set; }

            /// <summary>
            /// 状态码
            /// </summary>
            public int StatusCode { get; set; }

            /// <summary>
            /// 返回数据
            /// </summary>
            public T RespData { get; set; }
        }

        #region Version:HttpCline

        private static readonly string _baseAddress = ConfigurationManager.AppSettings.Get("http-server");
        private static readonly string _robotName = ConfigurationManager.AppSettings.Get("RobotSN");

        private static readonly HttpClient _httpClient;
        private static readonly HttpClient _fileClient;

        static HttpHelper()
        {
            try
            {
                #region 初始化和预热 httpClient

                //基本设置
                _httpClient = new HttpClient();
                _httpClient.BaseAddress = new Uri(_baseAddress);
                _httpClient.Timeout = TimeSpan.FromMilliseconds(2000);
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/xml");

                _httpClient.SendAsync(new HttpRequestMessage
                {
                    Method = new HttpMethod("HEAD"),
                    RequestUri = new Uri(_baseAddress)
                }).Result.EnsureSuccessStatusCode();

                #endregion 初始化和预热 httpClient

                #region 初始化和预热 fileClient

                _fileClient = new HttpClient();
                _fileClient.BaseAddress = new Uri(_baseAddress);
                _fileClient.MaxResponseContentBufferSize = 256000;

                #endregion 初始化和预热 fileClient
            }
            catch (Exception ex)
            {
                if (ex is AggregateException)
                {
                    AggregateException ae = (AggregateException)ex;
                    ae.Handle(p =>
                    {
                        LogService.Info("From httpHelper() " + ae.InnerException);
                        return true;
                    });
                }

                LogService.Info("From httpHelper() " + ex.Source + ex.Message);
            }
        }

        /// <summary>
        /// http Get请求
        /// </summary>
        /// <typeparam name="T">入参类型</typeparam>
        /// <typeparam name="TResult">出参类型</typeparam>
        /// <param name="req">入参对象</param>
        /// <returns></returns>
        public static async Task<Resp<TResult>> GetAsync<T, TResult>(Req<T> req)
        {
            try
            {
                var result = await _httpClient.GetAsync(req.Url).Result.Content.ReadAsStringAsync();
                return new Resp<TResult>() { RespData = JsonConverter.Deserialize<TResult>(result) };
            }
            catch (Exception ex)
            {
                LogService.Info("From http_GET()" + ex.Message);
            }

            return new Resp<TResult>() { RespData = JsonConverter.Deserialize<TResult>("") };
        }

        /// <summary>
        /// http Post请求
        /// </summary>
        /// <typeparam name="T">入参类型</typeparam>
        /// <typeparam name="TResult">出参类型</typeparam>
        /// <param name="req">入参对象</param>
        /// <returns></returns>
        public static async Task<Resp<TResult>> PostAsJsonAsync<T, TResult>(Req<T> req)
        {
            HttpContent data = new StringContent(JsonConverter.Serialize<T>(req.Input));

            var result = await _httpClient.PostAsync(req.Url, data).Result.Content.ReadAsStringAsync();

            return new Resp<TResult>() { RespData = JsonConverter.Deserialize<TResult>(result) };
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="req"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task<Resp<TResult>> SendFile<T, TResult>(Req<T> req, string filePath, string function, string contentType)
        {
            try
            {
                //读文件流
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    //为文件流提供HTTP容器
                    HttpContent fileContent = new StreamContent(fs);
                    //设置媒体类型
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
                    //配置传递参数容器
                    MultipartFormDataContent mulContent = new MultipartFormDataContent("----");
                    string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                    //第二个是表单名，第三个是对应的值
                    mulContent.Add(fileContent, "file", fileName);
                    mulContent.Add(new StringContent(_robotName), "robotName");
                    mulContent.Add(new StringContent(function), "function");

                    HttpResponseMessage response = await _fileClient.PostAsync(req.Url, mulContent);
                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();
                    return new Resp<TResult>() { RespData = JsonConverter.Deserialize<TResult>(result) };
                }
            }
            catch (Exception ex)
            {
                LogService.Info(ex.Message + "From SendFile()");
            }
            finally
            {
            }

            return null;
        }

        public static async Task<Resp<byte[]>> HttpDownloadData<T>(Req<T> req)
        {
            var byteres = await _fileClient.GetByteArrayAsync(req.Url);
            return new Resp<byte[]> { RespData = byteres };
        }

        #endregion Version:HttpCline
    }
}