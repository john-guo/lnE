using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace lnE
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ComicDishAttribute : DishAttribute
    {
        public ComicDishAttribute(string pattern) : base(pattern)
        {
            Level = 2;
        }
    }

    public class PageIndex 
    {
        public PageIndex()
        {
            name = String.Empty;
            url = String.Empty;
            userData = null;
        }

        public string name;
        public string url;
        public object userData;
    }

    public abstract class BaseComicDish : WebDish
    {
        public abstract List<PageIndex> GetChapterIndex(HtmlAgilityPack.HtmlDocument html, string baseUrl);
        public abstract List<PageIndex> GetImageIndex(HtmlAgilityPack.HtmlDocument html, string baseUrl);
        protected virtual byte[] OnData(byte[] data, object userData)
        {
            return data;
        }
        public override void Eat(HtmlAgilityPack.HtmlDocument html, string url, string path, object userData)
        {

        }
    }

    public abstract class ComicDish : BaseComicDish
    {
        public override List<Index> GetIndex(HtmlAgilityPack.HtmlDocument html, string url, uint level, string path, object userData)
        {
            List<PageIndex> pages;

            if (level == 1)
            {
                SetReferer(url);
                pages = GetImageIndex(html, url);
            }
            else //level == 0
            {
                pages = GetChapterIndex(html, url);
            }

            return pages.ConvertAll(page => new Index(level) { name = page.name, url = GetUrl(url, page.url), userData = page.userData });
        }

        public override HtmlDocument Load(string url, uint level, string path, object userData)
        {
            if (level == 2)
            {
                var data = LoadData(url, level);
                data = OnData(data, userData);
                var di = Path.GetDirectoryName(path);
                if (!Directory.Exists(di))
                    Directory.CreateDirectory(di);
                File.WriteAllBytes(path, data);
                return null;
            }

            return base.Load(url, level, path, userData);
        }
    }

    public abstract class ComicDish2 : BaseComicDish
    {
        public abstract List<PageIndex> GetImagePageIndex(HtmlAgilityPack.HtmlDocument html, string baseUrl);
        public override List<Index> GetIndex(HtmlAgilityPack.HtmlDocument html, string url, uint level, string path, object userData)
        {
            List<PageIndex> pages;
            
            if (level == 2)
            {
                pages = GetImageIndex(html, url);
            }
            else if (level == 1)
            {
                SetReferer(url);
                pages = GetImagePageIndex(html, url);
            }
            else //level == 0
            {
                pages = GetChapterIndex(html, url);
            }

            return pages.ConvertAll(page => new Index(level) { name = page.name, url = GetUrl(url, page.url), userData = page.userData });
        }

        public override HtmlDocument Load(string url, uint level, string path, object userData)
        {
            if (level == 3)
            {
                var data = LoadData(url, level);
                data = OnData(data, userData);
                var di = Path.GetDirectoryName(path);
                if (!Directory.Exists(di))
                    Directory.CreateDirectory(di);
                File.WriteAllBytes(path, data);
                return null;
            }

            return base.Load(url, level, path, userData);
        }
    }
}
