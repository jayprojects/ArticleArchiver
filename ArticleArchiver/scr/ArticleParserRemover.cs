using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
namespace ArticleArchiver
{
    public class ArticleParserRemover
    {
        /// <summary>
        /// Remove any node that is empty 
        /// also remove a node that doesn't have a text node as decended
        /// </summary>
        /// <param name="node"></param>
        public static void RemoveEmptyNodes(HtmlNode node)
        {
            /*
            foreach (HtmlAttribute nodeAttr in node.Attributes)
            {
                string nodeText = CommonUtil.removeWhiteSpace(node.InnerText);
                string attrName = nodeAttr.Name.ToLower();
                string attrVal = nodeAttr.Value;
                if(attrVal!=null && attrVal.Contains("viewport"))
                {
                    string found = "yea";
                }
            }
             * */
            //int pc = ArticleParserUtil.Pcount(node);
            bool isImportant = false;
            bool hasImportant = false;
            foreach(string iNode in CommonVal.importentNodes)
            {
                if (node.Name.Equals(iNode)) isImportant = true;
                if ((null != node.Descendants(iNode)) && (node.Descendants(iNode).Count() > 0)) hasImportant = true;
            }

            if ((!isImportant && !hasImportant)
                || ((!node.Name.Equals("img")) &&
                    (node.ChildNodes == null || node.ChildNodes.Count < 1) && 
                    (node.InnerText == null || node.InnerText == string.Empty))
                )
            {
                node.Remove();
            }
            else
            {
                for (int i = node.ChildNodes.Count - 1; i >= 0; i--)
                {
                    RemoveEmptyNodes(node.ChildNodes[i]);
                }
            }
        }


        /// <summary>
        /// Remove html tages that is unnecessary for article content 
        /// </summary>
        /// <param name="doc"></param>
        public static void removeJunkTags(ref HtmlAgilityPack.HtmlDocument doc)
        {
            /*
            HtmlNodeCollection nc = doc.DocumentNode.SelectNodes("//script|//link|//iframe|//frameset|//frame|//applet|//object");
            if (nc != null)
            {
                foreach (HtmlNode node in nc)
                {
                    node.ParentNode.RemoveChild(node, false);

                }
            } 
            */
            string[] junkTags = new string[] { "script", "style", ".hidden", "head", "meta", "link", "iframe", "video", "object", "embed", "param", "#comment", "footer", "nav" };
            string[] junkClass = new string[] { "comments", "ad-content", "ad-container", "ad_content", "ad_container", "hidden", "header-nav", "share", "social", "aside", "related" };
            string[] notJunkTags = new string[] { "p", "img", "#text" };
            doc.DocumentNode.Descendants()
                .Where(n => (junkTags.Contains(n.Name)
                    //|| ((n.Attributes["class"]!=null) && (junkClass.Contains(n.Attributes["class"].Value)))
                    //|| ((n.Attributes["id"] != null) && ( junkClass.Contains(n.Attributes["id"].Value)))
                    //|| ((n.Attributes["class"] != null) && (ParserUtil.exist(n.Attributes["class"], junkClass)))
                    // || ((n.Attributes["id"] != null) && ( ParserUtil.exist(n.Attributes["id"],junkClass)))
                            || ArticleParserUtil.exist(n, "id", junkClass, notJunkTags)
                            || ArticleParserUtil.exist(n, "class", junkClass, notJunkTags)
                            || ArticleParserUtil.exist(n, "itemprop", junkClass, notJunkTags)

                            )
                       ).ToList()
                       .ForEach(n => n.Remove());

        }

        /// <summary>
        /// Remove all junk attributes. this does not remove the elemnt itself
        /// removes only the attributes such as onclick or class
        /// </summary>
        /// <param name="doc"></param>
        public static void removeJunkAttr(ref HtmlAgilityPack.HtmlDocument doc)
        {
            foreach (HtmlNode node in doc.DocumentNode.Descendants())
            {
                string[] junkAttributes = new string[] { "style", "class", "itemprop", "name", "id" };
            doAgain:
                foreach (HtmlAttribute ha in node.Attributes)
                {
                    //removes atributes starts with "on"
                    if (ha.Name.ToLower().StartsWith("on"))
                    {
                        node.Attributes[ha.Name].Remove();
                        goto doAgain;
                    }
                    else
                    {
                        //removes everything on the list
                        foreach (string ja in junkAttributes)
                        {
                            if (ha.Name.Contains(ja))
                            {
                                node.Attributes[ha.Name].Remove();
                                goto doAgain;
                            }
                        }
                    }
                }

            }
        }

    }
}
