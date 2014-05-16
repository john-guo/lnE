using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using System.Dynamic;
using SwfDotNet.IO;
using SwfDotNet.IO.Tags;

namespace lnE
{
    [ComicDish("http://ac.qq.com/", Ext = ".jpg")]
    public class qqDish : ComicDish
    {
        public override List<PageIndex> GetChapterIndex(HtmlDocument html)
        {
            var pages = new List<PageIndex>();

            if (html.DocumentNode.SelectSingleNode("/html[@id='jp']") == null)
            { 
                var nodes = html.DocumentNode.SelectNodes("//body/div[@id='chapter']/div[@class='works-chapter-list-wr']/ol[@class='chapter-page-all works-chapter-list']/li");
                foreach (var node in nodes)
                {
                    var pnodes = node.SelectNodes(".//span");
                    foreach (var pnode in pnodes)
                    {
                        var link = pnode.SelectSingleNode("./a");
                        var url = link.GetAttributeValue("href", String.Empty);
                        var name = link.GetAttributeValue("title", String.Empty);

                        pages.Add(new PageIndex() { name = name, url = url });
                    }
                }
            }
            else
            {
                var main = html.DocumentNode.SelectSingleNode("//body//div[@class='jp-main-chapter-wr ui-hide']");
                var nodes = main.SelectNodes(".//div[@class='jp-main-jump-list-wr ui-hide']/p");
                foreach (var node in nodes)
                {
                    var pnodes = node.SelectNodes("./span");
                    foreach (var pnode in pnodes)
                    {
                        var link = pnode.SelectSingleNode("./a");
                        var url = link.GetAttributeValue("href", String.Empty);
                        var name = link.InnerText;

                        pages.Add(new PageIndex() { name = name, url = url });
                    }
                }

                var volumes = main.SelectNodes("./ul[@class='jp-volume-list-wr ui-hide']");
                foreach (var volume in volumes)
                {
                    var chapters = volume.SelectNodes("./li[@class='listItem']");
                    foreach(var chapter in chapters)
                    {
                        var pnodes = volume.SelectNodes(".//ul[@class='jp-chapter-list-wr ui-hide']/li");
                        foreach (var pnode in pnodes)
                        {
                            var link = pnode.SelectSingleNode("./a");
                            var url = link.GetAttributeValue("href", String.Empty);
                            var name = link.InnerText;

                            pages.Add(new PageIndex() { name = name, url = url });
                        }
                    }
                }
            }

            return pages;
        }

        public override List<PageIndex> GetImageIndex(HtmlDocument html)
        {
            var pages = new List<PageIndex>();

            var script = html.DocumentNode.SelectNodes("//body/script[boolean(@src)=false]").Last().InnerText;
            string data = String.Empty;
            using (var reader = new StringReader(script))
            {
                while (reader.Peek() > 0)
                {
                    var line = reader.ReadLine();
                    if (line.TrimStart().StartsWith("var data"))
                    {
                        data = AssemblyHelper.EvalJs<string>(line, "data");
                        break;
                    }
                }
            }

            var bytes = Convert.FromBase64String(data);
            var js = Encoding.UTF8.GetString(bytes);
            var info = AssemblyHelper.EvalJs("var a=" + js, "a");

            var i = 1;
            foreach (var a in info["picture"])
            {
                dynamic o = new ExpandoObject();
                o.id = info["comic"]["id"];
                o.cid = info["comic"]["cid"];
                o.pid = info["picture"][a]["pid"];

                var u = info["picture"][a]["url"];
                var n = GetIndexedName(i++, u);
                pages.Add(new PageIndex() { name = n, url = u, userData = o });
            }

            return pages;
        }

        protected override byte[] OnData(byte[] data, object userData)
        {
            dynamic o = userData;
            decrypt(data, o.id, o.cid, o.pid);
            data[3] = 7;

            using (var stream = new MemoryStream(data))
            {
                var swfreader = new SwfReader(stream);
                var swf = swfreader.ReadSwf();
                var j = swf.Tags.OfType<DefineBitsJpeg2Tag>().First();
                return j.JpegData;
            }
        }

        readonly string[] keyList = new string[] { "0TYxZWNhOWVhNGViYmViMDM5YjczNzkwNTFYzczZjc45GFDg%#$1234%#$@5", "ZGI1ODliZTJmMDQzYjRiMGM1MjdkODllOWI5ZWVmYzkDFK(*lIYUtU%^YHERT%", "NmUwYzZiMDU5MWU4ZGM4OTliOTE1MTU5Yzc3Nzg0NzYHFG$^*46&$^#@#$@$3", "gdfSDFSW2347634%FS2y678gfHBhhfJityDswertyy56h7hbyrtvggftj856g23G#", "dfGSDF23f1273bg6@fgwvtrhBHU*TRRTYf12y5G^%$^%G@1fD!@#GF^DR3tyrtg", "SDFG4f5rdtfwE%#G&^#34GF^$%&^@FTHY$%^gdfGHERBTHJUWCwqTV@$0%B#H", "DFWEVf345Y#d512Y$%&768%^&G@$%^@#GYU#$^jTYHTRU#^YWETWEF99fhTYU", "<>UThwvWEFSTjuJY&(&*HR!@#!CFDVAcdfsdtaectryrt75463452t3FWCERYfd" };

        void decrypt(byte[] data, int id, int cid, int pid)
        {
            var _local5 = data.Length;
            var _local6 = 0;
            var _local7 = keyList.Length;
            List<int> _local8 = new List<int>();
            var _local9 = "";
            int _local10;
            _local8.Add((((id + cid) + pid) % _local7));
            _local8.Add((((id + 1) + ((cid + 1) * (pid + 1))) % _local7));
            _local8.Add(((((id + 1) * (cid + 1)) + (pid + 1)) % _local7));
            _local8.Add((id % _local7));
            _local8.Add((cid % _local7));
            _local8.Add((pid % _local7));
            foreach (var _local13 in _local8)
            {
                _local9 = (_local9 + keyList[_local13]);
            }
            _local10 = _local9.Length;
            while (_local6 < _local5)
            {
                var _local11 = _local9[(_local6 % _local10)];
                var _local12 = (byte)(data[_local6] ^ (byte)_local11);
                var _temp1 = _local6;
                _local6 = (_local6 + 1);
                var _local14 = _temp1;
                data[_local14] = _local12;
            }
        }

    }
}
