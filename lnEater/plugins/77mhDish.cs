using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;

namespace lnE
{
    [ComicDish("http://www.77mh.com/", Level = 3)]
    public class _77mhDish : ComicDish2
    {
        private readonly string[] img_svrab = new string[62];

        public _77mhDish()
        {
            img_svrab[1] = "http://h62.177mh.com/h1/";
            img_svrab[2] = "http://h76.177mh.com/e2/";
            img_svrab[3] = "http://h59.177mh.com/h3/";
            img_svrab[4] = "http://h59.177mh.com/h4/";
            img_svrab[5] = "http://h59.177mh.com/h5/";
            img_svrab[6] = "http://h16.177mh.com/h6/";
            img_svrab[7] = "http://h16.177mh.com/h7/";
            img_svrab[8] = "http://h16.177mh.com/h8/";
            img_svrab[9] = "http://h16.177mh.com/h9/";
            img_svrab[10] = "http://h16.177mh.com/h10/";
            img_svrab[11] = "http://h59.177mh.com/h11/";
            img_svrab[12] = "http://h62.177mh.com/h12/";
            img_svrab[13] = "http://h16.177mh.com/h13/";
            img_svrab[14] = "http://h16.177mh.com/h14/";
            img_svrab[15] = "http://h59.177mh.com/h15/";
            img_svrab[16] = "http://h59.177mh.com/h16/";
            img_svrab[17] = "http://h16.177mh.com/h17/";
            img_svrab[18] = "http://h62.177mh.com/h18/";
            img_svrab[19] = "http://h16.177mh.com/h19/";
            img_svrab[20] = "http://h62.177mh.com/h20/";
            img_svrab[21] = "http://h62.177mh.com/h21/";
            img_svrab[22] = "http://h16.177mh.com/h22/";
            img_svrab[23] = "http://h16.177mh.com/h23/";
            img_svrab[24] = "http://h16.177mh.com/h24/";
            img_svrab[25] = "http://h16.177mh.com/h25/";
            img_svrab[26] = "http://h16.177mh.com/h26/";
            img_svrab[27] = "http://h16.177mh.com/h27/";
            img_svrab[28] = "http://h62.177mh.com/h28/";
            img_svrab[29] = "http://h59.177mh.com/h29/";
            img_svrab[30] = "http://h62.177mh.com/h30/";
            img_svrab[31] = "http://h62.177mh.com/h31/";
            img_svrab[32] = "http://h62.177mh.com/h32/";
            img_svrab[33] = "http://h62.177mh.com/h33/";
            img_svrab[34] = "http://h62.177mh.com/h34/";
            img_svrab[35] = "http://h59.177mh.com/h35/";
            img_svrab[36] = "http://h59.177mh.com/h36/";
            img_svrab[37] = "http://h59.177mh.com/h37/";
            img_svrab[38] = "http://h70.177mh.com/h38/";
            img_svrab[39] = "http://h70.177mh.com/h39/";
            img_svrab[40] = "http://h70.177mh.com/h40/";
            img_svrab[41] = "http://h70.177mh.com/h41/";
            img_svrab[42] = "http://h70.177mh.com/h42/";
            img_svrab[43] = "http://h70.177mh.com/h43/";
            img_svrab[44] = "http://h70.177mh.com/h44/";
            img_svrab[45] = "http://h62.177mh.com/h45/";
            img_svrab[46] = "http://hc59.177mh.com/h46/";
            img_svrab[47] = "http://h70.177mh.com/h47/";
            img_svrab[48] = "http://h62.177mh.com/h48/";
            img_svrab[49] = "http://h16.177mh.com/h49/";
            img_svrab[50] = "http://h70.177mh.com/h50/";
            img_svrab[51] = "http://h16.177mh.com/h51/";
            img_svrab[52] = "http://h76.177mh.com/h52/";
            img_svrab[53] = "http://h76.177mh.com/h53/";
            img_svrab[54] = "http://afdc.177mh.com/h54/";
            img_svrab[55] = "http://h76.177mh.com/h55/";
            img_svrab[56] = "http://h76.177mh.com/h56/";
            img_svrab[57] = "http://h76.177mh.com/h57/";
            img_svrab[58] = "http://h70.177mh.com/h58/";
            img_svrab[59] = "http://h16.177mh.com/h59/";
            img_svrab[60] = "http://h59.177mh.com/h60/";
            img_svrab[61] = "http://h59.177mh.com/h61/";
        }

        public override List<PageIndex> GetChapterIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();

            var nodes = html.DocumentNode.SelectNodes(".//ul[@class='ar_list_col']/li");
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
            var node = html.DocumentNode.SelectSingleNode("//body//div[@id='main']/script[2]");

            var url = node.GetAttributeValue("src", String.Empty);
            pages.Add(new PageIndex() { name = String.Empty, url = url });

            return pages;
        }

        public override List<PageIndex> GetImageIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();

            var script = (string)html.GetType().GetField("Text", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(html);
            var a = AssemblyHelper.EvalJs2(script, "var a={};a.imgs=msg;a.server=img_s;a");

            var e = a.server;
            var imgs = ((string)a.imgs).Split('|');

            for (var i = 0; i < imgs.Length; ++i)
            {
                var u = String.Format("{0}{1}", img_svrab[e], imgs[i]);
                var n = GetIndexedName(i + 1, u);
                pages.Add(new PageIndex() { name = n, url = u });
            }

            return pages;
        }
    }
}
