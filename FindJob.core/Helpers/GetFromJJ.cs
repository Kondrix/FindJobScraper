using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace FindJob.core
{
    public class GetFromJJ
    {
        public async Task<ObservableCollection<JobViewModel>> GetHtmlFromJJIT(string techl, string location,string lvl)
        {
            ObservableCollection<JobViewModel> jobs = new ObservableCollection<JobViewModel>();

            if (location == null)
            {
                location = "all-locations";
            }
            Dictionary<int, string> dict = new Dictionary<int, string>();
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl($"https://justjoin.it/{location}/{techl}/experience-level_{lvl}");

            Thread.Sleep(500);
            driver.FindElement(By.Id("cookiescript_accept")).Click();

            IJavaScriptExecutor js = driver as IJavaScriptExecutor;

            int lastIndex = -1;

            while (true)
            {
                var elements = driver.FindElements(By.CssSelector("div[data-index]"));
                bool foundNewIndex = false;
                foreach (var element in elements)
                {
                    int currentIndex = Int32.Parse(element.GetAttribute("data-index"));
                    if (currentIndex > lastIndex)
                    {
                        var linkElements = element.FindElement(By.TagName("a"));
                        var hrefValue = linkElements.GetAttribute("href");

                        string h2Value = element.FindElement(By.TagName("h2")).Text;
                        dict.Add(currentIndex, h2Value);
                        
                        var job = new JobViewModel
                        {
                            JobName = h2Value,
                            JobLink = hrefValue,

                        };
                        jobs.Add(job);
                        lastIndex = currentIndex;
                        foundNewIndex = true;
                    }
                }

                if (!foundNewIndex) 
                {
                    js.ExecuteScript("window.scrollBy(0,900);");
                    Thread.Sleep(200);
                }
                var scrollTop = (long)js.ExecuteScript("return window.pageYOffset;");
                var scrollHeight = (long)js.ExecuteScript("return document.body.scrollHeight;");
                if (scrollTop + driver.Manage().Window.Size.Height >= scrollHeight)
                {
                    break; 
                }

            }
            driver.Quit();


            return jobs;

        }
    }
}
