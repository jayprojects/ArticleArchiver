using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleArchiver
{
    public class ArticleCompact
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string PublishDate { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ArticleHtml { get; set; }
        public string ArticleText { get; set; }
    }
}
