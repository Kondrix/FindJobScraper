using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FindJob.core
{
    public class PracujClass
    {

        public async Task<ObservableCollection<JobViewModel>> GetHtmlFromPracujAsync(int key, int lvl,string location)
        {
            ObservableCollection<JobViewModel> jobs = new ObservableCollection<JobViewModel>();


            GetMaxPageClass getMaxPage = new GetMaxPageClass();

            int i;
            int maxpage = getMaxPage.GetMaxValueFromPagePracuj(key, lvl, location);

            for (i = 1; i < maxpage + 1; i++)
            {
                string html = CheckInputs(key, lvl, location, i);

                HtmlWeb web = new HtmlWeb();
                var htmlDoc = await web.LoadFromWebAsync(html);
                var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@data-test=\"offer-title\"]");
                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        var linkNode = node.SelectSingleNode(".//a");
                        string nodeText = node.InnerText;
                        if (nodeText.Contains(" "))
                        {
                            nodeText = nodeText.Replace(" ", "%20");
                        }
                        if (nodeText.Contains("/"))
                        {
                            nodeText = nodeText.Replace("/", "-x47-");
                        }

                        string jobLink = CheckJobLink(linkNode, nodeText);
                        var job = new JobViewModel
                        {
                            JobName = node.InnerText,
                            JobLink = jobLink,

                        };
                        jobs.Add(job);
                    }
                }


            }
            return jobs;

        }

        private static string CheckJobLink(HtmlNode linkNode, string nodeText)
        {
            return linkNode != null ? linkNode.GetAttributeValue("href", string.Empty) : $"https://it.pracuj.pl/praca/{nodeText.ToLower()};kw";
        }

        private static string CheckInputs(int key, int lvl, string location, int i)
        {
            var html = "";
            if (lvl > 0 && location == string.Empty)
            {
                html = $@"https://it.pracuj.pl/praca?et={lvl}&pn={i}&itth={key}";
            }
            else if (lvl == 0 && location == string.Empty)
            {
                html = $@"https://it.pracuj.pl/praca?pn={i}&itth={key}";
            }
            else if (location != string.Empty && lvl > 0 && key > 0)
            {
                html = $@"https://it.pracuj.pl/praca/{location};wp?rd=0&et={lvl}&pn={i}&itth={key}";
            }
            else if (location != string.Empty && lvl > 0 && key == 0)
            {
                html = $@"https://it.pracuj.pl/praca/{location};wp?rd=0&et={lvl}&pn={i}";
            }
            else if (location != string.Empty && lvl == 0 && key > 0)
            {
                html = $@"https://it.pracuj.pl/praca/{location};wp?rd=0&pn={i}&itth={key}";
            }

            return html;
        }
    }
}

