

namespace ArticleArchiver
{
    public class Article
    {
        public string TitleArticle { get; set; }
        public string TitleMeta { get; set; }
        public string TitleTag { get; set; }


        public string ImageMetaUrl { get; set; }
        public string ImageMetaData { get; set; }

        public string PublishDate { get; set; }
        public string PublishDateMeta { get; set; }

        public string AuthorArticle { get; set; }
        public string AuthorMeta { get; set; }
        
        public string AarticleUrl { get; set; }
        public string AccessDate { get; set; }
        public string SourceSite { get; set; }
        public string DescriptionMeta { get; set; }
        
        public string ArticleText { get; set; }
        public string ArticleHtml { get; set; }
        public string FullHtml { get; set; }
        
    }
}
