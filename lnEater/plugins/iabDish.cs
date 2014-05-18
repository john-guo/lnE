using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;

namespace lnE
{
    [ComicDish("http://i-ab.com/mh/")]
    public class iabDish : ComicDish
    {
        public override List<PageIndex> GetChapterIndex(HtmlDocument html)
        {
            var pages = new List<PageIndex>();

            var nodes = html.DocumentNode.SelectNodes("//body/div[@class='clearmain']/div[@class='w664 fr home']/div[@class='box mh-directory']/div[@class='box-bd']/div[@class='tabs']/div[@class='tabs-bd']/div[@class='tabs-panel']");
            foreach (var node in nodes)
            {
                var pnodes = node.SelectNodes("./ul/li");
                foreach (var pnode in pnodes)
                {
                    var link = pnode.SelectSingleNode("./a");
                    var url = link.GetAttributeValue("href", String.Empty);
                    var name = link.GetAttributeValue("title", String.Empty);

                    pages.Add(new PageIndex() { name = name, url = url });
                }
            }

            return pages;
        }

        public override List<PageIndex> GetImageIndex(HtmlDocument html)
        {
            var pages = new List<PageIndex>();

            var script = html.DocumentNode.SelectNodes("//body/script[boolean(@src)=false]").First().InnerText;
            var js = "function getPageIndex(){return 0;}var a=[];for(var i in picTree)a.push(pic_base+picTree[i]);a";
            var result = AssemblyHelper.EvalJs2(script, js);

            int i = 1;
            foreach (var a in result)
            {
                var u = a;
                var n = GetIndexedName(i++, u);
                pages.Add(new PageIndex() { name = n, url = u });
            }

            return pages;
        }
    }
}
