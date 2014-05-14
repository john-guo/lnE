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
        }

        public string name;
        public string url;
    }

    public abstract class ComicDish : WebDish
    {
        public abstract List<PageIndex> GetChapterIndex(HtmlAgilityPack.HtmlDocument html);
        public abstract List<PageIndex> GetImageIndex(HtmlAgilityPack.HtmlDocument html);
        public override List<Index> GetIndex(HtmlAgilityPack.HtmlDocument html, string url, uint level, string path)
        {
            List<PageIndex> pages;

            if (level == 1)
            {
                SetReferer(url);
                pages = GetImageIndex(html);
            }
            else //level == 0
            {
                pages = GetChapterIndex(html);
            }

            return pages.ConvertAll(page => new Index(level) { name = page.name, url = GetUrl(url, page.url) });
        }

        public override HtmlDocument Load(string url, uint level, string path)
        {
            if (level == 2)
            {
                var data = LoadData(url);
                var di = Path.GetDirectoryName(path);
                if (!Directory.Exists(di))
                    Directory.CreateDirectory(di);
                File.WriteAllBytes(path, data);
                return null;
            }

            return base.Load(url, level, path);
        }

        public override void Eat(HtmlAgilityPack.HtmlDocument html, string url, string path)
        {
            
        }
    }
    public abstract class ComicDish2 : WebDish
    {
        public abstract List<PageIndex> GetChapterIndex(HtmlAgilityPack.HtmlDocument html);
        public abstract List<PageIndex> GetImagePageIndex(HtmlAgilityPack.HtmlDocument html);
        public abstract void EatImage(HtmlAgilityPack.HtmlDocument html, string fullFileName);
        public override List<Index> GetIndex(HtmlAgilityPack.HtmlDocument html, string url, uint level, string path)
        {
            List<PageIndex> pages;

            if (level == 1)
            {
                pages = GetImagePageIndex(html);
            }
            else //level == 0
            {
                pages = GetChapterIndex(html);
            }

            return pages.ConvertAll(page => new Index(level) { name = page.name, url = GetUrl(url, page.url) });
        }

        public override void Eat(HtmlAgilityPack.HtmlDocument html, string url, string path)
        {
            var di = Path.GetDirectoryName(path);
            if (!Directory.Exists(di))
                Directory.CreateDirectory(di);

            EatImage(html, path);
        }
    }
}
