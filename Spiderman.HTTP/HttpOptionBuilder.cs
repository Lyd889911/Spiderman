using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Spiderman.HTTP
{
    public class HttpOptionBuilder
    {
        private HttpMethod _method = HttpMethod.Get;
        private string _url;
        private HttpRequestMessage _httpRequestMessage;
        private WebProxy _proxy;
        private HttpClientHandler _httpClientHandler;
        public HttpOptionBuilder(string url,string method = "GET")
        {
            switch (method.ToUpper())
            {
                case "GET":_method=HttpMethod.Get;break;
                case "POST":_method=HttpMethod.Post;break;
                case "PUT":_method=HttpMethod.Put;break;
            }
            _httpRequestMessage = new HttpRequestMessage(_method,url);
        }
        public HttpOptionBuilder AddHeader(string key,string value)
        {
            _httpRequestMessage.Headers.Add(key, value);
            return this;
        }
        public HttpOptionBuilder AddProxy(string proxy)
        {
            _proxy = new WebProxy(proxy);
            _httpClientHandler = new HttpClientHandler { Proxy = _proxy };
            HasProxy = true;
            return this;
        }
        public HttpRequestMessage HttpRequestMessage { get { return _httpRequestMessage; } }
        public HttpClientHandler HttpClientHandler { get { return _httpClientHandler; } }
        public bool HasProxy { get;private set; }
    }
}
