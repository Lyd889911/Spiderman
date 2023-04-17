using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spiderman.HTTP
{
    internal class HttpClientFactory
    {
        internal HttpClient Create(HttpOptionBuilder option)
        {
            if (option.HasProxy)
                return new HttpClient(option.HttpClientHandler);
            else
                return new HttpClient();
        }
    }
}
