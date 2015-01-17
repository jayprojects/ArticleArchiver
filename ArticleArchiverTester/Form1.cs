using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArticleArchiver;
using HtmlAgilityPack;
namespace ArticleArchiverTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = textBox1.Text;
            ArticleCompact ac = Archiver.getArticle(str);
            richTextBox1.Text =CommonUtil.JsonSerializer(ac);
            webBrowser1.DocumentText = ac.ArticleHtml;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //richTextBox1.Text = typeof(HtmlAgilityPack.HtmlDocument).Assembly.FullName;

            int i = (int)'a';
            i = (int)' ';
            i = (int)'€';
            i = (int)'ƒ';
            i = (int)'„';
            i = (int)'…';
            i = (int)'•';

        }
    }
}
