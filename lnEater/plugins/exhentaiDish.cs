using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using System.Net;

namespace lnE
{
    [ComicDish("http://exhentai.org/", Level = 3)]
    public class exhentaiDish : ehentaiDish
    {
        const string id = "";
        const string pass = "";
       
        protected override void BeforeRequestHentai(WebClient client, string url, uint level, int tryCount)
        {
            var cookie = String.Format("ipb_member_id={0};ipb_pass_hash={1}", id, pass);

            client.Headers.Add(HttpRequestHeader.Cookie, cookie);
        }
    }
}
