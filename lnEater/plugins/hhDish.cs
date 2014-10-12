using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;

namespace lnE
{
    [ComicDish("http://hhcomic.com/Comic/")]
    public class hhDish : ComicDish
    {
        const string server = "http://61.160.196.48:8081/dm03/";

        /*
         * ServerList[0]="http://33.3348.net:9393/dm01/";
ServerList[1]="http://33.3348.net:9393/dm02/";
ServerList[2]="http://61.160.196.48:8081/dm03/";
ServerList[3]="http://61.160.196.48:8081/dm04/";
ServerList[4]="http://33.3348.net:9393/dm05/";
ServerList[5]="http://33.3348.net:9393/dm06/";
ServerList[6]="http://33.3348.net:9393/dm07/";
ServerList[7]="http://61.160.196.48:8081/dm08/";
ServerList[8]="http://61.160.196.48:8081/dm09/";
ServerList[9]="http://33.3348.net:9393/dm10/";
ServerList[10]="http://33.3348.net:9393/dm11/";
ServerList[11]="http://61.160.196.48:8081/dm12/";
ServerList[12]="http://33.3348.net:9393/dm13/";
ServerList[13]="http://8.8.8.8:99/dm14/";
ServerList[14]="http://33.3348.net:9393/dm15/";
ServerList[15]="http://33.3348.net:9393/dm16/";
         * */

        public override List<PageIndex> GetChapterIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();

            var nodes = html.DocumentNode.SelectNodes("//div[@id='mainContent']/div[@id='content']/div[@class='vol']/ul[@class='bl']/li");
            foreach (var node in nodes)
            {
                var link = node.SelectSingleNode("./a");
                var url = link.GetAttributeValue("href", String.Empty);
                var name = link.InnerText;

                pages.Add(new PageIndex() { name = name, url = url });
            }

            return pages;
        }

        public override List<PageIndex> GetImageIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();

            var script = html.DocumentNode.SelectNodes("//head/script[boolean(@src)=false]").First().InnerText;

            var js = @"function unsuan(s,sk) 
{
    var k=sk.substring(0,sk.length-1);
    var f=sk.substring(sk.length-1);
	for(i=0;i<k.length;i++) {
	    eval(""s=s.replace(/""+ k.substring(i,i+1) +""/g,'""+ i +""')"");
	}
    var ss = s.split(f);
	s="""";
	for(i=0;i<ss.length;i++) {
	    s+=String.fromCharCode(ss[i]);
    }
    return s;
}
PicLlstUrl=unsuan(PicLlstUrl,""tavzscoewrm"");
var arrPicLlstUrl = PicLlstUrl.split('|');
arrPicLlstUrl";
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
