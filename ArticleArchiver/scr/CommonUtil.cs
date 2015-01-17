using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text.RegularExpressions;
namespace ArticleArchiver
{
    public class CommonUtil
    {
        /// <summary>
        /// Serialize an object to json String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }
        /// <summary>
        /// JSON Deserialization
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }

        /// <summary>
        /// Remove space, tab and line breaks for a string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string removeWhiteSpace(string str)
        {
            if (null != str && str.Length > 0)
            {
                Regex regex = new Regex(@"[ ]{2,}", RegexOptions.None);
                str = str.Replace("\r\n", " ");
                str = str.Replace("\n", " ");
                str = str.Replace("\t", " ");
                str = regex.Replace(str, @" ").Trim();
            }
            return str;
        }

        /// <summary>
        /// count depth on a xpath string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int countDepth(string str)
        {

            if (null != str && str.Length > 0)
            {
                return str.Split('/').Length - 1;
            }
            return 0;
        }

        public static bool empty(string str)
        {
            if (str != null && str.Length > 0) return false;
            return true;

        }


        public static string RemoveTroublesomeCharacters(string inString)
        {
            //return Regex.Replace(inString, @"[\u0000-\u001F]", string.Empty);
            return Regex.Replace(inString, @"[^\u0000-\u007F]", string.Empty);
           // inString= Regex.Replace(inString, @"[\u0000-\u0008]", string.Empty);
           // inString = Regex.Replace(inString, @"[\u00FF-\uFFFF]", string.Empty);
           // inString = Regex.Replace(inString, @"[\xFF-\xFFFF]", string.Empty);
        //    inString = Regex.Replace(inString, @"[•]", string.Empty);
            
           // return inString;
        }
    }
}
