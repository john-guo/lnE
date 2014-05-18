using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

namespace lnE
{
    [ComicDish("http://www.dm5.com/", Level = 3)]
    public class dm5Dish : ComicDish2
    {
        public override List<PageIndex> GetChapterIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();

            var nodes = html.DocumentNode.SelectNodes("//div[@id='index_mian']/div[@id='index_left']/div[@class='ma5']/div/ul[@class='nr6 lan2']");
            foreach (var node in nodes)
            {
                var pnodes = node.SelectNodes("./li");
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

        public override List<PageIndex> GetImagePageIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();

            var script = html.DocumentNode.SelectSingleNode("//script[boolean(@src)=false]").InnerText;
            var ex = new Regex("var\\sDM5_CID=(\\d+);");
            var match = ex.Match(script);
            if (match.Groups.Count != 2)
                return null;
            var cid = match.Groups[1].Value;
            ex = new Regex("var\\sDM5_IMAGE_COUNT=(\\d+);");
            match = ex.Match(script);
            if (match.Groups.Count != 2)
                return null;
            var count = Convert.ToInt32(match.Groups[1].Value);

            //TODO? Some comics need a dm5_key to get images url??
            //script = html.DocumentNode.SelectNodes("//body/div[@class='view_fy hui6']/script[boolean(@src)=false]").First().InnerText;
            //script = script.Replace("eval", "var a=");
            //script = AssemblyHelper.EvalJs<string>(script, "a");
            //script = script.Split(';')[1];
            //script = Regex.Replace(script, "var\\s[^=]+\\=", "var a=");
            //var key = AssemblyHelper.EvalJs<string>(script, "a");

            var key = String.Empty;
            for (var i = 1; i <= count; ++i)
            {
                var url = String.Format("http://www.dm5.com/m{0}/imagefun.ashx?cid={0}&key={1}&page={2}&language=1", cid, key, i);
                pages.Add(new PageIndex() { name = i.ToString(), url = url });
            }

            return pages;
        }

        public override List<PageIndex> GetImageIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();
            var script = (string)html.GetType().GetField("Text", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(html);
            var url = AssemblyHelper.EvalJs2(script, "d[0]");
            var n = GetIndexedName(url);
            pages.Add(new PageIndex() { name = n, url = url });

            return pages;
        }

    }
}
