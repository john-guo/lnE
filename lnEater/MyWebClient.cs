using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace lnE
{
    public class MyWebClient : WebClient
    {
        int connectionTimeout = 30000;
        int downloadTimeout = 60000;

        public MyWebClient() : base()
        {
            int.TryParse(ConfigurationManager.AppSettings["connectionTimeout"], out connectionTimeout);
            int.TryParse(ConfigurationManager.AppSettings["downloadTimeout"], out downloadTimeout);
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);

            request.Timeout = connectionTimeout;
            request.ReadWriteTimeout = downloadTimeout;

            return request;
        }
    }
}
