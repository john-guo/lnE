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
    [ComicDish("http://g.e-hentai.org/g/", Level = 3)]
    public class ehentaiDish : ComicDish2
    {
        public override List<PageIndex> GetChapterIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();

            var links = html.DocumentNode.SelectNodes("//table[@class='ptt']//a");
            var sets = new HashSet<string>();
            foreach (var link in links)
            {
                var url = link.GetAttributeValue("href", String.Empty);
                var name = link.InnerText;

                if (sets.Contains(url))
                    continue;

                sets.Add(url);

                pages.Add(new PageIndex() { name = name, url = url });
            }

            return pages;
        }

        public override List<PageIndex> GetImagePageIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();
            var links = html.DocumentNode.SelectNodes("//div[@id='gdt']/div[@class='gdtm']//a");

            foreach (var link in links)
            {
                var url = link.GetAttributeValue("href", String.Empty);
                var name = String.Empty;

                pages.Add(new PageIndex() { name = name, url = url });
            }

            return pages;
        }

        public override List<PageIndex> GetImageIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();

            var u = html.DocumentNode.SelectSingleNode("//div[@class='sni']/a[1]/img").GetAttributeValue("src", String.Empty);
            var n = Path.GetFileName(new Uri(u).AbsolutePath);

            pages.Add(new PageIndex() { name = n, url = u });

            return pages;
        }

        protected override bool BeforeRequest(WebClient client, string url, uint level)
        {
            client.Proxy = new WebProxy("127.0.0.1", 8087);
            return base.BeforeRequest(client, url, level);
        }
    }
}
