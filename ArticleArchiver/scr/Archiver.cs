using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleArchiver
{
    public class Archiver
    {
        /// <summary>
        /// Scrap the article and convert compact object
        /// e.g. title can be scrapped from title meta tag, html title tag, and the article body
        /// but we need the title once to save in the database
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static ArticleCompact getArticle(string url)
        {
            Article a = new Article();

            //string url = textBox1.Text;
            a.AarticleUrl = url;
            a.AccessDate = DateTime.Now.ToString();
            //HtmlWeb web = new HtmlWeb();
            //HtmlAgilityPack.HtmlDocument doc = web.Load(url);
            string htmlSource = NetUtil.getContentWC(url);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlSource);
            //richTextBox1.AppendText("elem count : " + doc.DocumentNode.Descendants().Count() + "\n");
            //removeJunkTags(ref doc);
            //richTextBox1.AppendText("elem count : " + doc.DocumentNode.Descendants().Count() + "\n");



            a.TitleTag = doc.DocumentNode.SelectSingleNode("//title").InnerText;

            
            ArticleParser.getMetaInfo(doc, ref a);
            ArticleParserRemover.removeJunkTags(ref doc);
            ArticleParser.getHeader(doc, ref a);
            ArticleParserRemover.RemoveEmptyNodes(doc.DocumentNode);
            ArticleParserRemover.removeJunkAttr(ref doc);
            //richTextBox1.AppendText(doc.DocumentNode.InnerHtml);
            HtmlAgilityPack.HtmlNode bodyNode = ArticleParser.getBody(doc.DocumentNode, 0.8);
            a.ArticleHtml =CommonUtil.RemoveTroublesomeCharacters(bodyNode.InnerHtml);
            a.ArticleText = CommonUtil.RemoveTroublesomeCharacters(bodyNode.InnerText);
            //webBrowser1.DocumentText = bodyNode.InnerHtml;



            ArticleCompact ac = new ArticleCompact();
            if (!CommonUtil.empty(a.TitleMeta))
                ac.Title = a.TitleMeta;
            else if (!CommonUtil.empty(a.TitleArticle))
                ac.Title = a.TitleArticle;
            else if (!CommonUtil.empty(a.TitleTag))
                ac.Title = a.TitleTag;
            else
                ac.Title = "Title not found";

            if (!CommonUtil.empty(a.AuthorMeta))
                ac.Author = a.AuthorMeta;
            else if (!CommonUtil.empty(a.AuthorArticle))
                ac.Author = a.AuthorArticle;
            else
                ac.Author = "Unknown";

            if (!CommonUtil.empty(a.PublishDateMeta))
                ac.PublishDate = a.PublishDateMeta;
            else if (!CommonUtil.empty(a.PublishDate))
                ac.PublishDate = a.PublishDate;
            else
                ac.PublishDate = "";

            if (!CommonUtil.empty(a.ImageMetaUrl))
                ac.ImageUrl = a.ImageMetaUrl;
            else
                ac.ImageUrl = "";


            if (!CommonUtil.empty(a.DescriptionMeta))
                ac.Description = a.DescriptionMeta;
            else
                ac.Description = "";


            if (!CommonUtil.empty(a.ArticleHtml))

                ac.ArticleHtml = CommonUtil.removeWhiteSpace(a.ArticleHtml);
            else
                ac.ArticleHtml = "";

            if (!CommonUtil.empty(a.ArticleText))
                ac.ArticleText = CommonUtil.removeWhiteSpace(a.ArticleText);
            else
                ac.ArticleText = "";
            return ac;
        }
    }
}
