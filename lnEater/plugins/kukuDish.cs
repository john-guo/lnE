using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;

namespace lnE
{
    [ComicDish("http://www.kukudm.com/comiclist/", Level = 3)]
    public class kukuDish : ComicDish2
    {
        const string server = "http://n.kukudm.com/";

        public override List<PageIndex> GetChapterIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();

            var nodes = html.DocumentNode.SelectNodes(".//*[@id='comiclistn']/dd");
            foreach (var node in nodes)
            {
                var link = node.SelectNodes("./a").First();
                var url = link.GetAttributeValue("href", String.Empty);
                var name = link.InnerText;

                pages.Add(new PageIndex() { name = name, url = url });
            }

            return pages;
        }

        public override List<PageIndex> GetImagePageIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();
            var node = html.DocumentNode.SelectSingleNode("//body/table[2]/tr/td");
            var ret = AssemblyHelper.ParseString("共(\\d+)页", node.InnerText);
            if (ret == null)
                return null;

            var count = Convert.ToInt32(ret);

            var uri = new Uri(baseUrl);
            var file = uri.LocalPath;

            for (var i = 1; i <= count; ++i) 
            {
                var href = Path.Combine(Path.GetDirectoryName(file), Path.ChangeExtension(i.ToString(), Path.GetExtension(file)));
                var url = GetUrl(baseUrl, href);
                pages.Add(new PageIndex() { name = i.ToString(), url = url });
            }
            return pages;
        }

        public override List<PageIndex> GetImageIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();
            
            var script = html.DocumentNode.SelectNodes("//body/table[2]/tr/td/script[boolean(@src)=false]").First().InnerText;
            var href = AssemblyHelper.ParseString("document.write\\(\\\"\\<img id\\=comicpic name\\=comicpic src\\=\\'\\\"\\+server\\+\\\"(.*)\\'\\>\\<span", script);

            var u = server + href;
            var n = GetIndexedName(u);

            pages.Add(new PageIndex() { name = n, url = u });

            return pages;
        }
    }
}
