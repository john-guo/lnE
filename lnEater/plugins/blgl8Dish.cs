using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;

namespace lnE
{
    [ComicDish("http://dx.blgl8.com/comic-i/")]
    public class blgl8Dish : ComicDish
    {
        const string server = "http://mh.jmymh.jmmh.net:2012/";

        /*
         * 電信1 http://mh.jmymh.jmmh.net:2012/
         * 網通  http://lt.jmydm.jmmh.net:2011/
         * 電信2 http://mh.jmmh.net:2011/"
        */

        public override List<PageIndex> GetChapterIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();

            var nodes = html.DocumentNode.SelectNodes("//div[@class='cVol']/div[@class='cVolList']");
            foreach (var node in nodes)
            {
                var links = node.SelectNodes("./div/a[1]");
                foreach (var link in links)
                {
                    var url = link.GetAttributeValue("href", String.Empty);
                    var name = link.InnerText;

                    pages.Add(new PageIndex() { name = name, url = url });
                }
            }

            return pages;
        }

        public override List<PageIndex> GetImageIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();

            var script = html.DocumentNode.SelectNodes("//head/script[boolean(@src)=false]").First().InnerText;
            var js = String.Format("var arrFiles = sFiles.split('|');var a=[];for(var j=0;j<arrFiles.length;++j)a.push(sPath+arrFiles[j]);a");
            var result = AssemblyHelper.EvalJs2(script, js);

            int i = 1;
            foreach (var a in result)
            {
                var u = String.Format("{0}{1}", server, a);
                var n = GetIndexedName(i++, u);
                pages.Add(new PageIndex() { name = n, url = u });
            }

            return pages;
        }
    }
}
