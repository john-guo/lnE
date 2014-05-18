using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;

namespace lnE
{
    [ComicDish("http://www.bengou.cm/cartoon/")]
    public class bengouDish : ComicDish
    {
        public override List<PageIndex> GetChapterIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();

            var nodes = html.DocumentNode.SelectNodes(".//div[@class='section-list mark']");
            foreach (var node in nodes)
            {
                var pnodes = node.SelectNodes("./span/a");
                foreach (var pnode in pnodes)
                {
                    var link = pnode;
                    var url = link.GetAttributeValue("href", String.Empty);
                    var name = link.GetAttributeValue("title", String.Empty);

                    pages.Add(new PageIndex() { name = name, url = url });
                }
            }

            return pages;
        }

        public override List<PageIndex> GetImageIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();

            var script = html.DocumentNode.SelectNodes("//body/script[boolean(@src)=false]").Last().InnerText;
            var js = "var a=[];for(var i in picTree)a.push(pic_base+picTree[i]);a";
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
