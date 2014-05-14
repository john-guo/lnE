using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HtmlAgilityPack;
using System.Net;
using wnd = System.Windows.Forms;
using System.Xml;
using System.Text.RegularExpressions;

namespace lnE
{
    public class Program
    {
        static void Main(string[] args)
        {
            string href = "http://www.imanhua.com/comic/4289/list_79578.html";
            string baseUrl = "http://www.imanhua.com/comic/4289/";

            Uri newUrl;
            if (!Uri.TryCreate(href, UriKind.Absolute, out newUrl))
            {
                newUrl = new Uri(new Uri(baseUrl, UriKind.Absolute), href);
            }
            var str = newUrl.AbsoluteUri;


            //Process("史上最脑残的小发明！.xml");
            //Eater.Initialize("plugins");
            //var p = new Eater();
            //p.Prepare("http://www.bilibili.tv/video/av140078/", wnd.DataFormats.Html);
            //p.Eat().Wait();
        }
    }
}
