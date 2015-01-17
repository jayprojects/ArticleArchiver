using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
namespace ArticleArchiver
{
    public class ArticleParser
    {
        /// <summary>
        /// This is a helper function for getMetaInfo function
        /// extract the value of the givey meta property 
        /// </summary>
        /// <param name="doc">the Document to lookup</param>
        /// <param name="keys">List of value the meta would be idenified with</param>
        /// <returns></returns>
        public static string getMetaValue(HtmlAgilityPack.HtmlDocument doc, string[] keys)
        {
            HtmlNode node = null;
            foreach (string key in keys)
            {
                node = doc.DocumentNode.SelectSingleNode("//meta[@name='" + key + "']");
                if (node != null) break;
                node = doc.DocumentNode.SelectSingleNode("//meta[@property='" + key + "']");
                if (node != null) break;
                node = doc.DocumentNode.SelectSingleNode("//meta[@name='" + key.ToLower() + "']");
                if (node != null) break;
                node = doc.DocumentNode.SelectSingleNode("//meta[@property='" + key.ToLower() + "']");
                if (node != null) break;
            }
            if (node != null) return node.GetAttributeValue("content", "").Trim();
            return "";
        }

        /// <summary>
        /// Lookup parameters in the meta tags for the various properties of the article 
        /// set Artilce object value by reference, description, title, author, date, imageurl
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="a"></param>
        public static void getMetaInfo(HtmlAgilityPack.HtmlDocument doc, ref Article a)
        {
            //find out what is og vs dc
            a.DescriptionMeta = getMetaValue(doc, CommonVal.MetaDescriptionContainers);
            a.ImageMetaUrl = getMetaValue(doc, CommonVal.MetaImageUrlContainers);
            a.TitleMeta = getMetaValue(doc, CommonVal.MetaTitleContainers);
            a.AuthorMeta = getMetaValue(doc, CommonVal.MetaAuthorContainers);
            a.PublishDateMeta = getMetaValue(doc, CommonVal.MetaPublishDateContainers);
        }




        /// <summary>
        /// Digg Deep for smallest posible author value
        /// </summary>
        /// <param name="root"></param>
        /// <param name="authorHolders"></param>
        /// <returns></returns>
        public static string getAuthorValue(HtmlNode root, string[] authorHolders)
        {
            if (root != null && root.Descendants() != null && root.Descendants().Count() > 0)
            {
                foreach (HtmlNode node in root.Descendants())
                {
                    string nodeText = CommonUtil.removeWhiteSpace(node.InnerText);
                    foreach (HtmlAttribute nodeAttr in node.Attributes)
                    {

                        string attrName = nodeAttr.Name.ToLower();
                        string attrVal = nodeAttr.Value.ToLower();

                        if (ArticleParserUtil.exist(attrVal, authorHolders) && nodeText.Length > 5 && nodeText.Length < 70)
                        {
                            return getAuthorValue(node, authorHolders);
                        }
                    }
                }
            }

            return root.InnerText.Replace("by", "").Replace("By", "");
        }

        public static string getPubDateValue(HtmlNode root, string[] pubDateHolders)
        {
            if (root != null && root.Descendants() != null && root.Descendants().Count() > 0)
            {
                foreach (HtmlNode node in root.Descendants())
                {
                    string nodeText = CommonUtil.removeWhiteSpace(node.InnerText);
                    foreach (HtmlAttribute nodeAttr in node.Attributes)
                    {

                        string attrName = nodeAttr.Name.ToLower();
                        string attrVal = nodeAttr.Value.ToLower();
                        //if (ParserUtil.exist(attrVal, authorHolders) && node.InnerText.Length > 5 && node.InnerText.Length < 70)
                        if ((Regex.Match(nodeText, "^(.*)(19|20)\\d{2}(.*)$").Success)
                            && (ArticleParserUtil.exist(attrVal, pubDateHolders) || (node.Name.ToLower().Equals("time") || node.Name.ToLower().Equals("date"))))
                        {
                            return getPubDateValue(node, pubDateHolders);
                        }
                    }
                }
            }

            return root.InnerText.Replace("published", "").Replace("date", "");
        }

        public static void getHeader(HtmlAgilityPack.HtmlDocument doc, ref Article a)
        {


            string titleText = "";
            string authorText = "";
            string pubDateText = "";

            string authorText0 = "";
            HtmlNode titleNode = null;

            foreach (HtmlNode node in doc.DocumentNode.Descendants())
            {
                foreach (HtmlAttribute nodeAttr in node.Attributes)
                {
                    string nodeText = CommonUtil.removeWhiteSpace(node.InnerText);

                    string attrName = nodeAttr.Name.ToLower();
                    string attrVal = nodeAttr.Value.ToLower();
                    if (CommonVal.holderNames.Contains(attrName))
                    {
                        //Parse title
                        if (titleText.Length < 1 && ArticleParserUtil.exist(attrVal, CommonVal.titleHolders))
                        {
                            foreach (HtmlNode innerNode in node.Descendants())
                            {
                                string t = innerNode.InnerText.Trim();
                                if (innerNode.Name.ToLower().Equals("h1"))
                                {
                                    //implement a matching checkup with title tag!
                                    titleText = innerNode.InnerText.Trim();
                                    titleNode = innerNode;
                                }

                                //just a backup search for author
                                if (t.Length > 5 && t.Length < 30 && (t.ToLower().Contains(" by ") || t.ToLower().Contains(" by: ") || t.ToLower().StartsWith("by: ") || t.ToLower().StartsWith("by ")))
                                {
                                    authorText0 = t;
                                }
                            }
                        }
                      
                        //parse author
                        if (authorText.Length < 1 && ArticleParserUtil.exist(attrVal, CommonVal.authorHolders))
                            //&& node.InnerText.Length > 5 && node.InnerText.Length < 70)
                        {
                            authorText = getAuthorValue(node, CommonVal.authorHolders);
                        }

                        //parse publishdate
                        if (pubDateText.Length < 1 && (Regex.Match(nodeText, "^(.*)(19|20)\\d{2}(.*)$").Success)
                            && (ArticleParserUtil.exist(attrVal, CommonVal.pubDateHolders) || (node.Name.ToLower().Equals("time") || node.Name.ToLower().Equals("date"))))
                        {

                            pubDateText = getPubDateValue(node, CommonVal.pubDateHolders);
                        }
                       
                    }

                }
            }

            if (pubDateText.Length < 1 && titleNode!=null)
            {
                foreach (HtmlNode node in doc.DocumentNode.Descendants("#text"))
                {
                    
                    if(node.Line>titleNode.Line-10 && node.Line<titleNode.Line+100 )
                    {
                        string nodeText = CommonUtil.removeWhiteSpace(node.InnerText).ToLower();

                        if ((Regex.Match(nodeText, "^(.*)(19|20)\\d{2}(.*)$").Success)
                            && (nodeText.Length > 5 && nodeText.Length<100)
                           && (nodeText.Contains("est") ||nodeText.Contains("gmt") || nodeText.Contains("date") || nodeText.Contains("time") || nodeText.Contains("am") ||nodeText.Contains("pm")))
                        {
                            pubDateText = CommonUtil.removeWhiteSpace(node.InnerText);
                            break;
                        }
                    }
                }
            }

            if (authorText.Length < 2 && authorText0.Length > 2) authorText = authorText0;
            a.TitleArticle = CommonUtil.removeWhiteSpace(titleText);
            a.PublishDate = CommonUtil.removeWhiteSpace(pubDateText.Replace("Published", "").Replace("published", "").Replace("Publish", "").Replace("publish", "").Replace("Date", "").Replace("date", "").Replace(":", ""));
            a.AuthorArticle = CommonUtil.removeWhiteSpace(authorText.Replace("By ", "").Replace("by ", "").Replace("Written", "").Replace(":", ""));

        }



        /// <summary>
        /// Parse the bodyHtml of the article
        /// </summary>
        /// <param name="root"></param>
        /// <param name="majorityPercent"></param>
        /// <returns></returns>
        public static HtmlNode getBody(HtmlNode root, double majorityPercent)
        {
            //string fullText = doc.DocumentNode.InnerHtml;
            int majorityP = (int)(ArticleParserUtil.Pcount(root) * majorityPercent);
            /*
            //Some articles folllows html5 tagging style and has and article tag
            //for those the job is simple simply get the content of article tag
            HtmlNode articleNode = root.SelectSingleNode("//article");
            if(null!=articleNode)
            {
                //bodyHtml = articleNode.InnerHtml;
                if(Pcount(articleNode)>majorityP)
                    return articleNode;
            }
            
            

            
            //few other articles use very common identifier to define the main body tag
            //if such a tag found its very good chance that that is the article did
            HtmlNode articleBodyNode =getElemByIdentifier(root, "div", new string[] {"articleBody", "article-Body"});//.SelectSingleNode("//div[@itemprop='articleBody']|");
            if(null!=articleBodyNode)
            {
                //bodyHtml = articleBodyNode.InnerHtml;
                if (Pcount(articleNode) > majorityP)
                    return articleBodyNode;
            }
           
            */

            //for rest of the majory articles though, we have to find the article body manually :(
            //lets do it!!
            HtmlNode target = root;
            HtmlNodeCollection divs = root.SelectNodes("//div");
            if (divs != null && divs.Count > 0)
            {

                foreach (HtmlNode node in divs)
                {

                    int pNo = ArticleParserUtil.Pcount(node);
                    if ((pNo > majorityP) && (CommonUtil.countDepth(node.XPath) >= CommonUtil.countDepth(target.XPath)))
                    {
                        target = node;
                    }
                }
                if (null != target)
                {
                    root = target;

                }
            }
            int newMajorityP = (int)(ArticleParserUtil.Pcount(root) * majorityPercent);
            if ((majorityPercent < 0.4) || ((majorityP - newMajorityP) > (majorityP * 0.30)))
            {
                return root;
            }
            else
            {
                return getBody(root, (majorityPercent * 0.8));
            }

        }
    }
}
