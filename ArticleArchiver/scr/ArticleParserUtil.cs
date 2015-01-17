using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
namespace ArticleArchiver
{
    public class ArticleParserUtil
    {
        /// <summary>
        /// Count number of qualified paragarap tags under a given elememnt 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int Pcount(HtmlNode target)
        {
            //if (target == null || target.Descendants("p") == null) return 0;
            int n = 0;
            foreach (HtmlNode node in target.Descendants("p"))
            {
                string t = node.InnerText;
                string ta = "";

                foreach (HtmlNode aNode in node.Descendants("a"))
                {
                    ta = ta + aNode.InnerText;
                }

                if (((t.Length - ta.Length) > 3) && (t.Contains(".") || t.Contains(",") || t.Contains("?") || t.Contains("!")))
                {
                    n++;
                }
            }
            return n;
        }
       

        /// <summary>
        /// for attribute only
        /// Check if a class or id attribute exist for an element
        /// and if class or id value matched with a supplied list
        /// </summary>
        /// <param name="elem">the element to check</param>
        /// <param name="array">List of the values to match with</param>
        /// <returns>returns true if matched</returns>
        public static bool exist(HtmlAttribute elem, string[] array)
        {
            if (null != elem)
            {
                return exist(elem.Value.ToLower(), array);
            }
            return false;
        }
        /// <summary>
        /// for attribute only
        /// more selective then exist(attribute, array)
        /// it check for exact match rather contains for potential text nodes
        /// </summary>
        /// <param name="n"></param>
        /// <param name="junkIdentifier"></param>
        /// <param name="junkClass"></param>
        /// <param name="notJunkTag"></param>
        /// <returns></returns>
        public static bool exist(HtmlNode n, string junkIdentifier, string[] junkClass, string[] notJunkTag)
        {
            if (notJunkTag.Contains(n.Name) || notJunkTag.Contains(n.ParentNode.Name) || ArticleParserUtil.Pcount(n) > 0)
            {
                //if potential text node then be carefule before declaring junk
                return ((n.Attributes[junkIdentifier] != null) && (junkClass.Contains(n.Attributes[junkIdentifier].Value)));
            }
            else
            {
                return ((n.Attributes[junkIdentifier] != null) && (exist(n.Attributes[junkIdentifier], junkClass)));
            }
        }

        /// <summary>
        /// checks with the value only,can be use for attribute and node
        /// </summary>
        /// <param name="elemValue"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool exist(string elemValue, string[] array)
        {
            if (null != elemValue && null != array)
            {
                foreach (string a in array)
                {
                    if (elemValue.Contains(a) || a.Contains(elemValue))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// check if an node has paragraph element
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        bool hasParagraph(HtmlNode root)
        {
// ************************
// check back here
// may be i need to add #text with p!!            
            if ((root != null)
                && ((root.Name.Equals("p"))
                || (root.Descendants("p") != null && root.Descendants("p").Count() > 0)))
                return true;

            return false;
        }

        


        /// <summary>
        /// Return the first element found for specified tag and matched with identifier name, class, id, properity
        /// </summary>
        /// <param name="root"></param>
        /// <param name="elemTag"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        HtmlNode getElemByIdentifier(HtmlNode root, string elemTag, string[] identifiers)
        {
            string query = "";
            foreach (string str in identifiers)
            {
                query = "//" + elemTag + "[@id='" + str + "']|//" +
                    elemTag + "[@class='" + str + "']|//" + elemTag + "[@name='" + str + "']|//" + elemTag + "[@itemprop='" + str + "']";
                HtmlNode node = root.SelectSingleNode(query);
                if (null != node) return node;
            }
            return null;
        }

    }
}
