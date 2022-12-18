using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Helpers;

namespace WebScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(
                "Choose one of these scraping options:\n" +
                "\ty - Youtube Video\n" +
                "\tj - ICT Job\n"+
                "\tb - Book\n" +
                "Your option: "
                );

            switch (Console.ReadLine().ToLower())
            {
                case "y":
                    Console.WriteLine(
                        "You chose Youtube video\n" +
                        "Give a topic"
                        );
                    string topic = Console.ReadLine();
                    YouTubeScraping(topic);
                    break;
                case "j":
                    Console.WriteLine(
                        "You chose ICT job\n" +
                        "Give a keyword"
                        );
                    string keyword = Console.ReadLine();
                    IctJobScraping(keyword);
                    break;
                case "b":
                    Console.WriteLine(
                        "You chose book\n" +
                        "Give a searchterm"
                        );
                    string searchTerm = Console.ReadLine();
                    GoodReads(searchTerm);
                    break;
            }

            Console.Write("Press any key to close the console app...");
            Console.ReadKey();
        }

        static void YouTubeScraping(string topic)
        {
            String youTubeLink = "https://www.youtube.com/results?search_query=" + topic + "&sp=CAI%253D";
            int vcount = 1;
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Url = youTubeLink;
            var timeout = 10000;
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            Thread.Sleep(5000);

            By elem_video_link = By.CssSelector("ytd-video-renderer.style-scope.ytd-item-section-renderer");
            ReadOnlyCollection<IWebElement> videos = driver.FindElements(elem_video_link);
            Console.WriteLine("Total number of videos in " + youTubeLink + " are " + videos.Count);

            var csv = new StringBuilder();
            var json = new StringBuilder();

            foreach (IWebElement video in videos.Take(5))
            {
                string str_title, str_views, str_chan, str_link;
                IWebElement elem_video_title = video.FindElement(By.CssSelector("#video-title"));
                str_title = elem_video_title.Text;

                IWebElement elem_video_views = video.FindElement(By.XPath(".//*[@id='metadata-line']/span[1]"));
                str_views = elem_video_views.Text;

                IWebElement elem_video_channel = video.FindElement(By.CssSelector("#channel-info"));
                str_chan = elem_video_channel.Text;

                IWebElement elem_video_li = video.FindElement(By.CssSelector("[class = 'yt-simple-endpoint style-scope ytd-video-renderer']"));
                str_link = elem_video_li.GetAttribute("href");

                var Line1 = "******* Video " + vcount + " *******";
                csv.AppendLine(Line1);
                json.AppendLine(Line1);
                var Line2 = "Video Title: " + str_title;
                csv.AppendLine(Line2);
                json.AppendLine(Line2);
                var Line3 = "Video Uploader: " + str_chan;
                csv.AppendLine(Line3);
                json.AppendLine(Line3);
                var Line4 = "Video Views: " + str_views;
                csv.AppendLine(Line4);
                json.AppendLine(Line4);
                var Line5 = "Video Link: " + str_link;
                csv.AppendLine(Line5);
                json.AppendLine(Line5);
    
                Console.WriteLine("\n" + Line1 + "\n" + Line2 + "\n" + Line3 + "\n" + Line4 + "\n" + Line5 + "\n");
                vcount++;
            }

            Console.WriteLine("Scraping Data from YouTube Channel Passed");
            File.WriteAllText("C:/Users/larap/schooljaar 2022-2023/DevOps/youtube.csv", csv.ToString(), Encoding.UTF8);
            File.WriteAllText("C:/Users/larap/schooljaar 2022-2023/DevOps/youtube.json", json.ToString(), Encoding.UTF8);
        }

        static void IctJobScraping(string keyword)
        {
            String ictjobLink = "https://www.ictjob.be/nl/it-vacatures-zoeken?keywords=" + keyword + "&keywords_options=EQUALS&SortOrder=DESC&sortField=DATE&From=0&To=19";
            int jcount = 1;
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Url = ictjobLink;
            var timeout = 10000;
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            Thread.Sleep(5000);

            By elem_result = By.CssSelector("li.search-item.clearfix");
            ReadOnlyCollection<IWebElement> jobs = driver.FindElements(elem_result);
            Console.WriteLine("Total number of jobs in " + ictjobLink + " are " + jobs.Count);

            var csv = new StringBuilder();
            var json = new StringBuilder();

            foreach (IWebElement job in jobs.Take(5))
            {
                string str_title, str_company, str_location, str_link, str_keywords;

                IWebElement elem_job_title = job.FindElement(By.CssSelector("a.job-title.search-item-link"));
                str_title = elem_job_title.Text;

                IWebElement elem_job_comp = job.FindElement(By.CssSelector("span.job-company"));
                str_company = elem_job_comp.Text;

                IWebElement elem_job_loc = job.FindElement(By.CssSelector("span.job-location"));
                str_location = elem_job_loc.Text;

                IWebElement elem_job_link = job.FindElement(By.CssSelector("a.job-title.search-item-link"));
                str_link = elem_job_link.GetAttribute("href");

                IWebElement elem_job_keywords = job.FindElement(By.CssSelector("span.job-keywords"));
                str_keywords = elem_job_keywords.Text;

                var Line1 = "******* Job " + jcount + " *******";
                csv.AppendLine(Line1);
                json.AppendLine(Line1);
                var Line2 = "Job Title: " + str_title;
                csv.AppendLine(Line2);
                json.AppendLine(Line2);
                var Line3 = "Company: " + str_company;
                csv.AppendLine(Line3);
                json.AppendLine(Line3);
                var Line4 = "Company Location: " + str_location;
                csv.AppendLine(Line4);
                json.AppendLine(Line4);
                var Line5 = "Keywords: " + str_keywords;
                csv.AppendLine(Line5);
                json.AppendLine(Line5);
                var Line6 = "Detail Page: " + str_link;
                csv.AppendLine(Line6);
                json.AppendLine(Line6);

                Console.WriteLine("\n" + Line1 + "\n" + Line2 + "\n" + Line3 + "\n" + Line4 + "\n" + Line5 + "\n" + Line6 + "\n");
                jcount++;

            }

            Console.WriteLine("Scraping Data from Ict Job Passed");
            File.WriteAllText("C:/Users/larap/schooljaar 2022-2023/DevOps/jobs.csv", csv.ToString(), Encoding.UTF8);
            File.WriteAllText("C:/Users/larap/schooljaar 2022-2023/DevOps/jobs.json", json.ToString(), Encoding.UTF8);

        }

        static void GoodReads(string searchTerm)
        {
            String goodReadsLink = "https://www.goodreads.com/search?utf8=%E2%9C%93&query=" + searchTerm ;
            int bcount = 1;
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Url = goodReadsLink;
            var timeout = 10000;
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            Thread.Sleep(5000);

            By elem__books = By.TagName("tr");
            ReadOnlyCollection<IWebElement> books = driver.FindElements(elem__books);
            Console.WriteLine("Total number of books in " + goodReadsLink + " are " + books.Count);

            var csv = new StringBuilder();
            var json = new StringBuilder();

            foreach (IWebElement book in books.Take(5))
            {
                string str_title, str_author, str_rating, str_link;
                IWebElement elem_book_title = book.FindElement(By.CssSelector("a.bookTitle"));
                str_title = elem_book_title.Text;

                IWebElement elem_book_author = book.FindElement(By.CssSelector("a.authorName"));
                str_author = elem_book_author.Text;

                IWebElement elem_book_rating = book.FindElement(By.CssSelector("span.minirating"));
                str_rating = elem_book_rating.Text;

                IWebElement elem_book_link = book.FindElement(By.CssSelector("a.bookTitle"));
                str_link = elem_book_link.GetAttribute("href");

                var Line1 = "******* Book " + bcount + " *******";
                csv.AppendLine(Line1);
                json.AppendLine(Line1);
                var Line2 = "Book Title: " + str_title;
                csv.AppendLine(Line2);
                json.AppendLine(Line2);
                var Line3 = "Book Author: " + str_author;
                csv.AppendLine(Line3);
                json.AppendLine(Line3);
                var Line4 = "Book Rating: " + str_rating;
                csv.AppendLine(Line4);
                json.AppendLine(Line4);
                var Line5 = "Book Link: " + str_link;
                csv.AppendLine(Line5);
                json.AppendLine(Line5);
               
                Console.WriteLine("\n" + Line1 + "\n" + Line2 + "\n" + Line3 + "\n" + Line4 + "\n" + Line5 + "\n");
                bcount++;

            }

            Console.WriteLine("Scraping Data from GoodReads Passed");
            File.WriteAllText("C:/Users/larap/schooljaar 2022-2023/DevOps/goodReads.csv", csv.ToString(), Encoding.UTF8);
            File.WriteAllText("C:/Users/larap/schooljaar 2022-2023/DevOps/goodReads.json", json.ToString(), Encoding.UTF8);


        }
    }
}
    

