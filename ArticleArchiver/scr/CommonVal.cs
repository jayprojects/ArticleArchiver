using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleArchiver
{
    public class CommonVal
    {
        public static string[] MetaDescriptionContainers = new string[] 
        { "Description", "DC.description" };
        public static string[] MetaImageUrlContainers = new string[] 
        { "og:image", "twitter:image:src" };
        public static string[] MetaTitleContainers =
            new string[] { "og:Title", "dc.Title" };
        public static string[] MetaAuthorContainers =
        new string[] { "DC.author", "DC.creator", "Author", "Creator" };
        public static string[] MetaPublishDateContainers =
        new string[] { "og:date", "DC.date", "DC.date.issued","Date" };

        public static string[] holderNames = new string[] { "id", "class", "itemprop", "name" };
        public static string[] titleHolders = new string[] { "title", "headline", "heading", "header", "article_main" };
        public static string[] authorHolders = new string[] { "byline", "by_line", "author", "writter", "creator", "a-byline" };
        public static string[] pubDateHolders = new string[] { "date", "published", "timestamp", "tmstmp", "datepublished" };

        public static string[] importentNodes = new string[] { "p", "a", "img", "#text"};
            
    }
}
