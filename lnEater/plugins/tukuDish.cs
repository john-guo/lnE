using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;

namespace lnE
{
    [ComicDish("http://www.tuku.cc/comic/")]
    public class tukuDish : ComicDish
    {
        const string server = "pic.tuku.cc:8899";

        public override List<PageIndex> GetChapterIndex(HtmlDocument html)
        {
            var pages = new List<PageIndex>();

            var nodes = html.DocumentNode.SelectNodes("//div[@class='chapterBox']/div[@class='content']/ul/li");
            foreach (var node in nodes)
            {
                var link = node.SelectSingleNode("./span[@class='t']/a");
                var url = link.GetAttributeValue("href", String.Empty);
                var name = link.InnerText;

                pages.Add(new PageIndex() { name = name, url = url });
            }

            return pages;
        }

        public override List<PageIndex> GetImageIndex(HtmlDocument html)
        {
            var pages = new List<PageIndex>();

            var script = html.DocumentNode.SelectNodes("//head/script[boolean(@src)=false]").Last().InnerText;
            var js = String.Format("serverUrl='{0}';var a=[];for(var j=1;j<=pages;++j)a.push(getImgUrl(j));a", server);
            var result = AssemblyHelper.EvalJs2(script, js);

            int i = 0;
            foreach (var a in result)
            {
                var u = String.Format("http://{0}", a);
                var n = GetIndexedName(i++, u);
                pages.Add(new PageIndex() { name = n, url = u });
            }

            return pages;
        }
    }
}
