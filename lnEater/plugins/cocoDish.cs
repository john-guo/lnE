using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;

namespace lnE
{
    [ComicDish("http://www.cococomic.com/comic/")]
    public class cocoDish : ComicDish
    {
        private readonly string[] ServerList = new string[15];
        private int serverIndex = 0;

        public cocoDish() 
        {
            ServerList[0]="http://58.215.241.60:9393/dm01/";
            ServerList[1]="http://58.215.240.35:9393/dm02/";
            ServerList[2]="http://61.147.113.115:9115/dm03/";
            ServerList[3]="http://61.147.113.115:9115/dm04/";
            ServerList[4]="http://58.215.241.60:9393/dm05/";
            ServerList[5]="http://61.147.113.115:9115/dm06/";
            ServerList[6]="http://58.215.240.35:9393/dm07/";
            ServerList[7]="http://58.215.240.35:9393/dm08/";
            ServerList[8]="http://61.147.113.115:9115/dm09/";
            ServerList[9]="http://58.215.241.60:9393/dm10/";
            ServerList[10]="http://61.147.113.115:9115/dm11/";
            ServerList[11]="http://58.215.241.60:9393/dm12/";
            ServerList[12]="http://58.215.241.60:9393/dm13/";
            ServerList[14]="http://61.147.113.115:9115/dm15/";
        }

        public override List<PageIndex> GetChapterIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();

            var nodes = html.DocumentNode.SelectNodes(".//*[@id='main']/div[2]/div[2]/div[1]/div[2]/div[3]/ul/li");
            foreach (var node in nodes)
            {
                var link = node.SelectNodes("./a").First();
                var url = link.GetAttributeValue("href", String.Empty);
                var name = link.InnerText;

                if (serverIndex == 0)
                {
                    var ret = AssemblyHelper.ParseString("\\?s=(\\d+)", url);
                    if (ret != null)
                    {
                        serverIndex = Convert.ToInt32(ret);
                    }
                }

                pages.Add(new PageIndex() { name = name, url = url });
            }

            return pages;
        }

        const string unsuan = "function unsuan(s,a){k=a.substring(0,a.length-1);f=a.substring(a.length-1);for(i=0;i<k.length;i++){eval(\"s=s.replace(/\"+k.substring(i,i+1)+\"/g,'\"+i+\"')\")}ss=s.split(f);s=\"\";for(i=0;i<ss.length;i++){s+=String.fromCharCode(ss[i])}return s}";


        public override List<PageIndex> GetImageIndex(HtmlDocument html, string baseUrl)
        {
            var pages = new List<PageIndex>();

            var script = html.DocumentNode.SelectNodes("//head/script[boolean(@src)=false]").First().InnerText;
            var js = "PicListUrl=unsuan(PicListUrl,\"zsanuxoewrk\");var arrPicListUrl = PicListUrl.split('|');var a=[];for(var i in arrPicListUrl)a.push(arrPicListUrl[i]);a";
            var result = AssemblyHelper.EvalJs2(script, unsuan + js);

            int i = 1;
            foreach (var a in result)
            {
                var u = String.Format("{0}{1}", ServerList[serverIndex == 0 ? serverIndex : serverIndex - 1],  a);
                var n = GetIndexedName(i++, u);
                pages.Add(new PageIndex() { name = n, url = u });
            }

            return pages;
        }
    }
}
