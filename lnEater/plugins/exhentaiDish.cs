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

        public override List<PageIndex> GetImageIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();
            try
            {
                pages = base.GetImageIndex(html, baseUrl);
                (new WebClient()).DownloadData(pages.First().url);
            }
            catch
            {
                var script = html.DocumentNode.SelectNodes("//script[boolean(@src)=false]").Last().InnerText;

                var gid = AssemblyHelper.EvalJs2(script, "gid");
                var page = AssemblyHelper.EvalJs2(script, "startpage");
                var imgkey = AssemblyHelper.EvalJs2(script, "startkey");
                var showkey = AssemblyHelper.EvalJs2(script, "showkey");

                var temp = new WebClient();
                var cookie = String.Format("ipb_member_id={0};ipb_pass_hash={1}", id, pass);
                temp.Headers.Add(HttpRequestHeader.Cookie, cookie);
                temp.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                var post = String.Format("{{\"method\":\"showpage\", \"gid\":{0}, \"page\":{1}, \"imgkey\":\"{2}\", \"showkey\":\"{3}\"}}", gid, page, imgkey, showkey);

                var result = temp.UploadData("http://exhentai.org/api.php", Encoding.UTF8.GetBytes(post));
                var json = Encoding.UTF8.GetString(result);
                var htm = AssemblyHelper.EvalJs2("var a=" + json, "a.i3");
                var doc = new HtmlDocument();
                doc.LoadHtml(htm);
                var u = doc.DocumentNode.SelectSingleNode("//img[@id='img']").GetAttributeValue("src", String.Empty);
                var n = Path.GetFileName(new Uri(u).AbsolutePath);

                pages.Add(new PageIndex() { name = n, url = u });
            }

            return pages;
        }

        //b.setRequestHeader("Content-Type","application/json"); exhentai.org/api.php
        //
        protected override void BeforeRequestHentai(WebClient client, string url, uint level, int tryCount)
        {
            var cookie = String.Format("ipb_member_id={0};ipb_pass_hash={1}", id, pass);

            client.Headers.Add(HttpRequestHeader.Cookie, cookie);

            client.Proxy = new WebProxy("127.0.0.1", 8087);
            if (tryCount < 3)
                client.Proxy = null;
        }
    }
}
