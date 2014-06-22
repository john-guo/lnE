using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using wnd = System.Windows.Forms;

namespace lnE
{
    public class Index
    {
        public Index()
        {
            name = String.Empty;
            url = String.Empty;
            level = 0;
            userData = null;
        }

        public Index(uint curLevel)
        {
            name = String.Empty;
            url = String.Empty;
            level = curLevel + 1;
            userData = null;
        }

        public string name;
        public string url;
        public uint level;
        public object userData;
    }

    public interface IDish
    {
        HtmlDocument Load(string url, uint level, string path, object userData, int tryCount);
        List<Index> GetIndex(HtmlDocument html, string url, uint level, string path, object userData);
        void Eat(HtmlDocument html, string url, string path, object userData);
        Index GetLink(object rawData);
    }

    public abstract class Dish : IDish
    {
        protected DishAttribute dishSettings;
        public abstract HtmlDocument Load(string url, uint level, string path, object userData, int tryCount);
        public abstract List<Index> GetIndex(HtmlDocument html, string url, uint level, string path, object userData);
        public abstract void Eat(HtmlDocument html, string url, string path, object userData);
        public abstract Index GetLink(object rawData);

        public void SetSettings(DishAttribute settings)
        {
            dishSettings = settings;
        }

        protected string XTrim(string str)
        {
            var s = str.Trim();
            s = s.Replace("\r", String.Empty);
            s = s.Replace("\n", String.Empty);
            s = s.Replace("\t", String.Empty);
            s = s.Replace(" ", "_");
            foreach (var c in Path.GetInvalidFileNameChars())
                s = s.Replace(c, '_');
            foreach (var c in Path.GetInvalidPathChars())
                s = s.Replace(c, '_');
            s = s.Replace('.', '_');
            return s.Replace("&nbsp;", "_");
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DishAttribute : Attribute
    {
        public DishAttribute(string pattern)
        {
            DataFormat = wnd.DataFormats.Html;
            Pattern = pattern;
            Level = 1;
            Ext = String.Empty;
            Comment = String.Empty;
        }

        public string DataFormat
        {
            get;
            set;
        }

        public string Pattern
        {
            get;
            set;
        }

        public uint Level
        {
            get;
            set;
        }

        public string Ext
        {
            get;
            set;
        }

        public string Comment
        {
            get;
            set;
        }
    }
}
