using HtmlAgilityPack;
using System;

namespace FindJob.core
{
    public class GetMaxPageClass
    {
           
        public int GetMaxValueFromPagePracuj(int key,int lvl, string location)
        {
            string html;
            int maxpage;
            CheckInput(key, lvl, location, out html, out maxpage);
            if (html != string.Empty)
            {
                HtmlWeb web = new HtmlWeb();
                var htmlDoc = web.Load(html);
                var nodes = htmlDoc.DocumentNode.SelectSingleNode("//*[@data-test=\"top-pagination-max-page-number\"]");

                if (nodes == null)
                {
                    maxpage = 1;
                }
                else
                {
                    maxpage = Convert.ToInt32(nodes.InnerText);
                }

                return maxpage;
            }

            return 0;
        }

        private static void CheckInput(int key, int lvl, string location, out string html, out int maxpage)
        {
            html = "";
            maxpage = 0;
            if (lvl > 0 && location == string.Empty)
            {
                html = $@"https://it.pracuj.pl/praca?et={lvl}&itth={key}";
            }
            else if (lvl == 0 && location == string.Empty)
            {
                html = $@"https://it.pracuj.pl/praca?&itth={key}";
            }
            else if (location != string.Empty && lvl > 0 && key > 0)
            {
                html = $@"https://it.pracuj.pl/praca/{location};wp?rd=0&et={lvl}&itth={key}";
            }
            else if (location != string.Empty && lvl > 0 && key == 0)
            {
                html = $@"https://it.pracuj.pl/praca/{location};wp?rd=0&et={lvl}";
            }
            else if (location != string.Empty && lvl == 0 && key > 0)
            {
                html = $@"https://it.pracuj.pl/praca/{location};wp?rd=0&itth={key}";
            }
        }
    }
}
