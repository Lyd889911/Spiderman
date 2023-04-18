using OpenQA.Selenium.Chrome;
using Spiderman.HTTP.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spiderman.HTTP
{
    public class HttpOptionBuilder
    {
        #region 属性
        public HttpMethod Method { get;private set; } = HttpMethod.Get;
        public string Url { get; private set; }
        public HttpRequestMessage HttpRequestMessage { get; private set; }
        public WebProxy Proxy { get; private set; }
        public bool HasProxy { get; private set; }
        public HttpClientHandler HttpClientHandler { get;private set; }
        /// <summary>
        /// 请求方式，用api请求或者驱浏览器请求
        /// </summary>
        public RequestMode RequestMode { get; set; } = RequestMode.API;
        /// <summary>
        /// 浏览器类型
        /// </summary>
        public BrowserType BrowserType { get; set; } = BrowserType.Chrome;
        #region 谷歌浏览器
        public ChromeOptions ChromeOptions { get; private set; }
        #endregion
        #endregion

        public HttpOptionBuilder(
            string url,
            string method = "GET")
        {
            switch (method.ToUpper())
            {
                case "GET": Method = HttpMethod.Get;break;
                case "POST": Method = HttpMethod.Post;break;
                case "PUT": Method = HttpMethod.Put;break;
            }
            Url = url;
            HttpRequestMessage = new HttpRequestMessage(Method, Url);
            HttpClientHandler = new HttpClientHandler();
            ChromeOptions = new ChromeOptions();
        }
        /// <summary>
        /// 添加请求头
        /// </summary>
        public HttpOptionBuilder AddHeader(string key,string value)
        {
            HttpRequestMessage.Headers.Add(key, value);
            return this;
        }
        /// <summary>
        /// 添加代理
        /// </summary>
        public HttpOptionBuilder AddProxy(string proxy)
        {
            if (!Regex.IsMatch(proxy, @"^https?://"))
                proxy = "http://" + proxy;
            Proxy = new WebProxy(proxy);
            HttpClientHandler.Proxy = Proxy;
            ChromeOptions.AddArgument($"--proxy-server={proxy}"); 
            HasProxy = true;
            return this;
        }
    }
}
