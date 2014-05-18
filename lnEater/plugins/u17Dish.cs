using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;

namespace lnE
{
    [ComicDish("http://www.u17.com/comic/")]
    public class u17Dish : ComicDish
    {
        const string server = "http://imgfast.dmzj.com/";
        /*
            cfg: {
            comic_site_host : "http://manhua.dmzj.com/",
            fast_img_host : "http://imgfast.dmzj.com/",
            fast_tel_img_host : "http://imgfastd.dmzj.com/",
            fast_cnc_img_host : "http://imgfastw.dmzj.com/",
            fast_img_host1 : "http://imgfast0.dmzj.com/",
            fast_tel_img_host1 : "http://imgfastd0.dmzj.com/",
            fast_cnc_img_host1 : "http://imgfastw0.dmzj.com/",
            tel_img_host : "http://imgd.dmzj.com/",
            cnc_img_host : "http://imgw.dmzj.com/",
            hot_img_host : "http://hot.dmzj.com/",
            img_host : "http://img.dmzj.com/"
        },
        */

        public override List<PageIndex> GetChapterIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();

            var nodes = html.DocumentNode.SelectNodes(".//ul[@id='chapter']/li");
            foreach (var node in nodes)
            {
                var link = node.SelectSingleNode("./a");
                var url = link.GetAttributeValue("href", String.Empty);
                var name = link.GetAttributeValue("title", String.Empty);

                pages.Add(new PageIndex() { name = name, url = url });
            }

            return pages;
        }

        public override List<PageIndex> GetImageIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();

            var script = html.DocumentNode.SelectNodes("//head/script[boolean(@src)=false]").Last().InnerText;
            script = "var $={};$.evalJSON = function(s) { return eval('(' + s + ')'); };" + script;
            var js = "var a=[];for(var i in image_config.image_list)a.push(image_config.image_list[i].src);a";
            var result = AssemblyHelper.EvalJs2(script, js);

            int i = 1;
            foreach (var a in result)
            {
                var u = Encoding.ASCII.GetString(Convert.FromBase64String(a));
                var n = GetIndexedName(i++, u);
                pages.Add(new PageIndex() { name = n, url = u });
            }

            return pages;
        }
    }
}
