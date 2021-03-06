﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;

namespace lnE
{
    [ComicDish("http://www.imanhua.com/comic/")]
    public class imanhuaDish : ComicDish
    {
        const string server = "t4.mangafiles.com";

        /*
         'servs': [
        { 'name': '电信①', 'host': 't4.mangafiles.com' },
        { 'name': '电信②', 'host': 't5.mangafiles.com' },
        { 'name': '电信③', 'host': 't6.mangafiles.com' },
        { 'name': '广东电信', 'host': 't6.mangafiles.com' },
        { 'name': '江苏电信', 'host': 't5.mangafiles.com' },
        { 'name': '联通', 'host': 'c5.mangafiles.com' }
        ],
        */

        public override List<PageIndex> GetChapterIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();

            var nodes = html.DocumentNode.SelectNodes("//div[@id='subBookBox']/ul[@id='subBookList']/li");
            foreach (var node in nodes)
            {
                var link = node.SelectSingleNode(".//a");
                var url = link.GetAttributeValue("href", String.Empty);
                var name = link.GetAttributeValue("title", String.Empty);

                pages.Add(new PageIndex() { name = name, url = url });
            }

            return pages;
        }

        public override List<PageIndex> GetImageIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();

            var script = html.DocumentNode.SelectSingleNode("//head/script[boolean(@src)=false]").InnerText;
            var cInfo = AssemblyHelper.EvalJs(script);
            
            var e = cInfo["cid"] > 7910 ? "/Files/Images/" + cInfo["bid"] + "/" + cInfo["cid"] + "/" : "";
            for (var i = 0; i < cInfo["files"].length; ++i)
            {
                var a = cInfo["files"][i];
                var u = String.Format("http://{0}{1}{2}", server, e, a);
                var n = GetIndexedName(i + 1, u);
                pages.Add(new PageIndex() { name = n, url = u });
            }

            return pages;
        }
    }
}
